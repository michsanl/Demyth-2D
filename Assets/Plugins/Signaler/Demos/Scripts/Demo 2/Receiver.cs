namespace echo17.Signaler.Demos.Demo2
{
    using System;
    using UnityEngine;
    using echo17.Signaler.Core;

    /// <summary>
    /// Receiver that will turn color and display text when it receives a broadcast signal
    /// </summary>
    public class Receiver : SpinningObject, ISubscriber
    {
        /// <summary>
        /// How long the text and color should show
        /// </summary>
        public float displayTime;

        /// <summary>
        /// The text mesh that will display the message
        /// </summary>
        public TextMesh textMesh;

        /// <summary>
        /// The renderer of the cube so that we can change its color
        /// </summary>
        public MeshRenderer cubeRenderer;

        /// <summary>
        /// The group of the receiver
        /// </summary>
        private long? _group;

        /// <summary>
        /// How much time is left to display the message
        /// </summary>
        private float _displayCountdown;

        /// <summary>
        /// The original color of the cube
        /// </summary>
        private Color _originalColor;

        /// <summary>
        /// The subscription we have registered with the SignalMananger.
        /// We keep track of it so that we can dispose and create a new one
        /// if necessary.
        /// </summary>
        private MessageSubscription<CubeUpdate> _subscription;

        void Awake()
        {
            // grab the original color
            _originalColor = cubeRenderer.material.GetColor("_Color");

            // initialize the spin base object
            SpinInitialize();
        }

        /// <summary>
        /// Sets the group of the subscription,
        /// creating a new subscription if necessary
        /// </summary>
        /// <param name="group">Group of the subscription</param>
        public void SetGroup(long? group)
        {
            _group = group;

            if (_subscription != null)
            {
                // subscription already exists, so we just update the group
                _subscription.Group = _group;
            }
            else
            {
                // create a new subscription with the group
                _subscription = Signaler.Instance.Subscribe<CubeUpdate>(this, OnCubeUpdate, group: _group);
            }

            // update the text to show the group number of the cube
            textMesh.text = FormatGroup();
        }

        void Update()
        {
            // update the base spin object
            SpinUpdate();

            // if we are displaying message text
            if (_displayCountdown > 0)
            {
                // countdown the display time
                _displayCountdown -= Time.deltaTime;

                // if the display time is over
                if (_displayCountdown <= 0)
                {
                    // reset the display time
                    _displayCountdown = 0;

                    // update the text to show the group number
                    textMesh.text = FormatGroup();

                    // reset the color of the cube
                    cubeRenderer.material.SetColor("_Color", _originalColor);
                }
            }
        }

        /// <summary>
        /// Handles the CubeUpdate signal
        /// </summary>
        /// <param name="signal">Signal with text and color</param>
        /// <returns></returns>
        private bool OnCubeUpdate(CubeUpdate signal)
        {
            // set the text to the group and message text
            textMesh.text = FormatGroup() + ": " + signal.text;

            // set the cube color
            cubeRenderer.material.SetColor("_Color", signal.color);

            // set the countdown timer
            _displayCountdown = displayTime;

            return true;
        }

        /// <summary>
        /// Formats the group number into a text string
        /// </summary>
        /// <returns></returns>
        private string FormatGroup()
        {
            return _group == null ? "No Group" : (_group ?? 0L).ToString();
        }
    }
}
