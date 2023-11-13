namespace echo17.Signaler.Demos.Demo5
{
    using System;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Spinning cube that responds to a broadcast, letting the sender 
    /// know its position
    /// </summary>
    public class Receiver : SpinningObject, ISubscriber
    {
        void Awake()
        {
            SpinInitialize();
        }

        void Start()
        {
            // subscribe to the GetPositionRequest
            Signaler.Instance.Subscribe<GetPositionRequest, GetPositionOutput>(this, OnGetPosition);
        }

        void Update()
        {
            SpinUpdate();
        }

        /// <summary>
        /// Handles the request to get the position of the receiver
        /// </summary>
        /// <param name="signal">Contains the direction filter</param>
        /// <param name="response">Response back to the sender</param>
        /// <returns></returns>
        private bool OnGetPosition(GetPositionRequest signal, out GetPositionOutput response)
        {
            // if the direction is the same as the signal's direction filter
            if (
                signal.directionFilter == Direction.AnyDirection
                ||
                (signal.directionFilter == Direction.Up && Mathf.Sign(_direction.y) == 1)
                ||
                (signal.directionFilter == Direction.Down && Mathf.Sign(_direction.y) == -1)
                )
            {
                // formulate the response,
                // passing the position back to the sender
                response = new GetPositionOutput() { outputPosition = _transform.position };

                // signal handled
                return true;
            }
            else
            {
                // this receiver was not going the right direction
                response = default(GetPositionOutput);

                // signal not handled
                return false;
            }
        }
    }
}
