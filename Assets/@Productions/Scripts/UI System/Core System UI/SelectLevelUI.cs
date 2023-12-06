using Core;
using Core.UI;
using Demyth.Gameplay;
using PixelCrushers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UISystem
{
    public class SelectLevelUI : UIPageView
    {

        [SerializeField]
        private EnumId gameViewId;

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
            _uiPage.OpenPage(gameViewId);

            _gameStateService?.SetState(GameState.Gameplay);
        }
    }
}
