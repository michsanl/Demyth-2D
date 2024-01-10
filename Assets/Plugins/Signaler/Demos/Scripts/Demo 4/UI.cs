namespace echo17.Signaler.Demos.Demo4
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Shows the results of the order test
    /// </summary>
    public class UI : MonoBehaviour, ISubscriber
    {
        /// <summary>
        /// Result UI label
        /// </summary>
        public UnityEngine.UI.Text resultText;

        void Start()
        {
            // subscribe to the Reset trigger signal
            Signaler.Instance.Subscribe<ResetUI>(this, OnResetUI);

            // subscribe to the acknowledgement signal
            Signaler.Instance.Subscribe<Acknowledgement>(this, OnAcknowledgement);
        }

        /// <summary>
        /// Trigger send to reset the UI label
        /// </summary>
        /// <param name="signal">Signal is blank since it is only a trigger</param>
        /// <returns></returns>
        private bool OnResetUI(ResetUI signal)
        {
            // clear out the results
            resultText.text = "";

            // handled the signal
            return true;
        }

        /// <summary>
        /// Acknowledgement that a receiver processed the order test signal
        /// </summary>
        /// <param name="signal">Message information</param>
        /// <returns></returns>
        private bool OnAcknowledgement(Acknowledgement signal)
        {
            // add the result text to the UI label
            resultText.text += signal.text + "\n";

            // handled the signal
            return true;
        }
    }
}