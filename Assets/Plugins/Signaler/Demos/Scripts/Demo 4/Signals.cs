namespace echo17.Signaler.Demos.Demo4
{
    /// <summary>
    /// Simple trigger message with no data
    /// </summary>
    public struct OrderTest { }

    /// <summary>
    /// Simple trigger message with no data
    /// </summary>
    public struct ResetUI { }

    /// <summary>
    /// Message to acknowledge that a order test signal was received
    /// and in what order
    /// </summary>
    public struct Acknowledgement
    {
        public string text;
    }
}