using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyAISystem : GSystem
{
    [SerializeField] private PlayerSystem _playerSystem;
    [SerializeField] private CharacterFactorySystem _characterFactorySystem;
    [SerializeField] private float enemyDeathDistanceToPlayer = 2f;
    [SerializeField] private float enemiesSpeed = 2f;
    [SerializeField] private Color enemiesColor = Color.red;
    [SerializeField] private float randomDirectionRange = 1f;
    [SerializeField] private int _initialEnemyAmount = 50;

    private List<IPawn> controlledPawns = new List<IPawn>();
    private Dictionary<IPawn, AIState> aiStates = new Dictionary<IPawn, AIState>();

    private BoxCollider area;
    public Bounds Bounds
    {
        get
        {
            if (area == null)
                area = GetComponent<BoxCollider>();
            return area.bounds;
        }
    }
    public override void InitializeSystem()
    {
        SpawnInitialEnemies();

        Managers.inputManager.OnActionBPressed.AddListener(SpawnEnemy);
    }

    void Possess(IPawn pawn)
    {
        if (!controlledPawns.Contains(pawn))
        {
            controlledPawns.Add(pawn);
            SetupPawnState(pawn);
        }
    }

    AIState GetState(IPawn pawn)
    {
        if (!aiStates.ContainsKey(pawn))
            aiStates.Add(pawn, new AIState());
        return aiStates[pawn];
    }

    private void SetupPawnState(IPawn pawn)
    {
        AIState state = new AIState();
        state.direction = pawn.GetTransform().forward;
        aiStates.Add(pawn, state);
    }

    void SpawnInitialEnemies()
    {
        for (int i = 0; i < _initialEnemyAmount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(Bounds.center.x - Bounds.extents.x, Bounds.center.x + Bounds.extents.x),
            0, Random.Range(Bounds.center.z - Bounds.extents.z, Bounds.center.z + Bounds.extents.z));
        Possess(_characterFactorySystem.SpawnCharacter(randomSpawnPosition).SetSpeed(enemiesSpeed).SetColor(enemiesColor));
    }

    public override void UpdateSystem()
    {
        for(int i = 0; i < controlledPawns.Count; i++)
            UpdatePawnBehaviour(controlledPawns[i]);
    }

    void UpdatePawnBehaviour(IPawn pawn)
    {
        AIState state = GetState(pawn);

        if(!Bounds.Contains(pawn.GetPosition()))
            state.direction = -state.direction;

        state.direction = Quaternion.AngleAxis(Random.Range(-randomDirectionRange, randomDirectionRange), Vector2.up) * state.direction;
        
        pawn.Move(state.direction);

        if (IsNearPlayer(pawn, enemyDeathDistanceToPlayer))
            pawn.Die();
    }

    bool IsNearPlayer(IPawn pawn, float distance)
    {
        if (Vector3.Distance(_playerSystem.ControlledPawn.GetPosition(), pawn.GetPosition()) < distance)
            return true;
        else
            return false;
    }
}

[System.Serializable]
public class AIState
{
    public Vector3 direction;
}
