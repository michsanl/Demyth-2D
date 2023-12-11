namespace echo17.Signaler.Demos.Demo3
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;
    using System.Linq;

    /// <summary>
    /// Receiver class that will handle the broadcast message.
    /// Note that this class does not use the built in long group
    /// of the subscription, but rather handles the filtering within
    /// the signal handler itself. This allows more complex filtering, 
    /// but more receivers will have to handle the signal.
    /// </summary>
    public class Receiver : MonoBehaviour, ISubscriber
    {
        /// <summary>
        /// Whether the receiver should use an advanced filter
        /// </summary>
        public bool useFilter;

        /// <summary>
        /// The advanced filters to be used 
        /// </summary>
        public ObjectType[] filter;

        /// <summary>
        /// Transform of the receiver
        /// </summary>
        public Transform objectTransform;
        /// <summary>
        /// The text to display on the receiver
        /// </summary>
        public TextMesh messageText;
        /// <summary>
        /// The amount of time to display the message
        /// </summary>
        public float messageTime;

        /// <summary>
        /// Filtered list converted to long values
        /// </summary>
        private long[] _longFilter;

        /// <summary>
        /// Subscription to the broadcast
        /// </summary>
        private MessageSubscription<SaySomething> _subscription;

        /// <summary>
        /// If the object is rotating
        /// </summary>
        private bool _rotating;

        /// <summary>
        /// The time left to display the message
        /// </summary>
        private float _messageCountdown = 0;

        void Start()
        {
            // subscribe to the broadcast
            _subscription = Signaler.Instance.Subscribe<SaySomething>(this, OnSaySomething);
            
            // set up the long filter list and set rotating
            _longFilter = GetFilter();
            SetRotating();

            messageText.text = "";
        }

        void Update()
        {
            // rotate the object if necessary
            if (_rotating)
            {
                objectTransform.Rotate(new Vector3(30, 30, 30) * Time.deltaTime);
            }

            // if the message countdown is active
            if (_messageCountdown > 0)
            {
                // countdown the time
                _messageCountdown -= Time.deltaTime;

                // countdown expired
                if (_messageCountdown <= 0)
                {
                    // reset the message
                    messageText.text = "";
                }
            }
        }

        /// <summary>
        /// Called when the inspector values are changed
        /// </summary>
        void OnValidate()
        {
            // check if there is a subscription set up
            if (_subscription != null)
            {
                // get the long filter list and set rotating
                _longFilter = GetFilter();
                SetRotating();
            }
        }

        /// <summary>
        /// Handle the signal
        /// </summary>
        /// <param name="signal">Signal that is broadcast containing an advanced filter</param>
        /// <returns></returns>
        private bool OnSaySomething(SaySomething signal)
        {
            long[] appliedFilters;
            bool match;

            // if the signal filter and the receivers filters are both null
            if (signal.filter == null || _longFilter == null)
            {
                // no filters applied
                appliedFilters = null;

                // check if there is a match
                // if the And type is used, the only match if both signal filter and receiver filter are null
                // if the Or type is used, then return a match
                match = (signal.filterType == FilterType.And ? signal.filter == _longFilter : true);
            }
            else
            {
                // set up the applied filters as the intersection of the long filter list and the signal filter
                appliedFilters = _longFilter.Intersect(signal.filter).ToArray();

                // check if there is a match
                // if the And type is used, then only match if the applied filter length is greater than the signal filter length and applied filters length is greater than zero. This means that all values matched
                // if the Or type is used, then only match if the applied filters length is greater than zero
                match = (signal.filterType == FilterType.And ? appliedFilters.Length >= signal.filter.Length : true) && appliedFilters.Length > 0;
            }

            if (match)
            {
                // if there was a match, then show the signal text, the filters that were used and start the countdown
                messageText.text = FormatMessage(signal.text, appliedFilters);
                _messageCountdown = messageTime;

                // received signal
                return true;
            }
            else
            {
                // signal was not received since the filters did not match
                return false;
            }
        }

        /// <summary>
        /// Helper method to convert the enum list into a long list
        /// </summary>
        /// <returns>list of long values</returns>
        private long[] GetFilter()
        {
            if (useFilter)
            {
                return Array.ConvertAll<ObjectType, long>(filter, delegate (ObjectType value) { return (long)value; });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the rotating member if one of the filters is Rotating
        /// </summary>
        private void SetRotating()
        {
            _rotating = (useFilter ? filter.Where(x => x == ObjectType.Rotating).Count() > 0 : false);
        }

        /// <summary>
        /// Formats the message based on the message text and filters used
        /// </summary>
        /// <param name="message">Message from the signal</param>
        /// <param name="filter">Filters applied</param>
        /// <returns></returns>
        private string FormatMessage(string message, long[] filter)
        {
            if (!useFilter)
            {
                message += "\n(No Filter)";
            }
            else
            {
                if (filter == null)
                {
                    message += "\n(All)";
                }
                else
                {
                    var enumFilter = Array.ConvertAll<long, ObjectType>(filter, delegate (long value) { return (ObjectType)value; });
                    for (var i = 0; i < enumFilter.Length; i++)
                    {
                        if (i == 0)
                        {
                            message += "\n(";
                        }
                        message += enumFilter[i].ToString();
                        if (i < enumFilter.Length - 1)
                        {
                            message += ",";
                        }
                        else
                        {
                            message += ")";
                        }
                    }
                }
            }

            return message;
        }
    }
}