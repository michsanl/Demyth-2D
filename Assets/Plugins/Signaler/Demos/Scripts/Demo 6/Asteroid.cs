namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    public class Asteroid : MonoBehaviour, IBroadcaster
    {
        public enum AsteroidSize
        {
            Small,
            Medium,
            Large
        }

        public float smallRotationSpeed;
        public float mediumRotationSpeed;
        public float largeRotationSpeed;

        public float smallScale;
        public float mediumScale;
        public float largeScale;

        public Vector2 minMaxSpeed;

        private AsteroidSize _asteroidSize;
        private Transform _cachedTransform;
        private Vector3 _axis;
        private float _rotationSpeed;
        private Vector3 _direction;
        private Rect _bounds;
        private bool _hit = false;

        // cache the create asteroid signal so that we
        // don't have to keep recreating it
        private CreateAsteroidSignal _createAsteroidSignal;

        public AsteroidSize Size
        {
            get
            {
                return _asteroidSize;
            }
            set
            {
                _asteroidSize = value;

                switch (_asteroidSize)
                {
                    case AsteroidSize.Small:

                        _rotationSpeed = smallRotationSpeed;
                        _cachedTransform.localScale = new Vector3(smallScale, smallScale, smallScale);

                        break;

                    case AsteroidSize.Medium:

                        _rotationSpeed = mediumRotationSpeed;
                        _cachedTransform.localScale = new Vector3(mediumScale, mediumScale, mediumScale);

                        break;

                    case AsteroidSize.Large:

                        _rotationSpeed = largeRotationSpeed;
                        _cachedTransform.localScale = new Vector3(largeScale, largeScale, largeScale);

                        break;
                }

            }
        }

        public Rect Bounds
        {
            set
            {
                _bounds = value;
            }
        }

        void Awake()
        {
            _cachedTransform = this.transform;

            _axis = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            var speed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);
            var radAngle = Random.Range(0, 360f) * Mathf.Deg2Rad;
            _direction = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0) * speed;

            // cache the create asteroid signal so that we don't have to keep recreating it
            _createAsteroidSignal = new CreateAsteroidSignal();
        }

        void Update()
        {
            _cachedTransform.Rotate(_axis, _rotationSpeed * Time.deltaTime);

            var position = _cachedTransform.position;
            position += (_direction * Time.deltaTime);
            if (position.x <= _bounds.xMin || position.x >= _bounds.xMax)
            {
                _direction.x = -_direction.x;
            }
            if (position.y <= _bounds.yMin || position.y >= _bounds.yMax)
            {
                _direction.y = -_direction.y;
            }
            _cachedTransform.position = position;
        }

        /// <summary>
        /// We could have handled the collision in the blaster or ship classes,
        /// but since asteroids collide with both ships and blasters, it was
        /// easier to implement once in the asteroid class. We will broadcast out
        /// whatever the asteroid hit to all subscribers, which is just about every class.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerEnter(Collider collider)
        {
            if (_hit) return;

            // check if what we hit is friendly (ship or blaster)
            // Note: it would be more efficient to filter these by the layers and physics
            // in Unity, but these settings don't export well, so I am checking the
            // collider type here instead.
            var friendly = collider.GetComponent(typeof(IFriendly)) as IFriendly;
            if (friendly == null) return;

            _hit = true;

            switch (_asteroidSize)
            {
                case AsteroidSize.Small:

                    // destroyed

                    break;

                case AsteroidSize.Medium:

                    // Send out a signal to create two small asteroids
                    _createAsteroidSignal.size = AsteroidSize.Small;
                    _createAsteroidSignal.position = _cachedTransform.position;
                    Signaler.Instance.Broadcast<CreateAsteroidSignal>(this, _createAsteroidSignal);
                    Signaler.Instance.Broadcast<CreateAsteroidSignal>(this, _createAsteroidSignal);

                    break;

                case AsteroidSize.Large:

                    // Send out a signal to create two medium asteroids
                    _createAsteroidSignal.size = AsteroidSize.Medium;
                    _createAsteroidSignal.position = _cachedTransform.position;
                    Signaler.Instance.Broadcast<CreateAsteroidSignal>(this, _createAsteroidSignal);
                    Signaler.Instance.Broadcast<CreateAsteroidSignal>(this, _createAsteroidSignal);

                    break;
            }

            // Send out a signal that this asteroid was hit, along with what it hit and where 
            Signaler.Instance.Broadcast<AsteroidHitFriendlySignal>(this, 
                                                                        new AsteroidHitFriendlySignal()
                                                                        {
                                                                            friendly = friendly,
                                                                            position = _cachedTransform.position
                                                                        });
            Destroy(this.gameObject);
        }
    }
}