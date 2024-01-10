namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    public class Explosion : MonoBehaviour
    {
        public float lifetime;

        void Update()
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
