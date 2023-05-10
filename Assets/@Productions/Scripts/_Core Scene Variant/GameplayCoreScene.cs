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
            
            yield return SpawnPlayer();
            // TODO : Prepare Level            
        }

        private IEnumerator SpawnPlayer()
        {
            var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            //TODO : initiate player
            Context.Player = player;
            player.Context = Context;

            yield return new WaitForSeconds(1f);

            //Spawn player completed
        }
    }
}