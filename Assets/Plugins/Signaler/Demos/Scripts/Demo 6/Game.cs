namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    public class Game : MonoBehaviour, IBroadcaster, ISubscriber
    {
        public bool debugSignals = false;
        public GameObject shipPrefab;
        public AudioSource winSound;

        private int _asteroidsRemaining;
        private Ship _ship;

        void Awake()
        {
            // If debugging, set the signal manager to log info
            if (debugSignals)
            {
                Signaler.Instance.LogLevel |= SignalLogLevelEnum.Info;
            }

            // Set up subscriptions
            Signaler.Instance.Subscribe<ResetGameSignal>(this, OnResetGameSignal);
            Signaler.Instance.Subscribe<AsteroidHitFriendlySignal>(this, OnAsteroidHitFriendlySignal);
            Signaler.Instance.Subscribe<GameOverSignal>(this, OnGameOverSignal);
        }

        void Start()
        {
            // Game is over, send out the signal to start
            Signaler.Instance.Broadcast<GameOverSignal>(this, new GameOverSignal() { state = GameOverState.Start });
        }

        void OnValidate()
        {
            // if the debug changes in the inspector, update the log
            // level of the signal manager
            if (debugSignals)
            {
                Signaler.Instance.LogLevel |= SignalLogLevelEnum.Info;
            }
            else
            {
                Signaler.Instance.LogLevel &= ~SignalLogLevelEnum.Info;
            }
        }

        /// <summary>
        /// If the reset signal is broadcast, reset the ship (or create it),
        /// and get the total number of asteroids that will be created in the game.
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnResetGameSignal(ResetGameSignal signal)
        {
            if (_ship != null)
            {
                _ship.Reset();
            }
            else
            {
                var obj = GameObject.Instantiate(shipPrefab, Vector3.zero, Quaternion.identity, this.transform);
                _ship = obj.GetComponent<Ship>();
            }

            // request the total asteroids that will be created in the game
            List<SignalResponse<GetAsteroidCountResponse>> responses;
            if (Signaler.Instance.Broadcast<GetAsteroidCountRequest, GetAsteroidCountResponse>(this, out responses) > 0)
            {
                // a response was sent back
                _asteroidsRemaining = responses[0].response.count;
            }

            return true;
        }

        /// <summary>
        /// If the hit signal was broadcast, check to see if the game over condition exists (zero asteroids remaining)
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnAsteroidHitFriendlySignal(AsteroidHitFriendlySignal signal)
        {
            _asteroidsRemaining--;

            if (_asteroidsRemaining == 0)
            {
                // No asteroids remaining, so we broadcast a game over signal
                Signaler.Instance.Broadcast<GameOverSignal>(this, new GameOverSignal() { state = GameOverState.Won });
            }

            return true;
        }

        /// <summary>
        /// If the game over signal is broadcase, we play a winning sound if appropriate.
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnGameOverSignal(GameOverSignal signal)
        {
            if (signal.state == GameOverState.Won)
            {
                winSound.Play();
            }

            return true;
        }
    }
}
