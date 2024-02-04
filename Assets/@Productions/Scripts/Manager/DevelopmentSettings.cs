using System.Collections;
using System.Collections.Generic;
using Core;
using Demyth.Gameplay;
using UnityEngine;

public class DevelopmentSettings : MonoBehaviour
{

    [SerializeField] private GameSettingsSO _gameSettingsSO;
    [Space]
    [SerializeField] private GameObject[] _levelGateArray;

    private Player _player;

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;

        SetupPlayerItems();
        SetupLevelGate();
    }

    private void SetupPlayerItems()
    {
        _player.UsePan = _gameSettingsSO.UsePanOnStart;
        _player.IsLanternUnlocked = _gameSettingsSO.UnlockLanternOnStart;
        _player.IsHealthPotionUnlocked = _gameSettingsSO.UnlockPotionOnStart;
        _player.IsShieldUnlocked = _gameSettingsSO.UnlockShieldOnStart;
    }

    private void SetupLevelGate()
    {
        foreach (var levelGate in _levelGateArray)
        {
            levelGate.SetActive(_gameSettingsSO.UnlockAllGateOnStart);
        }
    }
}
