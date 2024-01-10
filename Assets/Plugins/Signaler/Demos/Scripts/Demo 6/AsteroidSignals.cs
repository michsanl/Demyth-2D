namespace echo17.Signaler.Demos.Demo6
{
    using UnityEngine;

    public struct CreateAsteroidSignal
    {
        public Vector3 position;
        public Asteroid.AsteroidSize size;
    }

    // ship and blasters are considered friendly
    // and both can be hit by the asteroids
    public interface IFriendly { }

    public struct AsteroidHitFriendlySignal
    {
        // the ship or blaster that was hit by the asteroid
        public IFriendly friendly;
        public Vector3 position;
    }

    public struct GetAsteroidCountRequest { }

    public struct GetAsteroidCountResponse
    {
        public int count;
    }
}