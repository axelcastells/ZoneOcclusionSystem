using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour, IPawn, ILocatable, IOccludable
{
    [SerializeField] private float speed = 3f;

    [Header("Gizmo Variables")]
    [SerializeField] private float yGizmoDistance = 1f;
    [SerializeField] private float gizmoRadius = .1f;

    private bool isHidden = false;

    private void OnDrawGizmos()
    {
        if (isHidden)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawSphere(transform.position + (Vector3.up * yGizmoDistance), gizmoRadius);
    }
    public CharacterController Setup(Vector3 position)
    {
        SetPosition(position);
        return this;
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Show()
    {
        isHidden = false;
    }

    public void Hide()
    {
        isHidden = true;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
