using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class PlayerManager : SceneService
{
    [SerializeField]
    private Player playerPrefab;

    private Player _spawnedPlayer;

    private void Start()
    {
        
    }
}
