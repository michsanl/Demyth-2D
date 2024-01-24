using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using CustomExtensions;
using Demyth.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private UIClipSO _uiClipSO;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Animator _animator;
        
        private UIPage _uiPage;    
        private GameStateService _gameStateService;
        private Level7RestartHandler _level7RestartHandler;
        private Action _onRestartLevel;
        
        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _level7RestartHandler = SceneServiceProvider.GetService<Level7RestartHandler>();

            _gameStateService[GameState.GameOver].onEnter += GameStateGameOver_OnEnter;
            _gameStateService[GameState.GameOver].onExit += GameStateGameOver_OnExit;

            _retryButton.onClick.AddListener(RetryButton_OnClick);
            _uiPage.OnOpen.AddListener(UIPage_OnOpen);

            _level7RestartHandler.OnPlayerDeathByBoss += OnPlayerDeathByBoss;
            _level7RestartHandler.OnPlayerDeathByDialogue += OnPlayerDeathByDialogue;
        }

        private void Start()
        {
            _retryButton.gameObject.SetActive(false);
        }

        private void RetryButton_OnClick()
        {
            ResetCondition();

            _gameStateService.SetState(GameState.Gameplay);

            _onRestartLevel?.Invoke();
            _onRestartLevel = null;
        }

        private void GameStateGameOver_OnEnter(GameState state)
        {
            _uiPage.OpenPage(_uiPage.PageID);
        }

        private void GameStateGameOver_OnExit(GameState state)
        {
            _uiPage.Return();
        }

        private void OnPlayerDeathByBoss(Action restartLevelCallback)
        {
            _onRestartLevel = restartLevelCallback;
        }

        private void OnPlayerDeathByDialogue(Action restartLevelCallback)
        {
            _onRestartLevel = restartLevelCallback;
        }

        private void UIPage_OnOpen()
        {
            StartCoroutine(OpenPageCoroutine());
        }

        private IEnumerator OpenPageCoroutine()
        {
            _animator.SetTrigger("OpenPage");
            Helper.PlaySFX(_uiClipSO.GameOver, _uiClipSO.GameOverVolume);

            yield return Helper.GetWaitForSeconds(1f);

            _retryButton.gameObject.SetActive(true);
        }

        private void ResetCondition()
        {
            StopAllCoroutines();
            _retryButton.gameObject.SetActive(false);
        }
    }
}
