using BrunoMikoski.AnimationSequencer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.UI
{
    public interface IPageAnimator
    {
        public bool IsPlaying { get; }
        public void PlayAnimation(Action onFinished);
        public void CloseAnimation(Action onFinished);
    }

    public class UIPageAnimator : MonoBehaviour, IPageAnimator
    {
        public bool IsPlaying => _isPlaying;

        [Header("Animation Sequencer")]
        [SerializeField]
        private AnimationSequencerController openPageAnimation;
        [SerializeField]
        private AnimationSequencerController closePageAnimation;

        private bool _isPlaying;

        public void PlayAnimation(Action onFinished)
        {
            if (openPageAnimation != null)
            {
                _isPlaying = true;
                openPageAnimation.ResetToInitialState();
                openPageAnimation.Play(() => {
                    _isPlaying = false;
                    onFinished?.Invoke();
                });
            }
            else
            {

            }
        }

        public void CloseAnimation(Action onFinished)
        {
            if (closePageAnimation != null)
            {
                _isPlaying = true;
                closePageAnimation.ResetToInitialState();
                closePageAnimation.Play(() => {
                    _isPlaying = false;
                    onFinished?.Invoke();
                });
            }
            else
            {

            }
        }
    }
}