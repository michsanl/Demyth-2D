using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class GroundSummon : MonoBehaviour
{
    [Title("Object Timeline")]
    [SerializeField, OnValueChanged("CalculateTotalDuration")] private float anticipationDuration;
    [SerializeField, OnValueChanged("CalculateTotalDuration")] private float attackDuration;
    [SerializeField, OnValueChanged("CalculateTotalDuration")] private float recoveryDuration;
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

    private IEnumerator SummonRoutine()
    {
        if (useRandomizedSpawnDelay)
            yield return StartCoroutine(RandomizedSpawnDelay());

        OnAnticipation?.Invoke();
        yield return Helper.GetWaitForSeconds(anticipationDuration);

        OnAttack?.Invoke();
        colliderGameObject.SetActive(true);
        yield return Helper.GetWaitForSeconds(attackDuration);

        OnRecovery?.Invoke();
        colliderGameObject.SetActive(false);
        yield return Helper.GetWaitForSeconds(recoveryDuration);

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
