namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    public class BlasterManager : MonoBehaviour, IBroadcaster, ISubscriber
    {
        public GameObject blasterPrefab;

        void Awake()
        {
            // Set up subscriptions
            Signaler.Instance.Subscribe<ResetGameSignal>(this, OnResetGameSignal);
            Signaler.Instance.Subscribe<InputSignal>(this, OnInputSignal);
        }

        /// <summary>
        /// When the reset signal is broadcast, destroy any current blasters
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnResetGameSignal(ResetGameSignal signal)
        {
            var blasters = GetComponentsInChildren<Blaster>();
            foreach (var blaster in blasters)
            {
                Destroy(blaster.gameObject);
            }

            return true;
        }

        /// <summary>
        /// When the input signal is broadcast, check to see if it includes a fire action.
        /// If so, instantiate the blaster at the ship's gun position
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnInputSignal(InputSignal signal)
        {
            if ((signal.action & (int)Action.Fire) == (int)Action.Fire)
            {
                // get the ship's gun position by broadcasting a request for it
                List<SignalResponse<GetShipGunResponse>> responses;
                if (Signaler.Instance.Broadcast<GetShipGunRequest, GetShipGunResponse>(this, out responses) > 0)
                {
                    // the ship acknowleged the broadcast and sent a response back.
                    // use the response to set the position and orientation of the blaster.
                    GameObject.Instantiate(blasterPrefab, responses[0].response.position, responses[0].response.orientation, this.transform);
                }
            }

            return true;
        }
    }
}