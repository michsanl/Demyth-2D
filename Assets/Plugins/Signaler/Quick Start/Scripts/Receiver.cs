using UnityEngine;
using echo17.Signaler.Core;

public class Receiver : MonoBehaviour, ISubscriber
{
	void Awake()
    {
        Signaler.Instance.Subscribe<MySignal>(this, OnMySignal);
	}

    private bool OnMySignal(MySignal signal)
    {
        Debug.Log(name + " received signal. Message = '" + signal.message + "'");

        return true;
    }
}
