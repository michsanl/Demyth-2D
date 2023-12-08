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
        
        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();

            _gameStateService[GameState.GameOver].onEnter += GameStateGameOver_OnEnter;
            _gameStateService[GameState.GameOver].onExit += GameStateGameOver_OnExit;

            _retryButton.onClick.AddListener(SetGamteStateToGameplay);

            _uiPage.OnOpen.AddListener(() => _animator.SetTrigger("OpenPage"));
        }

        private void GameStateGameOver_OnEnter(GameState state)
        {
            _uiPage.OpenPage(_uiPage.PageID);
        }

        private void GameStateGameOver_OnExit(GameState state)
        {
            _uiPage.Return();
        }

        private void SetGamteStateToGameplay()
        {
            _gameStateService.SetState(GameState.Gameplay);
        }
    }
}
