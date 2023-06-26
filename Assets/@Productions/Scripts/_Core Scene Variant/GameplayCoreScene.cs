using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomTools.Core
{
    public class GameplayCoreScene : CoreScene
    {
        [Title("Gamplay Core Scene")]
        [SerializeField]
        private Player playerPrefab;

        protected override IEnumerator OnActivate()
        {
            yield return base.OnActivate();
            
            // yield return SpawnPlayer();
            SetPlayerContext();
            SetLevelOnGameStart();
            // TODO : Prepare Level            
        }

        private IEnumerator SpawnPlayer()
        {
            var starterPoint = Context.LevelManager.CurrentLevel.StarterPosition;

            var player = Instantiate(playerPrefab, starterPoint, Quaternion.identity);
            // //TODO : initiate player
            Context.Player = player;
            player.Context = Context;

            Context.Player.Context = Context;
            Context.Player.transform.position = starterPoint;
            Context.Player.gameObject.SetActive(false);

            yield return new WaitForSeconds(1f);

            //Spawn player completed
        }

        private void SetPlayerContext()
        {
            Context.Player.Context = Context;
        }

        private void SetLevelOnGameStart()
        {
            var levelDestination = Context.LevelManager.GetLevelByID("Level Main Menu");
            if (levelDestination != null)
                Context.LevelManager.ChangeLevel(levelDestination);
        }
    }
}