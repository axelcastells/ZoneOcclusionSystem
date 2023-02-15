using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterFactorySystem : GSystem
{
    private static Vector3 NOWHERE = new Vector3(100000,100000,100000);

    [SerializeField] private CharacterController characterPrefab;
    [SerializeField] private int characterPoolingAmount = 100;

    private Queue<CharacterController> charactersPool = new Queue<CharacterController>();
    private List<IPawn> spawnedCharacters = new List<IPawn>();

    public UnityEvent<IPawn> OnCharacterSpawned = new UnityEvent<IPawn>();
    public UnityEvent<IPawn> OnCharacterDestroyed = new UnityEvent<IPawn>();

    public List<IPawn> GetSpawnedCharacters() { return spawnedCharacters; }

    public override void InitializeSystem()
    {
        PoolCharacters();
    }

    public override void UpdateSystem()
    {

    }

    public CharacterController SpawnCharacter(Vector3 position)
    {
        CharacterController newCharacter = null;

        if (charactersPool.Count > 0)
            newCharacter = charactersPool.Dequeue().Setup(position);
        else
            newCharacter = InstantiateCharacter(position);

        newCharacter.OnDiedEvent.AddListener((character) =>
        {
            OnCharacterDestroyed.Invoke(character);
            spawnedCharacters.Remove(character);
        });

        spawnedCharacters.Add(newCharacter);
        OnCharacterSpawned.Invoke(newCharacter);
        return newCharacter;
    }

    private CharacterController InstantiateCharacter(Vector3 position)
    {
        return Instantiate(characterPrefab).Setup(position);
    }

    private void PoolCharacters()
    {
        for (int i = 0; i < characterPoolingAmount; i++)
            charactersPool.Enqueue(InstantiateCharacter(NOWHERE));
    }
}
