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
    public Bounds Bounds
    {
        get
        {
            if(area == null)
                area = GetComponent<BoxCollider>();
            return area.bounds;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = zoneColor;
        Gizmos.DrawWireCube(Bounds.center, Bounds.size);

        Gizmos.color = Color.white;
        for(int i = 0; i < neighbourZones.Count; i++)
        {
            Gizmos.DrawLine(Bounds.center, neighbourZones[i].Bounds.center);
        }
    }

    public bool Contains(Vector3 position)
    {
        return Bounds.Contains(position);
    }
}
