namespace echo17.Signaler.Demos.Demo6
{
    public enum Action
    {
        None = 0,
        TurnLeft = 1,
        TurnRight = 2,
        Thrust = 4,
        CeaseThrust = 8,
        Fire = 16
    }

    public struct InputSignal
    {
        public int action;
    }
}
