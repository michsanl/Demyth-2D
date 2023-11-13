namespace echo17.Signaler.Demos.Demo6
{
    public enum GameOverState
    {
        Start,
        Won,
        Lost
    }

    public struct GameOverSignal
    {
        public GameOverState state;
    }

    public struct ResetGameSignal { }
}
