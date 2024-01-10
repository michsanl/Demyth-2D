namespace echo17.Signaler.Core
{
    using System.Collections;

    /// <summary>
    /// Subscription to a broadcast. A receiver requests this and the Signaler will send out the
    /// broadcast to whomever has one. This is an abstract class used as a base for the Message and Request
    /// Subscriptions.
    /// </summary>
    /// <typeparam name="S">Signal type</typeparam>
    public abstract class Subscription<S, R> : ISubscription
    {
        #region Private and Hidden

        /// <summary>
        /// The subscriber of the subscription.
        /// This is used only by the Signaler
        /// and should not be tampered with.
        /// </summary>
        public ISubscriber _Subscriber { get; set; }

        /// <summary>
        /// This list that this subscription is part of.
        /// This is used to determine if we can unsubscribe,
        /// or if the current list is being broadcast.
        /// This should only be used by the Signaler and
        /// should not be tampered with.
        /// </summary>
        public IList _SubscriptionList { get; set; }

        /// <summary>
        /// Create a new subscription. This should only be called by the Signaler.
        /// You should instead call Subscribe on the Signaler.
        /// </summary>
        /// <param name="subscriber">The subscriber of this subscription</param>
        /// <param name="action">The action to call when the signal is broadcast</param>
        /// <param name="group">The group of the subscription</param>
        /// <param name="order">The order the subscription is processed in</param>
        public Subscription(ISubscriber subscriber, long? group, long order)
        {
            _Subscriber = subscriber;
            _group = group;
            _groupSet = true;
            _order = order;
            _orderSet = true;
        }

        #endregion

        #region Public

        public SignalKey _SignalKey { get; set; }

        /// <summary>
        /// Group of the subscription
        /// </summary>
        private bool _groupSet = false;
        private long? _group = null;
        public long? Group
        {
            get
            {
                return _group;
            }
            set
            {
                if (!_groupSet || value != _group)
                {
                    _groupSet = true;
                    _group = value;
                    Signaler.Instance._QueueUpdateGroup<S, R>(this, _group);
                }
            }
        }

        /// <summary>
        /// Order of the subscription
        /// </summary>
        private bool _orderSet = false;
        private long _order;
        public long Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (!_orderSet || value != _order)
                {
                    _orderSet = true;
                    _order = value;
                    Signaler.Instance._QueueUpdateOrder<S, R>(this, _order);
                }
            }
        }

        /// <summary>
        /// Remove the subscription from the Signaler
        /// </summary>
        public void UnSubscribe()
        {
            Signaler.Instance._QueueUnSubscribe(this);
        }

        #endregion
    }
}
