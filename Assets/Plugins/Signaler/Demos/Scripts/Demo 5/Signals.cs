namespace echo17.Signaler.Demos.Demo5
{ 
    using UnityEngine;

    /// <summary>
    /// Direction filter
    /// </summary>
    public enum Direction
    {
        AnyDirection,
        Up,
        Down
    }

    /// <summary>
    /// Request to get the position with a filtered direction
    /// </summary>
    public struct GetPositionRequest
    {
        public Direction directionFilter;
    }

    /// <summary>
    /// Response of the position data
    /// </summary>
    public struct GetPositionOutput
    { 
        public Vector2 outputPosition;
    }

    /// <summary>
    /// Signal to send with the results of the request
    /// </summary>
    public struct UpdateResults
    {
        public string text;
    }
}
