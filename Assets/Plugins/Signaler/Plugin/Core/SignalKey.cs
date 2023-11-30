namespace echo17.Signaler.Core
{
    using System;

    /// <summary>
    /// Private struct to key the signal dictionary. It uses three keys: the signal type, response type, and group
    /// to look up the appropriate subscription list.
    /// </summary>
    public struct SignalKey
    {
        public Type SignalType { get; private set; }
        public Type ResponseType { get; private set; }
        public long? Group { get; private set; }

        public SignalKey(Type signalType, Type responseType, long? group)
        {
            SignalType = signalType;
            ResponseType = responseType;
            Group = group;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(SignalKey)) return false;
            return Equals((SignalKey)obj);
        }

        public bool Equals(SignalKey obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.SignalType, SignalType) && Equals(obj.ResponseType, ResponseType) && Equals(obj.Group, Group);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + SignalType.GetHashCode();
                hash = (hash * 23) + ResponseType.GetHashCode();
                hash = (hash * 23) + (Group == null ? 0 : Group.GetHashCode());
                return hash;
            }
        }
    }
}