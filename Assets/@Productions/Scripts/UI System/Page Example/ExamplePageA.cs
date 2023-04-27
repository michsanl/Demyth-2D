using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public class ExamplePageA : StandardPage
    {
        public void ChangePage()
        {
            Debug.Log("Called");
            Switch<ExamplePageB>();
        }
    }
}