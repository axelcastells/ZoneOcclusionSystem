using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class ZoneController : MonoBehaviour
{
    [SerializeField] Color zoneColor = Color.black;
    [SerializeField] List<ZoneController> neighbourZones;

    private BoxCollider area;
    private BoxCollider Area
    {
        get
        {
            if(area == null)
                area = GetComponent<BoxCollider>();
            return area;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = zoneColor;
        Gizmos.DrawWireCube(Area.bounds.center, Area.bounds.size);

        Gizmos.color = Color.white;
        for(int i = 0; i < neighbourZones.Count; i++)
        {
            Gizmos.DrawLine(Area.bounds.center, neighbourZones[i].GetBounds().center);
        }
    }

    public Bounds GetBounds()
    {
        return Area.bounds;
    }

    public bool Contains(Vector3 position)
    {
        return Area.bounds.Contains(position);
    }
}
