using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZonesSystem : GSystem
{
    [SerializeField] private PlayerSystem playerSystem;

    private List<ZoneController> zoneControllers = new List<ZoneController>();
    public override void InitializeSystem()
    {
        zoneControllers = FindObjectsOfType<ZoneController>().ToList();
    }

    public override void UpdateSystem()
    {
        for(int i = 0; i < zoneControllers.Count; i++)
        {
            
        }
    }
}
