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
    [SerializeField] private KeyCode actionAKey = KeyCode.Space;
    [SerializeField] private KeyCode actionBKey = KeyCode.E;

    public UnityEvent OnActionAPressed;
    public UnityEvent OnActionBPressed;
    public UnityEvent OnActionAReleased;
    public UnityEvent OnActionBReleased;

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

        if (Input.GetKeyDown(actionAKey))
            OnActionAPressed.Invoke();        
        else if (Input.GetKeyUp(actionAKey))
            OnActionAReleased.Invoke();

        if (Input.GetKeyDown(actionBKey))
            OnActionBPressed.Invoke();
        else if (Input.GetKeyUp(actionBKey))
            OnActionBReleased.Invoke();

        moveInputDirection.Normalize();
    }
}
