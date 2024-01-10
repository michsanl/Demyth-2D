namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    public class FX : MonoBehaviour, ISubscriber
    {
        public GameObject asteroidExplosionPrefab;
        public GameObject shipExplosionPrefab;

        void Awake()
        {
            // Set up subscriptions
            Signaler.Instance.Subscribe<AsteroidHitFriendlySignal>(this, OnAsteroidHitFriendlySignal);
        }

        /// <summary>
        /// If the hit signal was broadcast, create an asteroid explosion fx.
        /// If the ship was hit, then also create a ship explosion fx
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnAsteroidHitFriendlySignal(AsteroidHitFriendlySignal signal)
        {
            GameObject.Instantiate(asteroidExplosionPrefab, signal.position, Quaternion.identity, this.transform);

            if (signal.friendly.GetType() == typeof(Ship))
            {
                GameObject.Instantiate(shipExplosionPrefab, signal.position, Quaternion.identity, this.transform);
            }

            return true;
        }
    }
}
