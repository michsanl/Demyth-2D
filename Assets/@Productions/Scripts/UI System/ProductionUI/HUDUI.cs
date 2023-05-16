using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem
{
    public class HUDUI : UIPageView
    {
        private Animator animator;

        protected override void OnInitialize()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            animator.Play("HUD_Closed");
        }

        protected override void OnOpen()
        {
            Canvas.enabled = true;
            animator.Play("HUD_Open");
        }

        protected override void OnClosed()
        {
            animator.Play("HUD_Close");
        }
    }
}
