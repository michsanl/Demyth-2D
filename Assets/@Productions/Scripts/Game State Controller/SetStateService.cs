using System.Collections;
using System.Collections.Generic;
using Core;
using Demyth.Gameplay;
using UnityEngine;

public class SetStateService : SceneService
{
    [SerializeField]
    private GameState targetState;

    private GameStateService _gameStateService;

    private void Awake()
    {
        _gameStateService = GetComponentInParent<GameStateService>();
    }

    public override IEnumerator StartService()
    {
        _gameStateService.SetState(targetState);
        yield return null;
    }
}
