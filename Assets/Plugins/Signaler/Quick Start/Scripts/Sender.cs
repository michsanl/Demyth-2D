using UnityEngine;
using echo17.Signaler.Core;
using System.Collections.Generic;

public class Sender : MonoBehaviour, IBroadcaster
{
	void Start ()
    {
        var signal = new MySignal()
        {
            message = "Hello World!"
        };

        Signaler.Instance.Broadcast<MySignal>(this, signal: signal);
	}
}
