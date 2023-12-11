using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Core;
using Demyth.Gameplay;

public class GroundSummon : MonoBehaviour
{
    [Title("Object Timeline")]
    [SerializeField, OnValueChanged("CalculateTotalDuration")] private float anticipationDuration;
    [SerializeField, OnValueChanged("CalculateTotalDuration")] private float attackDuration;
    [SerializeField, OnValueChanged("CalculateTotalDuration")] private float recoveryDuration;
    [SerializeField] private bool isEndless;
    [ReadOnly] public float TotalDuration;

    [Title("Randomized Spawn")]
    [SerializeField] private bool useRandomizedSpawnDelay;
    [ShowIf("useRandomizedSpawnDelay")]
    [SerializeField] private float minRandomSpawnDelay;
    [ShowIf("useRandomizedSpawnDelay")]
    [SerializeField] private float maxRandomSpawnDelay = 0.1f;

    [Title("External Components")]
    [SerializeField] private GameObject colliderGameObject;
    [SerializeField] private GameObject[] groundCoffinModelArray;

    public Action OnAnticipation;
    public Action OnAttack;
    public Action OnRecovery;

    private int topBorder = 2;
    private int bottomBorder = -4;
    private int rightBorder = 6;
    private int leftBorder = -6;

    private GameStateService _gameStateService;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();

        _gameStateService[GameState.MainMenu].onEnter += MainMenu_OnEnter;
        _gameStateService[GameState.GameOver].onEnter += GameOver_OnEnter;
    }

    private void Start() 
    {
        if (IsOutOfBounds())
        {
            Destroy(gameObject);
            return;
        }

        ActivateRandomModel();
        StartCoroutine(SummonRoutine());
    }

    private void OnDestroy() 
    {
        _gameStateService[GameState.MainMenu].onEnter -= MainMenu_OnEnter;
        _gameStateService[GameState.GameOver].onEnter -= GameOver_OnEnter;
    }

    private IEnumerator SummonRoutine()
    {
        if (useRandomizedSpawnDelay)
            yield return StartCoroutine(RandomizedSpawnDelay());

        OnAnticipation?.Invoke();
        yield return Helper.GetWaitForSeconds(anticipationDuration);

        OnAttack?.Invoke();
        colliderGameObject.SetActive(true);
        yield return Helper.GetWaitForSeconds(attackDuration);

        if (isEndless) yield break;

        OnRecovery?.Invoke();
        colliderGameObject.SetActive(false);
        yield return Helper.GetWaitForSeconds(recoveryDuration);

        Destroy(gameObject);
    }

    private void MainMenu_OnEnter(GameState state)
    {
        Destroy(gameObject);
    }

    private void GameOver_OnEnter(GameState state)
    {
        Destroy(gameObject);
    }
    
    private bool IsOutOfBounds()
    {
        float positionY = transform.position.y;
        float positionX = transform.position.x;

        return positionY > topBorder || positionY < bottomBorder || positionX > rightBorder || positionX < leftBorder;
    }

    private IEnumerator RandomizedSpawnDelay()
    {
        var delayTime = UnityEngine.Random.Range(minRandomSpawnDelay, maxRandomSpawnDelay);
        yield return Helper.GetWaitForSeconds(delayTime);
    }

    private void ActivateRandomModel()
    {
        var randomIndex = UnityEngine.Random.Range(0, groundCoffinModelArray.Length);
        groundCoffinModelArray[randomIndex].SetActive(true);
    }

    private void CalculateTotalDuration()
    {
        TotalDuration = anticipationDuration + attackDuration + recoveryDuration;
    }
}
