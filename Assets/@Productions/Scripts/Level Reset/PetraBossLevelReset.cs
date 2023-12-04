using System;
using Core;
using Demyth.Gameplay;
using UnityEngine;

public class PetraBossLevelReset : MonoBehaviour, IBossLevelReset
{

    public Action<IBossLevelReset> OnPlayerDeathByPetra;

    [SerializeField] private GameObject _npcBossPetra;
    [SerializeField] private GameObject _npcPetra;
    [SerializeField] private GameObject _invisibleDialogueTrigger;
    [Space]
    [SerializeField] private Vector3 _playerDefaultPosition;
    [SerializeField] private Vector3 _npcBossPetraDefaultPosition;
    private Player _player;
    private Health _playerHealth;
    private Health _petraHealth;
    
    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
        _playerHealth = _player.GetComponent<Health>();
        _petraHealth = _npcBossPetra.GetComponent<Health>();
    }

    private void OnEnable()
    {
        _playerHealth.OnDeath += PlayerHealth_OnDeath;
    }

    private void OnDisable() 
    {
        _playerHealth.OnDeath += PlayerHealth_OnDeath;
    }

    public void ResetLevel()
    {
        // To do :
        // Reset health, shield, etc on Player & Boss
        // Set back position on Player & Boss
        // Activate and deactivate objects

        _player.gameObject.SetActive(true);
        _player.ResetPlayerCondition();

        _petraHealth.ResetHealthToMaximum();

        ResetUnitPosition();
        ResetActiveState();
    }

    private void PlayerHealth_OnDeath()
    {
        OnPlayerDeathByPetra?.Invoke(this);
    }

    private void ResetUnitPosition()
    {
        _player.transform.position = _playerDefaultPosition;
        _npcBossPetra.transform.position = _npcBossPetraDefaultPosition;
    }

    private void ResetActiveState()
    {
        _npcPetra.SetActive(true);
        _invisibleDialogueTrigger.SetActive(true);
        
        _npcBossPetra.SetActive(false);
    }
}
