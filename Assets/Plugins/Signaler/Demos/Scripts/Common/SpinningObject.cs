namespace echo17.Signaler.Demos
{
    using System;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Simple class to control a spinning 3D object
    /// </summary>
    public class SpinningObject : MonoBehaviour
    {
        public Transform spinTransform;
        public float speed = 1f;
        public Rect bounds;

        protected Transform _transform;
        protected Vector3 _direction;

        public void SpinInitialize()
        {
            _transform = this.transform;
            _transform.position = new Vector3(UnityEngine.Random.Range(bounds.xMin, bounds.xMax), UnityEngine.Random.Range(bounds.yMin, bounds.yMax), 0);

            var angle = UnityEngine.Random.Range(0, 360f) * Mathf.Deg2Rad;
            _direction = new Vector3(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed, 0);
        }

        protected void SpinUpdate()
        {
            spinTransform.Rotate(new Vector3(30, 30, 30) * Time.deltaTime);

            var position = _transform.position;
            position += _direction * Time.deltaTime;
            if (position.x <= bounds.xMin || position.x >= bounds.xMax)
            {
                _direction.x = -_direction.x;
            }
            if (position.y <= bounds.yMin || position.y >= bounds.yMax)
            {
                _direction.y = -_direction.y;
            }
            _transform.position = position;
        }
    }
}
