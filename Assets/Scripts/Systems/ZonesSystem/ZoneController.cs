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

    public UnityEvent<ZoneController> OnInspectorModified = new UnityEvent<ZoneController>();
    public Bounds Bounds
    {
        get
        {
            if (area == null)
                area = GetComponent<BoxCollider>();
            return area.bounds;
        }
    }

    private void OnValidate()
    {
        Debug.Log("VALIDATE");
        OnInspectorModified.Invoke(this);
        // Remove Duplicates
        //_neighbourZones.RemoveDuplicates();

        //for (int i = 0; i < this._neighbourZones.Count; i++)
        //{
        //    _neighbourZones[i]._neighbourZones.RemoveAll(x => x.Equals(this));

        //    if (!_neighbourZones[i]._neighbourZones.Contains(this))
        //    {
        //        _neighbourZones[i]._neighbourZones.Add(this);
        //    }
        //}
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
