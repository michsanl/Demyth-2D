using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Demyth.Gameplay;
using Core;
using Lean.Pool;

public class GameObjectSpawnController : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private float _despawnTimer = 2f;
    [SerializeField] private GameObject[] gameObjectsToSpawnArray;

    private GameStateService _gameStateService;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();

        _gameStateService[GameState.MainMenu].onEnter += MainMenu_OnEnter;
        _gameStateService[GameState.GameOver].onEnter += GameOver_OnEnter;
    }
    private void OnEnable()
    {
        StartCoroutine(ActivateGameObjectWithInterval());
    }

    private void OnDisable()
    {
        foreach (var gameObject in gameObjectsToSpawnArray)
        {
            gameObject.gameObject.SetActive(false);
        }
    }

    private void OnDestroy() 
    {
        _gameStateService[GameState.MainMenu].onEnter -= MainMenu_OnEnter;
        _gameStateService[GameState.GameOver].onEnter -= GameOver_OnEnter;
    }

    private IEnumerator ActivateGameObjectWithInterval()
    {
        foreach (var gameObject in gameObjectsToSpawnArray)
        {
            gameObject.gameObject.SetActive(true);
            yield return Helper.GetWaitForSeconds(spawnInterval);
        }

        yield return Helper.GetWaitForSeconds(_despawnTimer);
        LeanPool.Despawn(gameObject);
    }

    private void MainMenu_OnEnter(GameState state)
    {
        LeanPool.Despawn(gameObject);
    }

    private void GameOver_OnEnter(GameState state)
    {
        LeanPool.Despawn(gameObject);
    }
}
