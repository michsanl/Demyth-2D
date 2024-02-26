using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SriAbilityController : MonoBehaviour
{
    [SerializeField] private GameObject[] _attackColliderArray;

    private Ability _upSlash;
    private Ability _downSlash;
    private Ability _horizontalSlash;
    private Ability _spinClaw;
    private Ability _nailAOE;
    private Ability _nailSummon;
    private Ability _fireBall;
    private Ability _teleport;
    private Ability _teleportMiddleArena;
    private Ability _horizontalNailWave;
    private Ability _verticalNailWave;
    private Ability _waveOutNailWave;
    private Ability _deathSlash;
    private Coroutine _currentAbilityCoroutine;

    private void Awake()
    {
        _upSlash = GetComponent<SriAbilityUpSlash>();
        _downSlash = GetComponent<SriAbilityDownSlash>();
        _horizontalSlash = GetComponent<SriAbilityHorizontalSlash>();
        _spinClaw = GetComponent<SriAbilitySpinClaw>();
        _nailAOE = GetComponent<SriAbilityNailAOE>();
        _nailSummon = GetComponent<SriAbilityNailSummon>();
        _fireBall = GetComponent<SriAbilityFireBall>();
        _teleport = GetComponent<SriAbilityTeleport>();
        _teleportMiddleArena = GetComponent<SriAbilityTeleportToMiddleArena>();
        _horizontalNailWave = GetComponent<SriAbilityHorizontalNailWave>();
        _verticalNailWave = GetComponent<SriAbilityVerticalNailWave>();
        _waveOutNailWave = GetComponent<SriAbilityWaveOutNailWave>();
        _deathSlash = GetComponent<SriAbilityDeathSlash>();
    }

    public void StopCurrentAbility()
    {
        if (_currentAbilityCoroutine != null) StopCoroutine(_currentAbilityCoroutine);
        transform.DOKill();
        DeactivateAllAttackCollider();
    }

    public IEnumerator StartUpSlashAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_upSlash.PlayAbility());
    }

    public IEnumerator StartDownSlashAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_downSlash.PlayAbility());
    }

    public IEnumerator StartHorizontalSlashAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_horizontalSlash.PlayAbility());
    }

    public IEnumerator StartSpinClawAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_spinClaw.PlayAbility());
    }

    public IEnumerator StartNailAOEAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_nailAOE.PlayAbility());
    }

    public IEnumerator StartNailSummonAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_nailSummon.PlayAbility());
    }

    public IEnumerator StartFireBallAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_fireBall.PlayAbility());
    }

    public IEnumerator StartHorizontalNailWaveAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_horizontalNailWave.PlayAbility());
    }

    public IEnumerator StartVerticalNailWaveAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_verticalNailWave.PlayAbility());
    }

    public IEnumerator StartWaveOutNailWaveAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_waveOutNailWave.PlayAbility());
    }

    public IEnumerator StartTeleportAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_teleport.PlayAbility());
    }

    public IEnumerator StartTeleportToMiddleArena()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_teleportMiddleArena.PlayAbility());
    }

    public IEnumerator StartDeathSlashAbility()
    {
        yield return _currentAbilityCoroutine = StartCoroutine(_deathSlash.PlayAbility());
    }

    private void DeactivateAllAttackCollider()
    {
        foreach (var collider in _attackColliderArray)
        {
            collider.SetActive(false);
        }
    }
}
