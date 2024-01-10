using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Core;
using Demyth.Gameplay;
using Lean.Pool;

public class GroundSummon : MonoBehaviour
{
    [Title("Object Timeline")]
    [SerializeField, OnValueChanged("CalculateTotalDuration")] private float anticipationDuration;
    [SerializeField, OnValueChanged("CalculateTotalDuration")] private float attackDuration;
    [SerializeField, OnValueChanged("CalculateTotalDuration")] private float recoveryDuration;
    [SerializeField] private int _animationModelCount = 3;
    [SerializeField] private bool isEndless;
    [SerializeField] private bool _disableDespawn;
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
    [SerializeField] private Animator _animator;

    public Action OnAnticipation;
    public Action OnAttack;
    public Action OnRecovery;

    private string[] _inAnimationArray = new string[6] { "Ground_Summon_1_In", "Ground_Summon_2_In", "Ground_Summon_3_In", "Ground_Summon_4_In", "Ground_Summon_5_In", "Ground_Summon_6_In"};
    private string[] _attackAnimationArray = new string[6] { "Ground_Summon_1_Attack", "Ground_Summon_2_Attack", "Ground_Summon_3_Attack", "Ground_Summon_4_Attack", "Ground_Summon_5_Attack", "Ground_Summon_6_Attack"};
    private string[] _outAnimationArray = new string[6] { "Ground_Summon_1_Out", "Ground_Summon_2_Out", "Ground_Summon_3_Out", "Ground_Summon_4_Out", "Ground_Summon_5_Out", "Ground_Summon_6_Out"};

    private int topBorder = 2;
    private int bottomBorder = -4;
    private int rightBorder = 6;
    private int leftBorder = -6;

    private void OnEnable() 
    {
        if (IsOutOfBounds())
        {
            if (_disableDespawn) return;

            LeanPool.Despawn(gameObject);
            return;
        }

        StartCoroutine(SummonRoutine());
    }

    private void OnDisable()
    {
        colliderGameObject.SetActive(false);
    }

    private IEnumerator SummonRoutine()
    {
        if (useRandomizedSpawnDelay)
            yield return StartCoroutine(RandomizedSpawnDelay());

        var randomIndex = UnityEngine.Random.Range(0, _animationModelCount);

        _animator.Play(_inAnimationArray[randomIndex]);
        yield return Helper.GetWaitForSeconds(anticipationDuration);

        colliderGameObject.SetActive(true);
        _animator.Play(_attackAnimationArray[randomIndex]);
        yield return Helper.GetWaitForSeconds(attackDuration);

        if (isEndless) yield break;

        colliderGameObject.SetActive(false);
        _animator.Play(_outAnimationArray[randomIndex]);
        yield return Helper.GetWaitForSeconds(recoveryDuration);
        
        if (_disableDespawn) yield break;

        LeanPool.Despawn(gameObject);
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
