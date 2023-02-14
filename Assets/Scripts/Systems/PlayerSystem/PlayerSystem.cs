using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : GSystem
{
    IPawn controlledPawn = null;

    public void Possess(IPawn pawn)
    {
        controlledPawn = pawn;
    }
    public override void InitializeSystem()
    {
        
    }

    public override void UpdateSystem()
    {
        if(controlledPawn != null)
            controlledPawn.Move(Managers.inputManager.GetMoveDirection());
    }
}
