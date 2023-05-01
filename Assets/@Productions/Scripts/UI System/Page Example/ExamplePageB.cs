using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public class ExamplePageB : StandardPage
    {
        public void ChangePage()
        {
            Switch<ExamplePageA>();
        }
    }
}