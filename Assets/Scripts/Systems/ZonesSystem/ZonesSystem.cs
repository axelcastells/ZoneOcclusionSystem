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


    public UnityEvent<ILocatable, ZoneController> OnLocatableEnteredZone = new UnityEvent<ILocatable, ZoneController>();
    public UnityEvent<ILocatable, ZoneController> OnLocatableLeftZone = new UnityEvent<ILocatable, ZoneController>();

    public override void InitializeSystem()
    {
        _zoneControllers = FindObjectsOfType<ZoneController>().ToList();
        _characterFactorySystem.OnCharacterSpawned.AddListener((potentialLocatable) => 
        {
            if(potentialLocatable is ILocatable)
                _locatableDataList.Add(new LocatableData(potentialLocatable)); 
        });

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
public class LocatableData
{
    public ILocatable locatable = null;
    public List<ZoneController> currentZones = new List<ZoneController>();

    public LocatableData(ILocatable locatable)
    {
        this.locatable = locatable;
    }
}
