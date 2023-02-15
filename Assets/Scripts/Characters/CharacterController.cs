using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour, IPawn, ILocatable, IOccludable
{
    private float speed = 3f;
    [SerializeField] Transform avatarTransform;
    [SerializeField] List<Renderer> visibleParts;

    [Header("Gizmo Variables")]
    [SerializeField] private float yGizmoDistance = 1f;
    [SerializeField] private float gizmoRadius = .1f;

    public UnityEvent<CharacterController> OnDied = new UnityEvent<CharacterController>();

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

    public CharacterController SetSpeed(float speed)
    {
        this.speed = speed;
        return this;
    }

    public CharacterController SetColor(Color color)
    {
        for (int i = 0; i < visibleParts.Count; i++) 
            visibleParts[i].material.color = color;

        return this;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Show()
    {
        isHidden = false;
        avatarTransform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        isHidden = true;
        avatarTransform.gameObject.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool IsHidden()
    {
        return isHidden;
    }

    public void Die()
    {
        OnDied.Invoke(this);
        gameObject.SetActive(false);
    }
}
