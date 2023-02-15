using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class ZoneController : MonoBehaviour
{
    [SerializeField] Color zoneColor = Color.black;

    private BoxCollider area;

    public Bounds Bounds
    {
        get
        {
            if (area == null)
                area = GetComponent<BoxCollider>();
            return area.bounds;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = zoneColor;
        Gizmos.DrawCube(Bounds.center, new Vector3(Bounds.size.x, 0, Bounds.size.z));
    }
    public bool Contains(Vector3 position)
    {
        return Bounds.Contains(position);
    }
}
