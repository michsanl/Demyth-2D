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
        public void PlayAnimation(Action onFinished);
        public void CloseAnimation(Action onFinished);
    }

    public class UIPageAnimator : MonoBehaviour, IPageAnimator
    {
        [Header("Animation Sequencer")]
        [SerializeField]
        private AnimationSequencerController openPageAnimation;
        [SerializeField]
        private AnimationSequencerController closePageAnimation;

        public void PlayAnimation(Action onFinished)
        {
            if (openPageAnimation != null)
            {
                openPageAnimation.ResetToInitialState();
                openPageAnimation.Play(onFinished);
            }
            else
            {

            }
        }

        public void CloseAnimation(Action onFinished)
        {
            if (closePageAnimation != null)
            {
                closePageAnimation.ResetToInitialState();
                closePageAnimation.Play(onFinished);
            }
            else
            {

            }
        }
    }
}