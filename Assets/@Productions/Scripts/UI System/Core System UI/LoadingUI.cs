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

        [SerializeField] private float _openPageDuration;
        [SerializeField] private float _closePageDuration;
        [Space]
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _model;

        private GameHUD _gameHUD;

        private void Awake()
        {
            _gameHUD = SceneServiceProvider.GetService<GameHUD>();
        }

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
            _gameHUD.Close();

            yield return Helper.GetWaitForSeconds(_openPageDuration);
        }

        private IEnumerator ClosePageCoroutine()
        {
            _animator.SetTrigger("ClosePage");
            _gameHUD.Open();

            yield return Helper.GetWaitForSeconds(_closePageDuration);
            _model.SetActive(false);
        }

    }
}
