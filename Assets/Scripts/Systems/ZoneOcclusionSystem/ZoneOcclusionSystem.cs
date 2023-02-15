using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneOcclusionSystem : GSystem
{
    [SerializeField] PlayerSystem _playerSystem;
    [SerializeField] ZonesSystem _zonesSystem;

    private OccludableData playerOccludableData = null;
    private List<OccludableData> _knownOccludablesList = new List<OccludableData>();

    private int _currentHiddenOccludablesCount = 0;
    OccludableData GetOccludabledata(IOccludable occludable)
    {
        return _knownOccludablesList.Find(x => x.occludable == occludable);
    }

    public List<OccludableData> GetKnownOccludables()
    {
        return _knownOccludablesList;
    }

    public int GetHiddenOccludablesCount()
    {
        return _currentHiddenOccludablesCount;
    }

    public override void InitializeSystem()
    {
        _playerSystem.OnPawnPossessed.AddListener((pawn) =>
        {
            Debug.Log("POSSESSION");
            if (pawn is IOccludable)
                playerOccludableData = GetOccludabledata(pawn as IOccludable);
        });

        _zonesSystem.OnLocatableEnteredZone.AddListener((locatable, zone) =>
        {
            if (locatable is IOccludable)
                AddOccludableIfUnknown(locatable as IOccludable).currentZones.Add(zone);
            Debug.Log($"Entered Zone");
        });

        _zonesSystem.OnLocatableLeftZone.AddListener((locatable, zone) =>
        {
            if (locatable is IOccludable)
                AddOccludableIfUnknown(locatable as IOccludable).currentZones.Remove(zone);
            Debug.Log($"Left Zone");
        });
    }

    private OccludableData AddOccludableIfUnknown(IOccludable occludable)
    {
        OccludableData newOccludableData = _knownOccludablesList.Find(l => l.occludable == occludable);

        if (newOccludableData == null)
        {
            newOccludableData = new OccludableData(occludable);
            _knownOccludablesList.Add(newOccludableData);
        }

        return newOccludableData;
    }

    public override void UpdateSystem()
    {
        playerOccludableData = GetOccludabledata(_playerSystem.ControlledPawn as IOccludable);

        if (playerOccludableData != null)
        {
            for (int i = 0; i < _knownOccludablesList.Count; i++)
            {
                for (int j = 0; j < _knownOccludablesList[i].currentZones.Count; j++)
                {
                    if (AreZonesWithinReach(playerOccludableData.currentZones, _knownOccludablesList[i].currentZones[j]) && _knownOccludablesList[i].occludable.IsHidden())
                        ShowOccludable(_knownOccludablesList[i].occludable);
                    else if (!AreZonesWithinReach(playerOccludableData.currentZones, _knownOccludablesList[i].currentZones[j]) && !_knownOccludablesList[i].occludable.IsHidden())
                        HideOccludable(_knownOccludablesList[i].occludable);
                }
            }
        }
    }

    void HideOccludable(IOccludable occludable)
    {
        _currentHiddenOccludablesCount++;
        occludable.Hide();
    }

    void ShowOccludable(IOccludable occludable)
    {
        _currentHiddenOccludablesCount = Mathf.Clamp(_currentHiddenOccludablesCount - 1, 0, _knownOccludablesList.Count);
        occludable.Show();
    }

    bool AreZonesWithinReach(List<ZoneController> potentiallyReachableZones, ZoneController targetZone)
    {
        List<ZoneController> targetZoneNeigbours = _zonesSystem.GetNeighbours(targetZone);

        for(int i = 0; i < targetZoneNeigbours.Count; i++)
        {
            if (potentiallyReachableZones.Contains(targetZoneNeigbours[i]))
                return true;
        }

        return false;
    }
    
    [System.Serializable]
    public class OccludableData
    {
        public IOccludable occludable = null;
        public List<ZoneController> currentZones = new List<ZoneController>();

        public OccludableData(IOccludable occludable)
        {
            this.occludable = occludable;
        }
    }
}
