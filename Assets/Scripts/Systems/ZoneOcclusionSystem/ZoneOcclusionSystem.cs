using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class ZoneOcclusionSystem : GSystem
{
    [SerializeField] PlayerSystem _playerSystem;
    [SerializeField] ZonesSystem _zonesSystem;
    [SerializeField] CharacterFactorySystem _characterFactorySystem;

    private OccludableData playerOccludableData = null;
    private List<OccludableData> _knownOccludablesList = new List<OccludableData>();

    private int _currentHiddenOccludablesCount = 0;
    OccludableData GetOccludableData(IOccludable occludable)
    {
        return _knownOccludablesList.Find(x => x.occludable == occludable);
    }

    void RemoveNullOccludablesFromList()
    {
        _knownOccludablesList.RemoveAll(x => x.occludable == null);
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
        _characterFactorySystem.OnCharacterDestroyed.AddListener((pawn) =>
        {
            if(pawn is IOccludable)
            {
                RemoveNullOccludablesFromList();
                OccludableData selectedOccludable = GetOccludableData(pawn as IOccludable);
                if (_knownOccludablesList.Contains(selectedOccludable))
                    _knownOccludablesList.Remove(selectedOccludable);
            }
        });

        _characterFactorySystem.OnCharacterSpawned.AddListener((pawn) =>
        {
            if(pawn is IOccludable)
                GetOrAddOccludable(pawn as IOccludable);
        });

        _playerSystem.OnPawnPossessed.AddListener((pawn) =>
        {
            if (pawn is IOccludable)
                playerOccludableData = GetOccludableData(pawn as IOccludable);
        });

        _zonesSystem.OnLocatableEnteredZone.AddListener((locatable, zone) =>
        {
            if (locatable is IOccludable)
                GetOrAddOccludable(locatable as IOccludable).currentZones.Add(zone);
        });

        _zonesSystem.OnLocatableLeftZone.AddListener((locatable, zone) =>
        {
            if (locatable is IOccludable)
                GetOrAddOccludable(locatable as IOccludable).currentZones.Remove(zone);
        });
    }

    private OccludableData GetOrAddOccludable(IOccludable occludable)
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
        playerOccludableData = GetOccludableData(_playerSystem.ControlledPawn as IOccludable);

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
