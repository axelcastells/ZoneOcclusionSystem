using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSystem : GSystem
{
    [SerializeField] private ZonesSystem _zoneSystem;

    [SerializeField] private float playerSpeed = 4f;
    [SerializeField] private Color playerColor = Color.cyan;
    [SerializeField] private CharacterFactorySystem _characterFactorySystem;
    [SerializeField] private CameraSystem _cameraSystem;
    [SerializeField] private Transform _spawnPoint;

    private Vector3 convertedDirection = Vector3.zero;

    IPawn _controlledPawn = null;
    public IPawn ControlledPawn { get { return _controlledPawn; } }

    public UnityEvent<IPawn> OnPawnPossessed = new UnityEvent<IPawn>();

    public void Possess(IPawn pawn)
    {
        _controlledPawn = pawn;
        OnPawnPossessed.Invoke(_controlledPawn);
    }

    void SpawnAndPossessCharacter()
    {
        IPawn newPawn = _characterFactorySystem.SpawnCharacter(_spawnPoint.position).SetSpeed(playerSpeed).SetColor(playerColor);
        _cameraSystem.FollowTarget(newPawn.GetTransform());
        Possess(newPawn);
    }

    public override void InitializeSystem()
    {
        SpawnAndPossessCharacter();

        Managers.inputManager.OnActionAPressed.AddListener(() =>
        {
            TeleportPlayer(_zoneSystem.Zones[Random.Range(0, _zoneSystem.Zones.Count)].Bounds.center);
        });
    }

    void TeleportPlayer(Vector3 position)
    {
        ControlledPawn.SetPosition(position);
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
