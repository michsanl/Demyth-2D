using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using CustomExtensions;
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
        [SerializeField] private AnimationSequencerController _skipButtonShowAnimation;
        [Space]
        [SerializeField] private float _skipButtonHideTimer;

        private void Awake()
        {
            _showSkipButton.onClick.AddListener(() =>
            {
                _skipButton.gameObject.SetActive(true);
                StartCoroutine(HideSkipButton());
            });
            _skipButton.onClick.AddListener(() =>
            {
                StartCoroutine(LoadSceneWithDelay(1.1f));
            });
        }

        private void Start()
        {
            _skipButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _showSkipButton.gameObject.SetActive(true);
            _skipButton.gameObject.SetActive(false);
        }

        private IEnumerator HideSkipButton()
        {
            yield return Helper.GetWaitForSeconds(_skipButtonHideTimer);

            _skipButton.SetActive(false);
        }

        private IEnumerator LoadSceneWithDelay(float delay)
        {
            yield return Helper.GetWaitForSeconds(delay);
            SceneManager.LoadScene(0);
        }
    }
}

