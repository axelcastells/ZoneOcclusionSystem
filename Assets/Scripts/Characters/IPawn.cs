using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPawn
{
    public void Move(Vector3 direction);
    public void SetPosition(Vector3 worldPosition);
    public Vector3 GetPosition();
}
