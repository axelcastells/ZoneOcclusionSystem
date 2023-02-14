using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GSystem : MonoBehaviour
{
    Managers managers;
    protected Managers Managers
    {
        get { return managers; }
    }

    public void SetupSystem(Managers managers)
    {
        this.managers = managers;
    }
    public abstract void InitializeSystem();

    public abstract void UpdateSystem();
}
