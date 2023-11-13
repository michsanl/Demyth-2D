namespace echo17.Signaler.Demos.Demo2
{
    using System;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Creates a number of receiver gameobjects in the scene
    /// </summary>
    public class ReceiverCreator : MonoBehaviour
    {
        /// <summary>
        /// The prefab of the receiver to instantiate off of
        /// </summary>
        public Receiver receiverPrefab;

        void Start()
        {
            // create three no group receivers
            for (var i = 0; i < 3; i++)
            {
                Create(null);
            }

            // create a number of receivers that use groups
            for (var i = 0; i <= 6; i++)
            {
                // create the receiver
                Create(i);

                // if the number is even, create another copy of the receiver
                // to show that multiple receivers can have the same group
                if (i % 2 == 0)
                {
                    Create(i);
                }
            }
        }

        /// <summary>
        /// Creates a receiver with a group
        /// </summary>
        /// <param name="number"></param>
        private void Create(long? group)
        {
            // instantiate the receiver gameobject based on the prefab
            var receiver = GameObject.Instantiate<Receiver>(receiverPrefab);

            // set the group of the receiver (which also creates a subscription to listen to the broadcast)
            receiver.SetGroup(group);
        }
    }
}
