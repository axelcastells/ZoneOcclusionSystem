using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ZonesSystem : GSystem
{
    [SerializeField] private CharacterFactorySystem _characterFactorySystem;

    private List<ZoneController> _zoneControllers = new List<ZoneController>();
    private List<LocatableData> _locatableDataList = new List<LocatableData>();

    [SerializeField] private List<ZonePair> _neighbourRelations = new List<ZonePair>();

    public List<ZonePair> NeighbourRelations
    {
        get { return _neighbourRelations; }
    }


    public UnityEvent<ILocatable, ZoneController> OnLocatableEnteredZone = new UnityEvent<ILocatable, ZoneController>();
    public UnityEvent<ILocatable, ZoneController> OnLocatableLeftZone = new UnityEvent<ILocatable, ZoneController>();

    public List<ZoneController> Zones
    {
        get { return _zoneControllers; }
    }

    public List<ZoneController> GetNeighbours(ZoneController zoneController)
    {
        List<ZoneController> neighbours = new List<ZoneController>();
        
        for(int i = 0; i < _neighbourRelations.Count; i++)
        {
            if (zoneController.Equals(_neighbourRelations[i].zoneA))
                neighbours.Add(_neighbourRelations[i].zoneB);
            else if (zoneController.Equals(_neighbourRelations[i].zoneB))
                neighbours.Add(_neighbourRelations[i].zoneA);
        }

        neighbours.Add(zoneController);
        neighbours.RemoveDuplicates();
        return neighbours;
    }

    public override void InitializeSystem()
    {
        FetchZoneControllers();

        _characterFactorySystem.OnCharacterSpawned.AddListener((potentialLocatable) =>
        {
            if (potentialLocatable is ILocatable)
                _locatableDataList.Add(new LocatableData(potentialLocatable));
        });
    }

    private void FetchZoneControllers()
    {
        _zoneControllers = FindObjectsOfType<ZoneController>().ToList();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < _neighbourRelations.Count; i++)
        {
            if (_neighbourRelations[i].zoneA != null && _neighbourRelations[i].zoneB != null)
                Gizmos.DrawLine(_neighbourRelations[i].zoneA.Bounds.center, _neighbourRelations[i].zoneB.Bounds.center);
        }
    }

    public override void UpdateSystem()
    {
        for (int i = 0; i < _locatableDataList.Count; i++)
        {
            for (int j = 0; j < _zoneControllers.Count; j++)
            {
                if (_zoneControllers[j].Contains(_locatableDataList[i].locatable.GetPosition()) && !_locatableDataList[i].currentZones.Contains(_zoneControllers[j]))
                {
                    OnLocatableEnteredZone.Invoke(_locatableDataList[i].locatable, _zoneControllers[j]);
                    _locatableDataList[i].currentZones.Add(_zoneControllers[j]);
                }
                else if(!_zoneControllers[j].Contains(_locatableDataList[i].locatable.GetPosition()) && _locatableDataList[i].currentZones.Contains(_zoneControllers[j]))
                {
                    OnLocatableLeftZone.Invoke(_locatableDataList[i].locatable, _zoneControllers[j]);
                    _locatableDataList[i].currentZones.Remove(_zoneControllers[j]);
                }
            }
        }
    }
}

[System.Serializable]
public struct ZonePair
{
    public ZoneController zoneA;
    public ZoneController zoneB;
}

[System.Serializable]
public class LocatableData
{
    public ILocatable locatable = null;
    public List<ZoneController> currentZones = new List<ZoneController>();

    public LocatableData(ILocatable locatable)
    {
        this.locatable = locatable;
    }
}
