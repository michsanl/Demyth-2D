using BrunoMikoski.AnimationSequencer;
using Lean.Gui;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.UI
{
    public class UIPage : MonoBehaviour
    {
        public enum ClosePageType
        {
            CanvasGraphic = 0,
            GameObject = 1,
        }

        public EnumId PageID => pageId;
        public PageData PageData => _pageData;
        public SceneUI SceneUI => _sceneUI;
        public bool DisablePreviousPage => disablePreviousPage;
        public bool IsCloseAnimationIsPlaying => closePageAnimation != null ? closePageAnimation.IsPlaying : true;

        [Header("UI ID")]
        [SerializeField] private EnumId pageId;

        [Header("Page Setting")]
        [SerializeField] private bool disablePreviousPage = false;
        [SerializeField] private ClosePageType offType;

        [Header("Page Animation")]
        [SerializeField]
        private AnimationSequencerController openPageAnimation;
        [SerializeField]
        private AnimationSequencerController closePageAnimation;

        [Header("Events Hook")]
        public UnityEvent<PageData> OnPushed;
        public UnityEvent OnOpen;
        public UnityEvent OnRefresh;
        public UnityEvent OnClose;

        private SceneUI _sceneUI;
        private PageData _pageData;
        private Canvas _canvas;
        private GraphicRaycaster _graphicRaycaster;
        private CanvasGroup _canvasGroup;

        private IPageAnimator _pageAnimator;

        internal void SetupPage(SceneUI sceneUI)
        {
            _sceneUI = sceneUI;

            _canvas = GetComponent<Canvas>();
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _pageAnimator = GetComponent<IPageAnimator>();
        }

        internal void OnPush(PageData data)
        {
            _pageData = data;
            OnPushed?.Invoke(_pageData);
        }

        internal void Open()
        {
            SetPageVisibility(true);
            
            if (_pageAnimator != null)
            {
                _pageAnimator.PlayAnimation(() =>
                {
                    SetRaycast(true);
                });
            }

            OnOpen?.Invoke();
        }

        internal void Refresh()
        {
            OnRefresh?.Invoke();
        }

        internal void Close()
        {
            if (_pageAnimator != null)
            {
                SetRaycast(false);
                _pageAnimator.CloseAnimation(() =>
                {
                    closePageAnimation.ResetToInitialState();
                    SetPageVisibility(false);
                });
            }
            else
            {
                SetPageVisibility(false);
            }

            OnClose?.Invoke();
        }

        internal void InstantClose()
        {
            SetPageVisibility(false);
        }

        public void OpenPage(EnumId pageId)
        {
            SceneUI.PushPage(pageId);
        }

        public void OpenPage(EnumId pageId, PageData data = null)
        {
            SceneUI.PushPage(pageId, data);
        }

        public void ReturnToPage(EnumId pageId)
        {
            SceneUI.PopToPage(pageId);
        }

        public void ReturnToFirstPage()
        {
            SceneUI.PopToFirstPage();
        }

        public void Return()
        {
            SceneUI.PopPage();
        }

        private void SetPageVisibility(bool condition)
        {
            if (offType == ClosePageType.CanvasGraphic)
            {
                if (_canvas != null)
                {
                    _canvas.enabled = condition;
                }
                else
                {
                    gameObject.SetActive(condition);
                }
            }
            else if (offType == ClosePageType.GameObject)
            {
                gameObject.SetActive(condition);
            }

            SetRaycast(condition);
        }

        private void SetRaycast(bool condition)
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.interactable = condition;
            }

            if (_graphicRaycaster != null)
            {
                _graphicRaycaster.enabled = condition;
            }
        }
    }
}