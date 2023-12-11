namespace echo17.Signaler.Demos.Demo6
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using echo17.Signaler.Core;

    public class UI : MonoBehaviour, ISubscriber
    {
        public Text countText;
        public Text gameOverText;
        public Text replayText;
        public Text instructionsText;

        private int _count = 0;

        void Awake()
        {
            // Set up the subscriptions
            Signaler.Instance.Subscribe<AsteroidHitFriendlySignal>(this, OnAsteroidHitFriendlySignal);
            Signaler.Instance.Subscribe<GameOverSignal>(this, OnGameOverSignal);
            Signaler.Instance.Subscribe<ResetGameSignal>(this, OnResetGameSignal);
        }

        /// <summary>
        /// If a hit signal is broadcast, update the asteroid count text
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnAsteroidHitFriendlySignal(AsteroidHitFriendlySignal signal)
        {
            _count++;
            SetAsteroidsDestroyedText();

            return true;
        }

        /// <summary>
        /// If a game over signal is broadcast, set up the game over UI
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnGameOverSignal(GameOverSignal signal)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = signal.state == GameOverState.Start ? "" : "Game Over\nYou " + (signal.state == GameOverState.Won ? "Win!" : "Lose...");

            replayText.gameObject.SetActive(true);
            instructionsText.gameObject.SetActive(false);

            return true;
        }

        /// <summary>
        /// If a reset signal is broadcast, set up the in game UI
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private bool OnResetGameSignal(ResetGameSignal signal)
        {
            _count = 0;
            SetAsteroidsDestroyedText();

            gameOverText.gameObject.SetActive(false);
            replayText.gameObject.SetActive(false);
            instructionsText.gameObject.SetActive(true);

            return true;
        }

        private void SetAsteroidsDestroyedText()
        {
            countText.text = "Asteroids Destroyed: " + _count.ToString();
        }
    }
}
