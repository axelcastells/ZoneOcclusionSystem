using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : GManager
{
    private Vector2 moveInputDirection;

    [SerializeField] private KeyCode forwardMoveKey = KeyCode.W;
    [SerializeField] private KeyCode backwardMoveKey = KeyCode.S;
    [SerializeField] private KeyCode leftMoveKey = KeyCode.A;
    [SerializeField] private KeyCode rightMoveKey = KeyCode.D;
    public override void InitializeManager()
    {

    }

    public Vector2 GetMoveDirection()
    {
        return moveInputDirection;
    }

    private void Update()
    {
        CalculateMovementInputs();
    }

    private void CalculateMovementInputs()
    {
        moveInputDirection.x = 0;
        moveInputDirection.y = 0;

        if (Input.GetKey(forwardMoveKey))
            moveInputDirection.y = 1;
        else if (Input.GetKey(backwardMoveKey))
            moveInputDirection.y = -1;
        if (Input.GetKey(leftMoveKey))
            moveInputDirection.x = -1;
        else if (Input.GetKey(rightMoveKey))
            moveInputDirection.x = 1;

        moveInputDirection.Normalize();
    }
}
