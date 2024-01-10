namespace echo17.Signaler.Demos.Demo3
{
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;
    using System.Linq;

    /// <summary>
    /// Sends a signal with an advanced filter based on options selected in the UI
    /// </summary>
    public class Sender : MonoBehaviour, IBroadcaster
    {
        /// <summary>
        ///  The toggles that set the advanced filter
        /// </summary>
        public UnityEngine.UI.Toggle[] filterToggles;

        /// <summary>
        ///  The message to send to the receivers
        /// </summary>
        public UnityEngine.UI.InputField inputMessage;

        /// <summary>
        /// The results of the broadcast
        /// </summary>
        public UnityEngine.UI.Text topText;

        /// <summary>
        /// How long to show the results
        /// </summary>
        public float topTextTime;

        /// <summary>
        /// Signal to send.
        /// We create a cached signal so that we don't have
        /// to instantiate a new signal each time it is sent.
        /// </summary>
        private SaySomething _signal;

        /// <summary>
        /// The time left to show the results
        /// </summary>
        private float _topTextCountdown = 0;

        void Start()
        {
            // cache the signal to save on memory allocation and disposal
            _signal = new SaySomething();
        }

        void Update()
        {
            // show the results if necessary
            if (_topTextCountdown > 0)
            {
                _topTextCountdown -= Time.deltaTime;
                if (_topTextCountdown <= 0)
                {
                    topText.text = "";
                }
            }
        }

        /// <summary>
        /// Sends message with filters And-ed together
        /// </summary>
        public void SendFilteredAndMessage()
        {
            SendMessage(GetFilter(), FilterType.And);
        }

        /// <summary>
        /// Sends message with no filters And-ed together
        /// </summary>
        public void SendNoFiltersAndMessage()
        {
            SendMessage(null, FilterType.And);
        }

        /// <summary>
        /// Sends message with filters Or-ed together
        /// </summary>
        public void SendFilteredOrMessage()
        {
            SendMessage(GetFilter(), FilterType.Or);
        }

        /// <summary>
        /// Sends message with no filters Or-ed together
        /// </summary>
        public void SendNoFiltersOrMessage()
        {
            SendMessage(null, FilterType.Or);
        }

        /// <summary>
        /// Sends a message with filters and filter type
        /// </summary>
        /// <param name="filter">A list of advanced filters</param>
        /// <param name="filterType">Filter type to use</param>
        private void SendMessage(long[] filter, FilterType filterType)
        {
            // set up the cached signal with the filter, filter type, and message
            _signal.filter = filter;
            _signal.filterType = filterType;
            _signal.text = inputMessage.text;

            // broadcast the signal and get the responder count
            var responderCount = Signaler.Instance.Broadcast<SaySomething>(this, signal: _signal);

            // show the results of the broadcast
            topText.text = (responderCount == 0 ? "No" : responderCount.ToString()) + " subscriber" + (responderCount > 1 ? "s" : "") + " responded";
            _topTextCountdown = topTextTime;
        }

        /// <summary>
        /// Gets a list of long values based on the toggles selected in the UI
        /// </summary>
        /// <returns></returns>
        private long[] GetFilter()
        {
            return filterToggles.Select((v, i) => new { Index = i, Value = v })
                    .Where(x => x.Value.isOn)
                    .Select(x => (long)x.Index)
                    .ToArray();
        }
    }
}