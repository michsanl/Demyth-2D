using System.Collections;
using System.Collections.Generic;
using CustomExtensions;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class CreditUI : MonoBehaviour
    {
        
        [SerializeField] private Button _showSkipButton;
        [SerializeField] private Button _skipButton;
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
                
            });
        }

        private void Start()
        {
            _skipButton.gameObject.SetActive(false);
        }

        private IEnumerator HideSkipButton()
        {
            yield return Helper.GetWaitForSeconds(_skipButtonHideTimer);

            _skipButton.SetActive(false);
        }
    }
}

