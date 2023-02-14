using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocatable
{
    Vector3 GetPosition();
    void Show();
    void Hide();
}
