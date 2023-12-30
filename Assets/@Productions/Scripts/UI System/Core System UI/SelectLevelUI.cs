using Core;
using Core.UI;
using Demyth.Gameplay;
using Demyth.UI;
using MoreMountains.Tools;
using PixelCrushers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UISystem
{
    public class SelectLevelUI : MonoBehaviour
    {
        [SerializeField] private GameHUD _gameHUD;
        [SerializeField] private AudioClip _levelBGM;

        private UIPage _uiPage;
        private LevelManager _levelManager;
        private GameStateService _gameStateService;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
            _levelManager = SceneServiceProvider.GetService<LevelManager>();
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        }       

        public void ButtonGoToLevel(EnumId levelId)
        {
            _levelManager.OpenLevel(levelId);
            _uiPage.Return();
            _gameHUD.gameObject.SetActive(true);
            _gameHUD.Open();

            SaveSystem.SaveToSlot(1);

            _gameStateService?.SetState(GameState.Gameplay);
            Helper.PlayMusic(_levelBGM, .2f, 1, true);
        }

        public void ButtonContinue()
        {
            SaveSystem.LoadFromSlot(1);
            _uiPage.Return();
            _gameHUD.gameObject.SetActive(true);
            _gameHUD.Open();

            _gameStateService?.SetState(GameState.Gameplay);
            Helper.PlayMusic(_levelBGM, .2f, 1, true);
        }
    }
}
