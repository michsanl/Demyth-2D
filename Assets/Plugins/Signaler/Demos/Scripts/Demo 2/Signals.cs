namespace echo17.Signaler.Demos.Demo2
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Message signal to pass text and color information.
    /// We set the serializable attribute so that we can use this
    /// struct in a MonoBehaviour, specifically the Sender of this demo.
    /// </summary>
    [Serializable]
    public struct CubeUpdate
    {
        public string text;
        public Color color;
    }
}