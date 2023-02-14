using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneOcclusionSystem : GSystem
{
    [SerializeField] PlayerSystem _playerSystem;
    [SerializeField] ZonesSystem _zonesSystem;

    private OccludableData playerOccludableData = null;
    private List<OccludableData> _knownLocatablesList = new List<OccludableData>();

    OccludableData GetPlayerOccludableData()
    {
        return _knownLocatablesList.Find(x => x.occludable == _playerSystem.ControlledPawn);
    }

    public override void InitializeSystem()
    {
        _zonesSystem.OnLocatableEnteredZone.AddListener((locatable, zone) =>
        {
            if (locatable is IOccludable)
                AddOccludableIfUnknown(locatable as IOccludable).currentZones.Add(zone);
            Debug.Log($"Entered Zone");
        });

        _zonesSystem.OnLocatableLeftZone.AddListener((locatable, zone) =>
        {
            if(locatable is IOccludable)
                AddOccludableIfUnknown(locatable as IOccludable).currentZones.Remove(zone);
            Debug.Log($"Left Zone");
        });
    }

    private OccludableData AddOccludableIfUnknown(IOccludable occludable)
    {
        OccludableData newOccludableData = _knownLocatablesList.Find(l => l.occludable == occludable);

        if (newOccludableData == null)
        {
            newOccludableData = new OccludableData(occludable);
            _knownLocatablesList.Add(newOccludableData);
        }

        return newOccludableData;
    }

    public override void UpdateSystem()
    {
        playerOccludableData = GetPlayerOccludableData();

        if (playerOccludableData != null)
        {
            for (int i = 0; i < _knownLocatablesList.Count; i++)
            {
                for (int j = 0; j < _knownLocatablesList[i].currentZones.Count; j++)
                {
                    if (playerOccludableData.currentZones.Contains(_knownLocatablesList[i].currentZones[j]) && _knownLocatablesList[i].occludable.IsHidden())
                        _knownLocatablesList[i].occludable.Show();
                    else if(!playerOccludableData.currentZones.Contains(_knownLocatablesList[i].currentZones[j]) && !_knownLocatablesList[i].occludable.IsHidden())
                        _knownLocatablesList[i].occludable.Hide();
                }
            }
        }
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
