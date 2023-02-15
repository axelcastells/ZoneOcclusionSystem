using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : GSystem
{
    [SerializeField] ZoneOcclusionSystem _zoneOcclusionSystem;
    [SerializeField] ZonesSystem _zonesSystem;
    [SerializeField] CharacterFactorySystem _characterFactorySystem;

    [SerializeField] UISystemGUI ui;
    public override void InitializeSystem()
    {

    }

    public override void UpdateSystem()
    {
        ui.SetHiddenCount(_zoneOcclusionSystem.GetHiddenOccludablesCount(), _characterFactorySystem.GetSpawnedCharacters().Count);
        ui.SetRelationsText(_zonesSystem.NeighbourRelations);
    }
}
