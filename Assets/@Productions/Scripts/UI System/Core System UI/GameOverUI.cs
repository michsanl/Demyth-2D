using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using Demyth.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Animator _animator;
        
        private UIPage _uiPage;    
        private GameStateService _gameStateService;
        private Level7RestartHandler _level7RestartHandler;
        private Action _restartLevelCallback;
        
        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _level7RestartHandler = SceneServiceProvider.GetService<Level7RestartHandler>();

            _gameStateService[GameState.GameOver].onEnter += GameStateGameOver_OnEnter;
            _gameStateService[GameState.GameOver].onExit += GameStateGameOver_OnExit;

            _retryButton.onClick.AddListener(RestartLevel);

            _uiPage.OnOpen.AddListener(() => _animator.SetTrigger("OpenPage"));
        }

        private void Start()
        {
            _level7RestartHandler.OnPlayerDeathByBoss += OnPlayerDeathByBoss;
            _level7RestartHandler.OnPlayerDeathByDialogue += OnPlayerDeathByDialogue;
        }

        private void OnPlayerDeathByBoss(Action restartLevelCallback)
        {
            _restartLevelCallback = restartLevelCallback;
        }

        private void OnPlayerDeathByDialogue(Action restartLevelCallback)
        {
            _restartLevelCallback = restartLevelCallback;
        }

        private void GameStateGameOver_OnEnter(GameState state)
        {
            _uiPage.OpenPage(_uiPage.PageID);
        }

        private void GameStateGameOver_OnExit(GameState state)
        {
            _uiPage.Return();
        }

        private void RestartLevel()
        {
            _gameStateService.SetState(GameState.Gameplay);
            
            _restartLevelCallback?.Invoke();
            _restartLevelCallback = null;
        }
    }
}
