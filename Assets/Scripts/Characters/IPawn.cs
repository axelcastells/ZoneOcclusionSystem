using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPawn
{
    public Transform GetTransform();
    public void Move(Vector3 direction);
    public void SetPosition(Vector3 worldPosition);
    public Vector3 GetPosition();
    public void Die();
    public UnityEvent<IPawn> OnDied();
}
