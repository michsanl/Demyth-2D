namespace echo17.Signaler.Demos.Demo5
{
    using System;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Shows the results of the position request
    /// </summary>
    public class UI : MonoBehaviour, ISubscriber
    {
        /// <summary>
        /// Results label
        /// </summary>
        public UnityEngine.UI.Text resultText;

        void Start()
        {
            // subscribe to the update results signal
            Signaler.Instance.Subscribe<UpdateResults>(this, OnUpdateResults);
        }

        /// <summary>
        /// Handles the update results signal
        /// </summary>
        /// <param name="signal">Information on the results</param>
        /// <returns></returns>
        private bool OnUpdateResults(UpdateResults signal)
        {
            // set the label with the results
            resultText.text = "Results: \n\n" + signal.text;

            // handled signal
            return true;
        }
    }
}