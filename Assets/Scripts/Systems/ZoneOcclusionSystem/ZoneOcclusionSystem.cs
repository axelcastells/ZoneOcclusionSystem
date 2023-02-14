using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneOcclusionSystem : GSystem
{
    [SerializeField] ZonesSystem _zonesSystem;
    

    public override void InitializeSystem()
    {
        _zonesSystem.OnLocatableEnteredZone.AddListener((locatable, zone) =>
        {
            Debug.Log($"Entered Zone");
        });

        _zonesSystem.OnLocatableLeftZone.AddListener((locatable, zone) =>
        {
            Debug.Log($"Left Zone");
        });
    }

    public override void UpdateSystem()
    {

    }
}
