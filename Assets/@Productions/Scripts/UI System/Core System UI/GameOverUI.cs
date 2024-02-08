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
        private GameStateService _gameStateService;
        private DeathDescriptionManager _deathDescriptionManager;
        private bool _isRestarting;
        private float updateSelectedTimerMax = .2f;
        private float updateSelectedTimer;
        private bool canUpdateSelected;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _inputController = SceneServiceProvider.GetService<GameInputController>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _deathDescriptionManager = SceneServiceProvider.GetService<DeathDescriptionManager>();

            _gameStateService[GameState.GameOver].onEnter += GameStateGameOver_OnEnter;
            _gameStateService[GameState.GameOver].onExit += GameStateGameOver_OnExit;

            _retryButton.onClick.AddListener(OnRetryButtonClick);
            _uiPage.OnOpen.AddListener(UIPage_OnOpen);
            _uiPage.OnClose.AddListener(() => { canUpdateSelected = false; });
        }

        private void Start()
        {
            _retryButton.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateSelectedButton();
        }

        private void OnRetryButtonClick()
        {
            if (_isRestarting) return;

            canUpdateSelected = false;
            DeactivateRetryButton();
            StartCoroutine(RestartLevel());
        }

        private IEnumerator RestartLevel()
        {
            _isRestarting = true;

            yield return StartCoroutine(PersistenceLoadingUI.Instance.OpenLoadingPage());

            _gameStateService.SetState(GameState.Gameplay);
            _inputController.EnablePlayerInput();

            yield return StartCoroutine(PersistenceLoadingUI.Instance.CloseLoadingPage());
            
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

        private void UpdateSelectedButton()
        {
            if (!canUpdateSelected) return;

            updateSelectedTimer += Time.deltaTime;

            if (updateSelectedTimer >= updateSelectedTimerMax)
            {
                _retryButton.Select();
                updateSelectedTimer = 0;
            }
        }

        private IEnumerator OpenPageCoroutine()
        {
            _animator.SetTrigger("OpenPage");
            _deathText.text = _deathDescriptionManager.SelectedDeathDescription;
            Helper.PlaySFX(_uiClipSO.GameOver, _uiClipSO.GameOverVolume);

            yield return Helper.GetWaitForSeconds(1f);
            _retryButton.gameObject.SetActive(true);
            canUpdateSelected = true;
        }

        private void DeactivateRetryButton()
        {
            _retryButton.gameObject.SetActive(false);
        }
    }
}
