namespace echo17.Signaler.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The log level will show certain messages in the Unity Console.
    /// You can do a bit wise or to have multiple log levels.
    /// Example: LogLevel = SignalLogLevelEnum.NoSubscriptions | SignalLogLevelEnum.Info;
    /// </summary>
    [Flags]
    public enum SignalLogLevelEnum
    {
        None = 0,
        NoSubscriptions = 1,
        Info = 2
    }

    /// <summary>
    /// This singleton class handles all signals. It acts as a middle-man between 
    /// classes so that you can avoid coupling and promote reusability.
    /// Signals are subscribed to by the receivers and broadcast by the senders. 
    /// Grouping allows you to send signals to a select group based on a long value.
    /// Ordering allows you to prioritize signals to certain receivers, keeping your game flow consistent.
    /// 
    /// Terminology:
    /// 
    /// Broadcast: A signal sent out to all subscribers listening to it.
    /// Subscription: An appeal to be included in a broadcast.
    /// Subscriber: The class that is listening to signals sent from a broadcast.
    /// Receiver: See subscriber.
    /// Broadcaster: The class that sends out a broadcast signal. Sometimes the broadcaster will handle return data (requests).
    /// Sender: See broadcaster.
    /// Request: A type of signal that has a broadcast signal type and a response type to send back to the sender.
    /// Message: A type of signal that only has a broadcast signal type.
    /// </summary>
    public class Signaler
    {
        #region Singleton
        /// <summary>
        /// This is the reference to the singleton value that all classes can use globally
        /// </summary>
        private static Signaler _instance;
        public static Signaler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Signaler();
                }
                return _instance;
            }
        }
        #endregion

        #region Private Members

        /// <summary>
        /// Subscriptions have no long value associated with them. Signals of a certain type will go to all receivers for that type.
        /// </summary>
        private Dictionary<SignalKey, IList> _subscriptions = new Dictionary<SignalKey, IList>();

        /// <summary>
        /// A comparer to determine subscription sorting
        /// </summary>
        private OrderComparer _orderComparer = new OrderComparer();

        /// <summary>
        /// A stack of the current keys that are broadcasting. This is used to 
        /// determine if we need to queue updates to subscriptions if they are
        /// currently being broadcast
        /// </summary>
        private LiteList<SignalKey> _broadcastingKeys = new LiteList<SignalKey>();

        /// <summary>
        /// There is at least one queued update this broadcast
        /// </summary>
        private bool _hasQueuedUpdate = false;

        /// <summary>
        /// A list of group updates to process after the broadcast
        /// </summary>
        private LiteList<ISubscription> _queuedGroupUpdates = new LiteList<ISubscription>();

        /// <summary>
        /// A list of order updates to process after the broadcast
        /// </summary>
        private LiteList<ISubscription> _queuedOrderUpdates = new LiteList<ISubscription>();

        /// <summary>
        /// A list of unsubscribes to process after the broadcast
        /// </summary>
        private LiteList<ISubscription> _queuedUnSubscribes = new LiteList<ISubscription>();

        #endregion

        #region Public Members

        /// <summary>
        /// Whether the manager is actively sending out broadcasts
        /// </summary>
        private bool _active = true;
        public bool Active { get { return _active; } set { _active = value; } }

        #endregion

        #region Parameters

        /// <summary>
        /// The log level determines what information is sent to the Unity Console. See notes on the SignalLogLevelEnum above.
        /// </summary>
        private SignalLogLevelEnum _logLevel = 0;
        private bool _logInfo;
        private bool _logNoSubscriptions;
        public SignalLogLevelEnum LogLevel
        {
            get
            {
                return _logLevel;
            }
            set
            {
                _logLevel = value;
                _logInfo = ((_logLevel & SignalLogLevelEnum.Info) == SignalLogLevelEnum.Info);
                _logNoSubscriptions = ((_logLevel & SignalLogLevelEnum.NoSubscriptions) == SignalLogLevelEnum.NoSubscriptions);
            }
        }

        #endregion

        #region Private and Hidden Methods

        // TODO: lock down how to move subscriptions (group, order) and delete them so 
        // that nested broadcasts do not try to call subscriptions that are already marked disposed.

        /// <summary>
        /// Removes a subscription from the signal manager. This should not be called directly,
        /// instead it is called from a subscription's dispose method.
        /// </summary>
        /// <typeparam name="S">Signal type</typeparam>
        /// <typeparam name="R">Response type</typeparam>
        /// <param name="subscription">The subscription to remove</param>
        public bool _QueueUnSubscribe(ISubscription subscription)
        {
            // Check to see if this subscription is currently being broadcast.
            // If so, we need to queue the unsubscribe for after the broadcast
            if (_broadcastingKeys.Contains(subscription._SignalKey))
            {
                _hasQueuedUpdate = true;
                _queuedUnSubscribes.Add(subscription);
                return false;
            }

            // Remove the subscription from its list
            if (subscription != null && subscription._SubscriptionList != null)
            {
                subscription._SubscriptionList.Remove(subscription);
                subscription._SubscriptionList = null;
            }

            return true;
        }

        /// <summary>
        /// Updates the group for a subscription. This should not be called directly,
        /// instead you should set the subscription's Group parameter, which will then
        /// call this method.
        /// </summary>
        /// <typeparam name="S">Signal type</typeparam>
        /// <typeparam name="R">Response type</typeparam>
        /// <param name="subscription">The subscription to alter</param>
        /// <param name="newGroup">The new group of the subscription</param>
        public bool _QueueUpdateGroup<S, R>(ISubscription subscription, long? newGroup)
        {
            // Check to see if this subscription is currently being broadcast.
            // If so, we need to queue the group update for after the broadcast
            if (_broadcastingKeys.Contains(subscription._SignalKey))
            {
                _hasQueuedUpdate = true;
                _queuedGroupUpdates.Add(subscription);
                return false;
            }

            // Remove the subscription from its list
            _QueueUnSubscribe(subscription);

            // Add the subscription to its new list
            _AddSubscription<S, R>(subscription, newGroup);

            return true;
        }

        /// <summary>
        /// Updates the order of the subscription. This should not be called directly,
        /// instead you should set the subscription's Order parameter, which will then
        /// call this method.
        /// </summary>
        /// <typeparam name="S">Signal type</typeparam>
        /// <typeparam name="R">Response type</typeparam>
        /// <param name="subscription">The subscription to alter</param>
        /// <param name="newOrder">The new order to assign</param>
        public bool _QueueUpdateOrder<S, R>(ISubscription subscription, long newOrder)
        {
            // Check to see if this subscription is currently being broadcast.
            // If so, we need to queue the order update for after the broadcast
            if (_broadcastingKeys.Contains(subscription._SignalKey))
            {
                _hasQueuedUpdate = true;
                _queuedOrderUpdates.Add(subscription);
                return false;
            }

            // Remove the subscription from its list
            _QueueUnSubscribe(subscription);

            // Add the subscription back with the proper order
            _AddSubscription<S, R>(subscription, subscription.Group);

            return true;
        }

        /// <summary>
        /// Add the subscription to the list
        /// </summary>
        /// <typeparam name="S">Signal type</typeparam>
        /// <typeparam name="R">Response type</typeparam>
        /// <param name="subscription">Subscription to add</param>
        /// <param name="group">The group to use</param>
        private void _AddSubscription<S, R>(ISubscription subscription, long? group)
        {
            var responseType = typeof(R);
            var signalKey = new SignalKey(typeof(S), responseType, group);
            IList subscriptionList;

            // Set the subscriptions signal key
            subscription._SignalKey = signalKey;

            // If the signal type is not found, then create a new signal type in the dictionary
            if (!_subscriptions.TryGetValue(signalKey, out subscriptionList))
            {
                // Set up the appropriate subscription list based on whether it is a message or request
                if (responseType == typeof(NoResponse))
                {
                    subscriptionList = new LiteList<MessageSubscription<S>>();
                }
                else
                {
                    subscriptionList = new LiteList<RequestSubscription<S, R>>();
                }
                subscriptionList.Add(subscription);

                _subscriptions.Add(signalKey, subscriptionList);
            }
            else
            {
                // The signal type / group was found, so we insert the subscription
                _InsertSubscription(subscriptionList, subscription);
            }

            // Set the subscription's list
            subscription._SubscriptionList = subscriptionList;
        }

        /// <summary>
        /// Inserts the subscription into a list, putting it in the correct order
        /// </summary>
        /// <param name="subscriptionList">The subscription list to insert into</param>
        /// <param name="subscription">The subscription to insert</param>
        private void _InsertSubscription(IList subscriptionList, ISubscription subscription)
        {
            int lower = 0;
            int upper = subscriptionList.Count - 1;
            var index = 0;
            var found = false;

            // Do a binary search until the appropriate order is located to insert into
            while (lower <= upper)
            {
                int middle = lower + (upper - lower) / 2;
                int comparisonResult = _orderComparer.Compare(subscription, (ISubscription)subscriptionList[middle]);
                if (comparisonResult == 0)
                {
                    index = middle;
                    found = true;
                    break;
                }
                else if (comparisonResult < 0)
                {
                    upper = middle - 1;
                }
                else
                {
                    lower = middle + 1;
                }
            }

            if (!found)
            {
                index = ~lower;
            }

            if (index < 0) index = ~index;
            subscriptionList.Insert(index, subscription);
        }

        /// <summary>
        /// Comparison for subscription order
        /// </summary>
        private class OrderComparer : IComparer<ISubscription>
        {
            public int Compare(ISubscription a, ISubscription b)
            {
                if (a.Order == b.Order) { return 0; }
                if (a.Order < b.Order) { return -1; }
                return 1;
            }
        }

        /// <summary>
        /// Sets up a broadcast signal key and subscription list
        /// </summary>
        /// <typeparam name="S">Signal Type</typeparam>
        /// <typeparam name="R">Response Type</typeparam>
        /// <param name="group">Group</param>
        /// <param name="signalKey">The returned signal key</param>
        /// <param name="subscriptionList">The return subscription list</param>
        /// <returns>True if successful</returns>
        private bool _PreBroadcast<S, R>(long? group, out SignalKey signalKey, out IList subscriptionList)
        {
            signalKey = new SignalKey(typeof(S), typeof(R), group);
            subscriptionList = null;

            // If the manager is not active ignore the broadcast
            if (!_active)
            {
                return false;
            }

            // Make sure that this broadcast is not already occuring, such as a
            // nested call. If it is we need to abort or the stack will overflow in
            // recursion.
            if (_broadcastingKeys.Contains(signalKey))
            {
                Debug.LogError("Circular broadcast will result in an overflow. Aborting.");
                return false;
            }

            if (_logInfo)
            {
                Debug.Log(string.Format("Signaler::Broadcast signal {0}", signalKey));
            }

            // Look for the list of subscriptions based on the signal type
            if (_subscriptions.TryGetValue(signalKey, out subscriptionList))
            {
                _broadcastingKeys.Add(signalKey);

                return true;
            }
            else
            {
                // No subscriptions for this signal
                if (_logNoSubscriptions)
                {
                    Debug.LogWarning(string.Format("Signaler::No subscriptions for {0}!", signalKey));
                }

                return false;
            }
        }

        /// <summary>
        /// Takes the broadcast off the current stack and processes any queued updates
        /// </summary>
        /// <param name="signalKey"></param>
        private void _PostBroadcast(SignalKey signalKey)
        {
            _broadcastingKeys.Remove(signalKey);

            if (_broadcastingKeys.Count == 0 && _hasQueuedUpdate)
            {
                _ProcessQueuedUpdates();
            }
        }

        /// <summary>
        /// Processes all the queued updates that could not be done due 
        /// to the subscriptions being in the middle of a broadcast
        /// </summary>
        private void _ProcessQueuedUpdates()
        {
            // Process the queued unsubscribes
            if (_queuedUnSubscribes.Count > 0)
            {
                for (var i = 0; i < _queuedUnSubscribes.Count; i++)
                {
                    _QueueUnSubscribe(_queuedUnSubscribes[i]);
                }
                _queuedUnSubscribes.Clear();
            }

            // Process the queued group updates
            if (_queuedGroupUpdates.Count > 0)
            {
                for (var i = 0; i < _queuedGroupUpdates.Count; i++)
                {
                    var signalKey = _queuedGroupUpdates[i]._SignalKey;

                    // Call the generic _QueueUpdateGroup with the appropriate types
                    System.Reflection.MethodInfo method = typeof(Signaler).GetMethod("_QueueUpdateGroup");
                    System.Reflection.MethodInfo generic = method.MakeGenericMethod(new Type[] { signalKey.SignalType, signalKey.ResponseType });
                    generic.Invoke(this, new object[] { _queuedGroupUpdates[i], _queuedGroupUpdates[i].Group });
                }
                _queuedGroupUpdates.Clear();
            }

            // Process the queued order updates
            if (_queuedOrderUpdates.Count > 0)
            {
                for (var i = 0; i < _queuedOrderUpdates.Count; i++)
                {
                    var signalKey = _queuedOrderUpdates[i]._SignalKey;

                    // Call the generic _QueueUpdateOrder with the appropriate types
                    System.Reflection.MethodInfo method = typeof(Signaler).GetMethod("_QueueUpdateOrder");
                    System.Reflection.MethodInfo generic = method.MakeGenericMethod(new Type[] { signalKey.SignalType, signalKey.ResponseType });
                    generic.Invoke(this, new object[] { _queuedOrderUpdates[i], _queuedOrderUpdates[i].Order });
                }
                _queuedOrderUpdates.Clear();
            }

            _hasQueuedUpdate = false;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Clear out all subscriptions
        /// </summary>
        public void Purge()
        {
            foreach (var kvp in _subscriptions)
            {
                kvp.Value.Clear();
            }
            _subscriptions.Clear();
        }

        /// <summary>
        /// Subscribe to a particular message signal type.
        /// </summary>
        /// <typeparam name="S">The type of the signal to subscribe to</typeparam>
        /// <param name="subscriber">The receiver that is subscribing to the signal</param>
        /// <param name="action">The delegate to perform on the receiver when the signal is broadcast</param>
        /// <param name="group">An optional group to narrow the broadcast to a small group. Default = null</param>
        /// <param name="order">The optional order that the receiver should be broadcast to. Default = 0</param>
        /// <returns></returns>
        public MessageSubscription<S> Subscribe<S>(ISubscriber subscriber, MessageAction<S> action,
            long? group = null,
            long order = 0)
        {
            var subscription = new MessageSubscription<S>(subscriber, action, group, order);
            _QueueUpdateOrder<S, NoResponse>(subscription, order);
            return subscription;
        }

        /// <summary>
        /// Subscribe to a particular request signal type.
        /// </summary>
        /// <typeparam name="S">The type of the signal to subscribe to</typeparam>
        /// <typeparam name="R">The return type of the response</typeparam>
        /// <param name="subscriber">The receiver that is subscribing to the signal</param>
        /// <param name="action">The delegate to perform on the receiver when the signal is broadcast</param>
        /// <param name="group">An optional group to narrow the broadcast to a small group. Default = null</param>
        /// <param name="order">The optional order that the receiver should be broadcast to. Default = 0</param>
        /// <returns></returns>
        public RequestSubscription<S, R> Subscribe<S, R>(ISubscriber subscriber, RequestAction<S, R> action,
            long? group = null,
            long order = 0)
        {
            var subscription = new RequestSubscription<S, R>(subscriber, action, group, order);
            _QueueUpdateOrder<S, R>(subscription, order);
            return subscription;
        }

        /// <summary>
        /// Broadcast a message signal.
        /// </summary>
        /// <typeparam name="S">Signal type</typeparam>
        /// <param name="broadcaster">The broadcaster of the signal</param>
        /// <param name="signal">The signal to send. Can be left blank if no data is being passed (like a trigger).</param>
        /// <param name="group">The group to use. Can be left blank if no group is being used.</param>
        /// <returns>Returns the count of subscribers that received the signal</returns>
        public int Broadcast<S>(IBroadcaster broadcaster,
            S signal = default(S),
            long? group = null)
        {
            IList subscriptionList;
            SignalKey signalKey;

            // Get the signal key and subscription list
            if (!_PreBroadcast<S, NoResponse>(group, out signalKey, out subscriptionList))
            {
                return 0;
            }

            var signalType = typeof(S);
            int count = 0;
            int i;

            // Cast the subscription list to the signal/response types
            LiteList<MessageSubscription<S>> typeList = (LiteList<MessageSubscription<S>>)subscriptionList;

            // Go through each subscription
            for (i = 0; i < typeList.Count; i++)
            {
                if (_logInfo)
                {
                    Debug.Log(string.Format("Signaler::Broadcasting message signal {0}: {1} of {2}", signalType, i + 1, subscriptionList.Count));
                }

                // Perform the subscription's action
                if (typeList[i].Action(signal))
                {
                    if (_logInfo)
                    {
                        Debug.Log(string.Format("Signaler::Message signal {0} {1} of {2} received by {3}", signalType, i + 1, subscriptionList.Count, typeList[i]._Subscriber));
                    }

                    count++;
                }
            }

            // clean up broadcast
            _PostBroadcast(signalKey);

            return count;
        }

        /// <summary>
        /// Broadcast a request signal and return responses.
        /// </summary>
        /// <typeparam name="S">Signal type</typeparam>
        /// <typeparam name="R">Response type</typeparam>
        /// <param name="broadcaster">The broadcaster of the signal</param>
        /// <param name="responses">A list of responses to return</param>
        /// <param name="signal">The signal to send. Can be left blank if no data is being passed (like a trigger).</param>
        /// <param name="group">The group to use. Can be left blank if no group is being used.</param>
        /// <returns>Returns the count of the subscribers that received the signal</returns>
        public int Broadcast<S, R>(IBroadcaster broadcaster, out List<SignalResponse<R>> responses,
            S signal = default(S),
            long? group = null)
        {
            IList subscriptionList;
            SignalKey signalKey;

            // Get the signal key and subscription list
            if (!_PreBroadcast<S, R>(group, out signalKey, out subscriptionList))
            {
                responses = null;
                return 0;
            }

            var signalType = typeof(S);
            var count = 0;
            int i;

            // Cast the subscription list to the signal/response types
            LiteList<RequestSubscription<S, R>> typeList = (LiteList<RequestSubscription<S, R>>)subscriptionList;
            responses = new List<SignalResponse<R>>();
            R response;

            // Go through each subscription
            for (i = 0; i < typeList.Count; i++)
            {
                if (_logInfo)
                {
                    Debug.Log(string.Format("Signaler::Broadcasting request signal {0}: {1} of {2}", signalType, i + 1, subscriptionList.Count));
                }

                // Perform the subscription's action and get the response
                if (typeList[i].Action(signal, out response))
                {
                    if (_logInfo)
                    {
                        Debug.Log(string.Format("Signaler::Request signal {0} received by {1}", signalType, typeList[i]._Subscriber));
                    }

                    // The response exists, so we add it to the list of responses to return
                    responses.Add(new SignalResponse<R>()
                    {
                        subscriber = typeList[i]._Subscriber,
                        response = response
                    });

                    count++;
                }
            }

            // clean up broadcast
            _PostBroadcast(signalKey);

            return count;
        }
        #endregion
    }
}