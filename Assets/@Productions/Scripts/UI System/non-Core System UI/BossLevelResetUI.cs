using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

public class BossLevelResetUI : MonoBehaviour
{
    
    [SerializeField] private BossLevelReset _petraBossLevelReset;
    [SerializeField] private BossLevelReset _sriBossLevelReset;
    [SerializeField] private Button _retryButton;
    [SerializeField] private EnumId _gameViewId;
    [SerializeField] private UIPage _uiPage;    
    
    private BossLevelReset _selectedBossLevelReset;

    private void Awake()
    {
        // This solve wierd null reference
        _petraBossLevelReset.OnPlayerDeathByBoss += PetraBossLevelReset_OnPlayerDeathByBoss;
        _sriBossLevelReset.OnPlayerDeathByBoss += PetraBossLevelReset_OnPlayerDeathByBoss;
        
        gameObject.SetActive(false);
    }

    private void PetraBossLevelReset_OnPlayerDeathByBoss(BossLevelReset bossLevelReset)
    {
        gameObject.SetActive(true);

        _selectedBossLevelReset = bossLevelReset;

        _retryButton.onClick.RemoveAllListeners();
        _retryButton.onClick.AddListener(ResetSelectedBossLevel);
    }

    private void ResetSelectedBossLevel()
    {
        _selectedBossLevelReset.ResetLevel();

        gameObject.SetActive(false);
    }


}
