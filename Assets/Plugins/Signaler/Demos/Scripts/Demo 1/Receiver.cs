namespace echo17.Signaler.Demos.Demo1
{
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Receiver class to listen to broadcasts
    /// </summary>
    public class Receiver : ISubscriber
    {
        private string _name;

        public Receiver(string name)
        {
            // set up the name of the receiver so we can see who heard the broadcast
            _name = name;

            // subscribe to the broadcast
            Signaler.Instance.Subscribe<SaySomething>(this, OnHandleSaySomething);
        }

        /// <summary>
        /// Handler for the broadcast
        /// </summary>
        /// <param name="signal">Signal for the broadcast</param>
        /// <returns></returns>
        private bool OnHandleSaySomething(SaySomething signal)
        {
            // Output to the console that the signal was received, along with the message text of the signal
            Debug.Log(_name + " handles signal. Message: " + signal.text);

            // acknowledge that the signal was handled
            return true;
        }
    }
}