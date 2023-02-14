using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : GSystem
{
    [SerializeField] private CharacterFactorySystem _characterFactorySystem;
    [SerializeField] private CameraSystem _cameraSystem;
    [SerializeField] private Transform _spawnPoint;

    private Vector3 convertedDirection = Vector3.zero;

    IPawn _controlledPawn = null;

    public void Possess(IPawn pawn)
    {
        _controlledPawn = pawn;
    }

    void SpawnAndPossessCharacter()
    {
        IPawn newPawn = _characterFactorySystem.SpawnCharacter(_spawnPoint.position);
        _cameraSystem.FollowTarget(newPawn.GetTransform());
        Possess(newPawn);
    }

    public override void InitializeSystem()
    {
        SpawnAndPossessCharacter();
    }

    public override void UpdateSystem()
    {
        if(_controlledPawn != null)
        {
            convertedDirection.x = Managers.inputManager.GetMoveDirection().x;
            convertedDirection.z = Managers.inputManager.GetMoveDirection().y;
            _controlledPawn.Move(convertedDirection);
        }
    }
}
