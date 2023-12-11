namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using echo17.Signaler.Core;

    public class InputHandler : MonoBehaviour, IBroadcaster, ISubscriber
    {
        private enum GameState
        {
            Playing,
            GameOver
        }

        public float fireInterval;

        // cache the input signal so that we only have to create it once
        private InputSignal _signal = new InputSignal();
        private float _fireCooldown;
        private GameState _gameState;

        void Awake()
        {
            // set up the subscriptions      
            Signaler.Instance.Subscribe<GameOverSignal>(this, OnGameOver);
        }
        
        void Update()
        {
            if (_gameState == GameState.GameOver)
            {
                // if the game state is over and the enter key is pressed, 
                // broadcast the reset game signal
                if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                    _gameState = GameState.Playing;
                    Signaler.Instance.Broadcast<ResetGameSignal>(this);
                }
            }
            else
            {
                if (_fireCooldown > 0)
                {
                    _fireCooldown -= Time.deltaTime;
                }

                // reset the signal each frame
                _signal.action = (int)Action.None;

                // check the keys pressed, adding their actions to the signal

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    _signal.action |= (int)Action.TurnLeft;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    _signal.action |= (int)Action.TurnRight;
                }

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    _signal.action |= (int)Action.Thrust;
                }
                else if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    _signal.action |= (int)Action.CeaseThrust;
                }

                if (Input.GetKey(KeyCode.Space) && _fireCooldown <= 0)
                {
                    _signal.action |= (int)Action.Fire;
                    _fireCooldown = fireInterval;
                }

                if (_signal.action != (int)Action.None)
                {
                    // at least one action was performed,
                    // broadcast the input signal
                    Signaler.Instance.Broadcast<InputSignal>(this, _signal);
                }
            }
        }

        /// <summary>
        /// If the game over signal is broadcast, set the input state of the game
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnGameOver(GameOverSignal signal)
        {
            _gameState = GameState.GameOver;

            return true;
        }
    }
}