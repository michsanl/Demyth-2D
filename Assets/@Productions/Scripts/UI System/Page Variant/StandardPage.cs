using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public abstract class StandardPage : UIPageView
    {
        protected override void OnOpen()
        {
            Canvas.enabled = true;
        }

        protected override void OnClosed()
        {
            Canvas.enabled = false;           
        }
    }
}