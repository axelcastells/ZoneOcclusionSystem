using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Managers
    [Header("Managers")]
    [SerializeField] private Managers managers;

    // Systems
    [Header("Systems")]
    [SerializeField] private List<GSystem> gameSystems;

    private void Awake()
    {
        // Init Managers
        managers.inputManager.InitializeManager();

        // Init Systems
        for (int i = 0; i < gameSystems.Count; i++)
            gameSystems[i].SetupSystem(managers);
    }

    private void Start()
    {
        for (int i = 0; i < gameSystems.Count; i++)
            gameSystems[i].InitializeSystem();
    }

    private void Update()
    {
        for (int i = 0; i < gameSystems.Count; i++)
            gameSystems[i].UpdateSystem();
    }
}

[System.Serializable]
public struct Managers
{
    public InputManager inputManager;
}