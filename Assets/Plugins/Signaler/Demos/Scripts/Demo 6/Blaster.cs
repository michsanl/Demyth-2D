namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    public class Blaster : MonoBehaviour, ISubscriber, IFriendly
    {
        public float speed;
        public Rect bounds;

        private Transform _cachedTransform;
        private MessageSubscription<AsteroidHitFriendlySignal> _asteroidHitFriendlySignalSubscription;

        void Awake()
        {
            _cachedTransform = this.transform;

            // cache the subscription so that we can unsubscribe when the blaster is removed
            _asteroidHitFriendlySignalSubscription = Signaler.Instance.Subscribe<AsteroidHitFriendlySignal>(this, OnAsteroidHitFriendlySignal);
        }

        void Update()
        {
            var position = _cachedTransform.position;
            position += _cachedTransform.right * speed * Time.deltaTime;
            if (position.x <= bounds.xMin || position.x >= bounds.xMax || position.y <= bounds.yMin || position.y >= bounds.yMax)
            {
                Kill();
            }
            else
            {
                _cachedTransform.position = position;
            }
        }

        /// <summary>
        /// When the hit signal is broadcast, check to see if it hit this blaster.
        /// If so, kill it.
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnAsteroidHitFriendlySignal(AsteroidHitFriendlySignal signal)
        {
            if (signal.friendly == (IFriendly)this)
            {
                Kill();
                return true;
            }

            return false;
        }

        private void Kill()
        {
            // unsubscribe from the hit broadcast
            _asteroidHitFriendlySignalSubscription.UnSubscribe();
            Destroy(this.gameObject);
        }
    }
}
