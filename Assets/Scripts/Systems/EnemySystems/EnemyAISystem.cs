using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyAISystem : GSystem
{
    [SerializeField] private CharacterFactorySystem _characterFactorySystem;
    [SerializeField] private int _initialEnemyAmount = 50;

    private List<IPawn> controlledPawns = new List<IPawn>();

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
    }

    void Possess(IPawn pawn)
    {
        if(!controlledPawns.Contains(pawn))
            controlledPawns.Add(pawn);
    }

    void SpawnInitialEnemies()
    {
        for (int i = 0; i < _initialEnemyAmount; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(Bounds.center.x - Bounds.extents.x, Bounds.center.x + Bounds.extents.x),
                0, Random.Range(Bounds.center.z - Bounds.extents.z, Bounds.center.z + Bounds.extents.z));
            Possess(_characterFactorySystem.SpawnCharacter(randomSpawnPosition));
        }
    }

    public override void UpdateSystem()
    {
        for(int i = 0; i < controlledPawns.Count; i++)
            UpdatePawnBehaviour(controlledPawns[i]);
    }

    void UpdatePawnBehaviour(IPawn pawn)
    {

    }
}
