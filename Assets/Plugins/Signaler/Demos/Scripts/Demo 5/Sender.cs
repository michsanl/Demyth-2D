namespace echo17.Signaler.Demos.Demo5
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Sends a request to all receivers to get their positions
    /// </summary>
    public class Sender : MonoBehaviour, IBroadcaster
    {
        /// <summary>
        /// Cache the request so that we can reuse it without reallocating
        /// </summary>
        private GetPositionRequest _requestGetPosition;

        /// <summary>
        /// Cache the results so that we can reuse it without reallocating
        /// </summary>
        private UpdateResults _signalUpdateResults;

        void Start()
        {
            // set up the cached request 
            _requestGetPosition = new GetPositionRequest();

            // set up the cached signal to update the results
            _signalUpdateResults = new UpdateResults();
        }

        void Update()
        {
            // check for input and send the request with the appropriate filter

            if (Input.GetKeyDown(KeyCode.A))
            {
                SendRequest(Direction.AnyDirection);
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                SendRequest(Direction.Up);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                SendRequest(Direction.Down);
            }
        }

        /// <summary>
        /// Sends a request to all receivers to get their position
        /// </summary>
        /// <param name="direction">The direction to filter</param>
        private void SendRequest(Direction direction)
        {
            // set the direction filter of the request signal
            _requestGetPosition.directionFilter = direction;

            // broadcast the request, populating the list of responses
            List<SignalResponse<GetPositionOutput>> responses;
            var responderCount = Signaler.Instance.Broadcast<GetPositionRequest, GetPositionOutput>(this, out responses, _requestGetPosition);

            // format the resulting text
            _signalUpdateResults.text = "Going " + direction.ToString() + "\n\n";
            if (responderCount == 0)
            {
                _signalUpdateResults.text += "None";
            }
            else
            {
                foreach (var response in responses)
                {
                    _signalUpdateResults.text += ((MonoBehaviour)response.subscriber).name + ": " + response.response.outputPosition + "\n";
                }
            }

            // send out a signal to update the results
            Signaler.Instance.Broadcast<UpdateResults>(this, _signalUpdateResults);
        }
    }
}
