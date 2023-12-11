namespace echo17.Signaler.Demos.Demo1
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Quick start to using the Signaler.
    /// Two receivers are created in memory (without gameobjects in this example)
    /// and then a message is broadcast
    /// </summary>
    public class Demo1 : MonoBehaviour, IBroadcaster
    {
        void Start()
        {
            new Receiver("Receiver 1");
            new Receiver("Receiver 2");

            Signaler.Instance.Broadcast<SaySomething>(this, signal: new SaySomething() { text = "Hello World!" });
        }
    }

}