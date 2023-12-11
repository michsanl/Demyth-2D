namespace echo17.Signaler.Core
{
    using System;
    using System.Collections;

    /// <summary>
    /// Delegate to call the action on a receiver when a message signal is received
    /// </summary>
    /// <typeparam name="S">Signal type</typeparam>
    /// <param name="signal">Signal sent</param>
    /// <returns>True if received, false otherwise</returns>
    public delegate bool MessageAction<S>(S signal);

    /// <summary>
    /// Simple struct used only as a type to key in the Signaler dictionary lookup
    /// </summary>
    public struct NoResponse { }

    /// <summary>
    /// Subscription to a message broadcast. A receiver requests this and the Signaler will send out the
    /// broadcast to whomever has one.
    /// </summary>
    /// <typeparam name="S">Signal type</typeparam>
    public class MessageSubscription<S> : Subscription<S, NoResponse>
    {
        #region Private and Hidden

        /// <summary>
        /// Create a new subscription. This should only be called by the Signaler
        /// </summary>
        /// <param name="subscriber">The subscriber of this subscription</param>
        /// <param name="action">The action to call when the signal is broadcast</param>
        /// <param name="group">The group of the subscription</param>
        /// <param name="order">The order the subscription is processed in</param>
        public MessageSubscription(ISubscriber subscriber, MessageAction<S> action, long? group, long order) :
            base(subscriber, group, order)
        {
            Action = action;
        }

        #endregion

        #region Public

        /// <summary>
        /// The action to perform when the signal is broadcast to the receiver
        /// </summary>
        public MessageAction<S> Action { get; set; }

        #endregion
    }
}
