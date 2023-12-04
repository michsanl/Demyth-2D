using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

public class BossLevelResetUI : MonoBehaviour
{
    
    [SerializeField] private PetraBossLevelReset _petraBossLevelReset;
    [SerializeField] private Button _retryButton;
    [SerializeField] private EnumId _gameViewId;
    [SerializeField] private UIPage _uiPage;    
    
    private IBossLevelReset _selectedBossLevelReset;

    private void Awake()
    {
        _petraBossLevelReset.OnPlayerDeathByPetra += PetraBossLevelReset_OnPlayerDeathByPetra;
        
        gameObject.SetActive(false);
    }

    private void PetraBossLevelReset_OnPlayerDeathByPetra(IBossLevelReset bossLevelReset)
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
