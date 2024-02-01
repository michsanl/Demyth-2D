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
        [Space]
        [SerializeField] private float _hideSkipButtonDelay;
        [SerializeField] private float _loadSceneDelay;

        private GameStateService _gameStateService;

        private void Awake()
        {
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _gameStateService[GameState.GameEnd].onEnter += GameEnd_OnEnter;

            _showSkipButton.onClick.AddListener(() =>
            {
                StartCoroutine(HideSkipButtonAfterDelay(_hideSkipButtonDelay));
                _skipButton.gameObject.SetActive(true);
            });
            _skipButton.onClick.AddListener(() =>
            {
                StartCoroutine(LoadSceneAfterDelay(_loadSceneDelay));
                _showSkipButton.gameObject.SetActive(false);
                _skipButton.gameObject.SetActive(false);
            });
        }

        private void OnEnable()
        {
            _showSkipButton.gameObject.SetActive(true);
            _skipButton.gameObject.SetActive(false);
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

        private IEnumerator HideSkipButtonAfterDelay(float delay)
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

