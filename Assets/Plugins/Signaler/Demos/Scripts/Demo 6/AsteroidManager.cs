namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    public class AsteroidManager : MonoBehaviour, ISubscriber
    {
        public int asteroidCount;
        public Asteroid asteroidPrefab;
        public Vector2 minMaxSpawnRadius;
        public Rect bounds;
        public float spawnBorder;

        void Awake()
        {
            // Set up our subscriptions
            Signaler.Instance.Subscribe<ResetGameSignal>(this, OnResetGameSignal);
            Signaler.Instance.Subscribe<CreateAsteroidSignal>(this, OnCreateAsteroidSignal);
            Signaler.Instance.Subscribe<GetAsteroidCountRequest, GetAsteroidCountResponse>(this, OnGetAsteroidCountRequest);
        }

        /// <summary>
        /// When the reset signal is broadcast, destroy all the asteroids, then 
        /// create new ones
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnResetGameSignal(ResetGameSignal signal)
        {
            var asteroids = GetComponentsInChildren<Asteroid>();
            foreach (var asteroid in asteroids)
            {
                Destroy(asteroid.gameObject);
            }

            CreateAsteroidSignal create = new CreateAsteroidSignal();

            for (var i = 0; i < asteroidCount; i++)
            {
                var spawnRadius = Random.Range(minMaxSpawnRadius.x, minMaxSpawnRadius.y);
                var spawnAngle = Random.Range(0, 360f) * Mathf.Deg2Rad;
                create.position = new Vector3(Mathf.Cos(spawnAngle), Mathf.Sin(spawnAngle), 0) * spawnRadius;
                create.size = Asteroid.AsteroidSize.Large;

                OnCreateAsteroidSignal(create);
            }

            return true;
        }

        /// <summary>
        /// When the create asteroid signal is broadcast, set up an asteroid at a particular location
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnCreateAsteroidSignal(CreateAsteroidSignal signal)
        {
            signal.position.x = Mathf.Clamp(signal.position.x, bounds.xMin, bounds.xMax);
            signal.position.y = Mathf.Clamp(signal.position.y, bounds.yMin, bounds.yMax);

            var asteroid = GameObject.Instantiate<Asteroid>(asteroidPrefab, signal.position, Quaternion.identity, this.transform);
            asteroid.Size = signal.size;
            asteroid.Bounds = bounds;

            return true;
        }

        /// <summary>
        /// Return the number of total asteroids that will be created over the course of the game.
        /// Each large asteroid will make two medium, which will in turn each make two small, so
        /// the total asteroids is 7 times the starting count.
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool OnGetAsteroidCountRequest(GetAsteroidCountRequest signal, out GetAsteroidCountResponse response)
        {
            response = new GetAsteroidCountResponse() { count = asteroidCount * 7 };

            return true;
        }
    }
}