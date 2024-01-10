namespace echo17.Signaler.Demos.Demo2
{
    using System;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Input class that sends out a broadcast message signal
    /// </summary>
    public class Sender : MonoBehaviour, IBroadcaster
    {
        public CubeUpdate cubeUpdate;

        void Update()
        {
            // if the U key is pressed, send out a signal to the null group (no group)
            if (Input.GetKeyDown(KeyCode.U))
            {
                Broadcast(null);
            }

            // check the number key and send a broadcast based on that number

            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                Broadcast(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                Broadcast(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                Broadcast(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                Broadcast(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                Broadcast(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                Broadcast(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                Broadcast(6);
            }
        }

        /// <summary>
        /// Broadcasts the signal to a specific group
        /// </summary>
        /// <param name="group">The group to send to</param>
        private void Broadcast(long? group)
        {
            // broadcast signal to grouped receivers
            Signaler.Instance.Broadcast<CubeUpdate>(this, signal: cubeUpdate, group: group);
        }
    }
}
