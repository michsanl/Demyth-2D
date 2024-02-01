using System;
using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using Core;
using CustomExtensions;
using Demyth.Gameplay;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UISystem
{
    public class CreditUI : MonoBehaviour
    {
        
        [SerializeField] private Button _showSkipButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private GameObject _fadeOutPanel;
        [Space]
        [SerializeField] private float _enableShowSkipButtonDelay;
        [SerializeField] private float _disableSkipButtonDelay;
        [SerializeField] private float _loadSceneDelay;

        private GameStateService _gameStateService;

        private void Awake()
        {
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _gameStateService[GameState.GameEnd].onEnter += GameEnd_OnEnter;

            _showSkipButton.onClick.AddListener(() =>
            {
                StartCoroutine(DisableSkipButtonAfterDelay(_disableSkipButtonDelay));
                _skipButton.gameObject.SetActive(true);
            });
            _skipButton.onClick.AddListener(() =>
            {
                StartCoroutine(LoadSceneAfterDelay(_loadSceneDelay));
                _showSkipButton.gameObject.SetActive(false);
                _skipButton.enabled = false;
                _fadeOutPanel.SetActive(true);
            });
        }

        private void Start()
        {
            HidePage();
        }

        private void OnEnable()
        {
            _showSkipButton.gameObject.SetActive(false);
            _skipButton.gameObject.SetActive(false);
            _fadeOutPanel.SetActive(false);

            StartCoroutine(EnableShowSkipButtonAfterDelay(_enableShowSkipButtonDelay));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void GameEnd_OnEnter(GameState state)
        {
            ShowPage();
        }

        public void ShowPage()
        {
            gameObject.SetActive(true);
        }

        public void HidePage()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator EnableShowSkipButtonAfterDelay(float delay)
        {
            yield return Helper.GetWaitForSeconds(delay);
            _showSkipButton.SetActive(true);
        }

        private IEnumerator DisableSkipButtonAfterDelay(float delay)
        {
            yield return Helper.GetWaitForSeconds(delay);
            _skipButton.SetActive(false);
        }

        private IEnumerator LoadSceneAfterDelay(float delay)
        {
            yield return Helper.GetWaitForSeconds(delay);
            SceneManager.LoadScene(0);
        }
    }
}

