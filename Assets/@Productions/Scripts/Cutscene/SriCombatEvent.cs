using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Core;
using Demyth.Gameplay;

public class SriCombatEvent : MonoBehaviour
{
    [SerializeField] private float _lightsOffIntensity;
    [SerializeField] private float _globalLightsOffDelay;
    [SerializeField] private float _lightsOffInterval;
    [Space]
    [SerializeField] private SriCombatBehaviour _sriCombatBehaviour;
    [SerializeField] private Light2D _light2D;
    [SerializeField] private PillarLight[] _pillarLightArray;

    private GameStateService _gameStateService;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
    }

    private void Start()
    {
        _gameStateService[GameState.MainMenu].onEnter += MainMenu_OnEnter;
        _gameStateService[GameState.GameOver].onEnter += GameOver_OnEnter;

        _sriCombatBehaviour.OnPhaseTwoStart += SriCombatBehaviour_OnPhaseTwoStart;
    }

    private void SriCombatBehaviour_OnPhaseTwoStart()
    {
        StartCoroutine(StartTurnOffLightCoroutine());
    }

    private void MainMenu_OnEnter(GameState state)
    {
        StopAllCoroutines();
        ResetLight();
    }

    private void GameOver_OnEnter(GameState state)
    {
        StopAllCoroutines();
        ResetLight();
    }

    private void ResetLight()
    {
        _light2D.intensity = 1f;
        foreach (var pillarLight in _pillarLightArray)
        {
            pillarLight.TurnOnPillarLight();
        }
    }

    private IEnumerator StartTurnOffLightCoroutine()
    {
        yield return Helper.GetWaitForSeconds(_globalLightsOffDelay);

        _light2D.intensity = _lightsOffIntensity;

        foreach (var pillarLight in _pillarLightArray)
        {
            yield return Helper.GetWaitForSeconds(_lightsOffInterval);
            pillarLight.TurnOffPillarLight();
        }
    }
}
