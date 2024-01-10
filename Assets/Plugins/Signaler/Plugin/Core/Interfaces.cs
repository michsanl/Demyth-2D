namespace echo17.Signaler.Core
{
    using System.Collections;

    public interface IBroadcaster { }

    public interface ISubscriber { }

    public interface ISubscription
    {
        SignalKey _SignalKey { get; set; }
        IList _SubscriptionList { get; set; }
        long? Group { get; set; }
        long Order { get; set; }

        void UnSubscribe();
    }
}
