namespace echo17.Signaler.Demos.Demo4
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Sends a trigger to all receivers.
    /// Receivers then send their own broadcast out.
    /// Note that all classes are decoupled. This sender does
    /// not need to know about the UI class or receivers, only
    /// the signals that can be sent.
    /// </summary>
    public class Sender : MonoBehaviour, IBroadcaster
    {
        void Update()
        {
            // S key pressed
            if (Input.GetKeyDown(KeyCode.S))
            {
                // Send a trigger signal to reset the UI (no data needed to send)
                Signaler.Instance.Broadcast<ResetUI>(this);

                // Send a trigger signal to all receivers to test the order (no data needed to send)
                Signaler.Instance.Broadcast<OrderTest>(this);
            }
        }
    }
}