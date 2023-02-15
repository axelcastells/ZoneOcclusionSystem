using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : GSystem
{
    [SerializeField] ZoneOcclusionSystem _zoneOcclusionSystem;
    [SerializeField] ZonesSystem _zonesSystem;

    [SerializeField] UISystemGUI ui;
    public override void InitializeSystem()
    {

    }

    public override void UpdateSystem()
    {
        ui.SetHiddenCount(_zoneOcclusionSystem.GetHiddenOccludablesCount(), _zoneOcclusionSystem.GetTotalOccludablesCount());
        ui.SetRelationsText(_zonesSystem.NeighbourRelations);
    }
}
