using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocatable
{
    Vector3 GetPosition();

}

public interface IOccludable
{
    void Show();
    void Hide();
}