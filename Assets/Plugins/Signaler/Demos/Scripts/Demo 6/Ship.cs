namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    public class Ship : MonoBehaviour, IBroadcaster, ISubscriber, IFriendly
    {
        public float turnSpeed;
        public float maxThrust;
        public float minThrust;
        public float friction;
        public Rect bounds;
        public float shipGunOffset;
        public ParticleSystem thrustParticles;
        public AudioSource thrustSound;
        public AudioSource thrustFadeSound;

        private Transform _cachedTransform;
        private float _angle = 0f;
        private float _thrust;
        private ParticleSystem.EmissionModule _thrustEmission;

        // cache the subscriptions so that we can unsubscribe when the ship is destroyed
        private ISubscription _inputSignalSubscription;
        private ISubscription _getGunSignalSubscription;
        private ISubscription _asteroidHitFriendlySignalSubscription;
        private ISubscription _gameOverSignalSubscription;

        void Awake()
        {
            _cachedTransform = this.transform;
            _thrustEmission = thrustParticles.emission;
            _thrustEmission.enabled = false;

            // Set up subscriptions, caching them so that we can unsubscribe when the ship is destroyed
            _inputSignalSubscription = Signaler.Instance.Subscribe<InputSignal>(this, OnInputSignal);
            _getGunSignalSubscription = Signaler.Instance.Subscribe<GetShipGunRequest, GetShipGunResponse>(this, OnGetShipGunRequest);
            _asteroidHitFriendlySignalSubscription = Signaler.Instance.Subscribe<AsteroidHitFriendlySignal>(this, OnAsteroidHitFriendlySignal);
            _gameOverSignalSubscription = Signaler.Instance.Subscribe<GameOverSignal>(this, OnGameOverSignal);
        }

        void Update()
        {
            if (_thrust > 0)
            {
                _thrust *= friction;
                if (_thrust <= minThrust)
                {
                    _thrust = 0;

                    thrustSound.Stop();
                    thrustFadeSound.Play();
                }
                var radAngle = _angle * Mathf.Deg2Rad;

                var position = _cachedTransform.position;
                position += new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0) * _thrust * Time.deltaTime;
                if (position.x < bounds.xMin) position.x = bounds.xMax;
                if (position.x > bounds.xMax) position.x = bounds.xMin;
                if (position.y < bounds.yMin) position.y = bounds.yMax;
                if (position.y > bounds.yMax) position.y = bounds.yMin;
                _cachedTransform.position = position;
            }
        }

        public void Reset()
        {
            _cachedTransform.position = Vector3.zero;
        }

        /// <summary>
        /// If input signal is broadcast, check for movement actions
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnInputSignal(InputSignal signal)
        {
            if ((signal.action & (int)Action.TurnLeft) == (int)Action.TurnLeft)
            {
                _angle += turnSpeed * Time.deltaTime;
                SetRotation();
            }

            if ((signal.action & (int)Action.TurnRight) == (int)Action.TurnRight)
            {
                _angle -= turnSpeed * Time.deltaTime;
                SetRotation();
            }

            if ((signal.action & (int)Action.Thrust) == (int)Action.Thrust)
            {
                _thrust = maxThrust;
                _thrustEmission.enabled = true;

                if (!thrustSound.isPlaying)
                {
                    thrustSound.Play();
                }
            }

            if ((signal.action & (int)Action.CeaseThrust) == (int)Action.CeaseThrust)
            {
                _thrustEmission.enabled = false;
            }

            return true;
        }

        /// <summary>
        /// If the get gun request is broadcast, return the ship's current state of its gun
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool OnGetShipGunRequest(GetShipGunRequest signal, out GetShipGunResponse response)
        {
            var radAngle = _angle * Mathf.Deg2Rad;

            // formulate a response
            response = new GetShipGunResponse()
            {
                position = _cachedTransform.position + new Vector3(Mathf.Cos(radAngle) * shipGunOffset, Mathf.Sin(radAngle), 0) * shipGunOffset,
                orientation = Quaternion.Euler(0, 0, _angle)
            };

            return true;
        }

        /// <summary>
        /// If a hit signal is broadcast, check if it hit this ship.
        /// If so, destroy it
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

        /// <summary>
        /// If the game over signal is broadcast, stop the thruster particles and sound
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnGameOverSignal(GameOverSignal signal)
        {
            _thrustEmission.enabled = false;
            thrustSound.Stop();

            return true;
        }

        private void SetRotation()
        {
            _cachedTransform.rotation = Quaternion.Euler(0, 0, _angle);
        }

        private void Kill()
        {
            // unsubscribe from the broadcasts.
            // if this is not done, then there will be orphaned
            // subscriptions that will be called when the game is reset.
            // This will cause null errors when it tries to reference the 
            // ship that is no longer attached to the subscriptions.
            _inputSignalSubscription.UnSubscribe();
            _getGunSignalSubscription.UnSubscribe();
            _asteroidHitFriendlySignalSubscription.UnSubscribe();
            _gameOverSignalSubscription.UnSubscribe();

            Destroy(this.gameObject);

            // ship was destroyed, so this is a game over condition.
            Signaler.Instance.Broadcast<GameOverSignal>(this, new GameOverSignal() { state = GameOverState.Lost });
        }
    }
}