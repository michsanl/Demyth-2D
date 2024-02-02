using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using CustomExtensions;
using Demyth.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private UIClipSO _uiClipSO;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Animator _animator;
        [SerializeField] private TextMeshProUGUI _deathText;
        
        private UIPage _uiPage;    
        private GameInputController _inputController;
        private LoadingUI _loadingUI;
        private GameStateService _gameStateService;
        private DeathDescriptionManager _deathDescriptionManager;
        private bool _isRestarting;
        private string _deathDescription;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _loadingUI = SceneServiceProvider.GetService<LoadingUI>();
            _inputController = SceneServiceProvider.GetService<GameInputController>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _deathDescriptionManager = SceneServiceProvider.GetService<DeathDescriptionManager>();

            _gameStateService[GameState.GameOver].onEnter += GameStateGameOver_OnEnter;
            _gameStateService[GameState.GameOver].onExit += GameStateGameOver_OnExit;

            _retryButton.onClick.AddListener(OnRetryButtonClick);
            _uiPage.OnOpen.AddListener(UIPage_OnOpen);
        }

        private void Start()
        {
            _retryButton.gameObject.SetActive(false);
        }

        private void OnRetryButtonClick()
        {
            if (_isRestarting) return;

            DeactivateRetryButton();
            StartCoroutine(RestartLevel());
        }

        private IEnumerator RestartLevel()
        {
            _isRestarting = true;
            _loadingUI.OpenPage();

            yield return Helper.GetWaitForSeconds(_loadingUI.GetOpenPageDuration());

            _gameStateService.SetState(GameState.Gameplay);
            _inputController.EnablePlayerInput();
            _loadingUI.ClosePage();

            yield return Helper.GetWaitForSeconds(_loadingUI.GetOpenPageDuration());
            
            _inputController.EnablePauseInput();
            _isRestarting = false;
        }

        private void GameStateGameOver_OnEnter(GameState state)
        {
            _uiPage.OpenPage(_uiPage.PageID);
        }

        private void GameStateGameOver_OnExit(GameState state)
        {
            _uiPage.Return();
        }

        private void UIPage_OnOpen()
        {
            StartCoroutine(OpenPageCoroutine());
        }

        private IEnumerator OpenPageCoroutine()
        {
            _animator.SetTrigger("OpenPage");
            _deathText.text = _deathDescriptionManager.SelectedDeathDescription;
            Helper.PlaySFX(_uiClipSO.GameOver, _uiClipSO.GameOverVolume);

            yield return Helper.GetWaitForSeconds(1f);
            _retryButton.gameObject.SetActive(true);
        }

        private void DeactivateRetryButton()
        {
            _retryButton.gameObject.SetActive(false);
        }
    }
}
