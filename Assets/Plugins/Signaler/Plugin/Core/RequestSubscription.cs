namespace echo17.Signaler.Core
{
    using System;

    /// <summary>
    /// Delegate to call the action on a receiver when a signal is received
    /// </summary>
    /// <typeparam name="S">Signal type</typeparam>
    /// <typeparam name="R">Response type</typeparam>
    /// <param name="signal">Signal sent</param>
    /// <param name="response">Response returned from receiver</param>
    /// <returns>True if received, false otherwise</returns>
    public delegate bool RequestAction<S, R>(S signal, out R response);

    /// <summary>
    /// A signal response sent back from Signaler containing
    /// both the response and the subscriber (receiver) that sent it.
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public struct SignalResponse<R>
    {
        public ISubscriber subscriber;
        public R response;
    }

    /// <summary>
    /// Subscription to a broadcast. A receiver requests this and the Signaler will send out the
    /// broadcast to whomever has one.
    /// </summary>
    /// <typeparam name="S">Signal type</typeparam>
    /// <typeparam name="R">Response type</typeparam>
    public class RequestSubscription<S, R> : Subscription<S, R>
    {
        #region Private and Hidden

        /// <summary>
        /// Create a new subscription. This should only be called by the Signaler
        /// </summary>
        /// <param name="subscriber">The subscriber of this subscription</param>
        /// <param name="action">The action to call when the signal is broadcast</param>
        /// <param name="group">The group of the subscription</param>
        /// <param name="order">The order the subscription is processed in</param>
        public RequestSubscription(ISubscriber subscriber, RequestAction<S, R> action, long? group, long order) :
            base(subscriber, group, order)
        {
            Action = action;
        }

        #endregion

        #region Public

        /// <summary>
        /// The action to perform when the signal is broadcast to the receiver
        /// </summary>
        public RequestAction<S, R> Action { get; set; }

        #endregion
    }
}
