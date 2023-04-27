using CustomCode.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace Demyth.UI
{
    public class MainMenuPage : StandardPage
    {
        [Title("Buttons")]
        [SerializeField]
        private Button playButton;

        protected override void OnInitialize()
        {
            playButton.onClick.AddListener(PlayGame);
        }

        private void PlayGame()
        {
            Global.SceneLoader.LoadScene(Global.GlobalSettings.GameplayScene);
        }
    }
}