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
        [SerializeField] private Player playerPrefab;

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override IEnumerator OnActivate()
        {
            yield return base.OnActivate();

            SetPlayerContext();
            SpawnPlayerOnLevelStartingPosition();
        }

        private void SpawnPlayerOnLevelStartingPosition()
        {
            if (Context.LevelManager == null)
                return;

            var starterPoint = Context.LevelManager.CurrentLevel.StarterPosition;
            Context.Player.transform.position = starterPoint;
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

    }

    
}