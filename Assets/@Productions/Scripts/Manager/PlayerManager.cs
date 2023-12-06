using Core;
using CustomExtensions;
using UnityEngine;

namespace Demyth.Gameplay
{
    public class PlayerManager : SceneService
    {
        public Player Player => spawnedPlayer;

        [SerializeField]
        private Player spawnedPlayer;

        private GameStateService _gameStateService;

        private void Awake()
        {
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _gameStateService[GameState.Gameplay].onEnter += ActivatePlayer;
            _gameStateService[GameState.MainMenu].onEnter += HidePlayerOnMainMenu;
        }

        private void HidePlayerOnMainMenu(GameState obj)
        {
            spawnedPlayer.SetActive(false);
        }

        private void ActivatePlayer(GameState obj)
        {
            spawnedPlayer.SetActive(true);
            spawnedPlayer.ResetUnitCondition();
        }
    }
}
