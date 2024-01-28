using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using Demyth.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class LoadingUI : SceneService
    {

        public Action OnOpenPageComplete;
        public Action OnClosePageComplete;

        [SerializeField] private float _openPageDuration;
        [SerializeField] private float _closePageDuration;
        [Space]
        [SerializeField] private UIClipSO _uiClipSO;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _model;

        private void Start()
        {
            _model.SetActive(false);
        }

        public void OpenPage()
        {
            _model.SetActive(true);
            StartCoroutine(OpenPageCoroutine());
        }

        public void ClosePage()
        {
            StartCoroutine(ClosePageCoroutine());
        }

        public float GetOpenPageDuration()
        {
            return _openPageDuration;
        }

        public float GetClosePageDuration()
        {
            return _closePageDuration;
        }

        private IEnumerator OpenPageCoroutine()
        {
            _animator.SetTrigger("OpenPage");
            Helper.PlaySFX(_uiClipSO.HUDOpen, _uiClipSO.HUDOpenVolume);

            yield return Helper.GetWaitForSeconds(_openPageDuration);

            OnOpenPageComplete?.Invoke();
        }

        private IEnumerator ClosePageCoroutine()
        {
            _animator.SetTrigger("ClosePage");
            Helper.PlaySFX(_uiClipSO.HUDClose, _uiClipSO.HUDCloseVolume);

            yield return Helper.GetWaitForSeconds(_closePageDuration);
            _model.SetActive(false);

            OnClosePageComplete?.Invoke();
        }

    }
}
