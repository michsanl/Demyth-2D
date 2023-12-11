namespace echo17.Signaler.Demos.Demo4
{
    using System;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Receivers with an order of priority. 
    /// Order is processed from low numbers to high numbers.
    /// Example: -1000, -22, 0, 5, 34, 23021
    /// </summary>
    [Serializable]
    public class Receiver : MonoBehaviour, ISubscriber, IBroadcaster
    {
        /// <summary>
        /// The order to receive the broadcast
        /// </summary>
        public long order;

        /// <summary>
        /// Subscription to the broadcast.
        /// This is cached to allow changes in the inspector
        /// </summary>
        private MessageSubscription<OrderTest> _subscription;

        /// <summary>
        /// An acknowledgement of the broadcast. 
        /// This shows that even receivers can be broadcasters
        /// </summary>
        private Acknowledgement _message;

        void Start()
        {
            // set up a subscription to the signal with an order
            _subscription = Signaler.Instance.Subscribe<OrderTest>(this, OnOrderTest, order: order);

            // cache the message so that we don't have to reallocate each time we need to broadcast
            _message = new Acknowledgement();
        }

        /// <summary>
        /// Occurs when the inspector values are changed
        /// </summary>
        void OnValidate()
        {
            if (_subscription != null)
            {
                // set the order of the subscription
                _subscription.Order = order;
            }
        }

        /// <summary>
        /// Handles the broadcast signal
        /// </summary>
        /// <param name="signal">Signal contains no information, it is just a trigger</param>
        /// <returns></returns>
        private bool OnOrderTest(OrderTest signal)
        {
            // set up the outgoing message signal
            _message.text = name + " received signal. Order: " + order.ToString();

            // Broadcast that the original message was received and in what order
            Signaler.Instance.Broadcast<Acknowledgement>(this, signal: _message);

            // message received
            return true;
        }
    }
}