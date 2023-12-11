namespace echo17.Signaler.Demos.Demo3
{
    /// <summary>
    /// And - all the filters must exist
    /// Or - Any of the filters can exist
    /// </summary>
    public enum FilterType
    {
        And,
        Or
    }

    /// <summary>
    /// Signal to send to the recievers
    /// </summary>
    public struct SaySomething
    {
        /// <summary>
        /// list of filters to use
        /// </summary>
        public long[] filter;

        /// <summary>
        /// The filter type to apply
        /// </summary>
        public FilterType filterType;

        /// <summary>
        /// The message to send
        /// </summary>
        public string text;
    }
}