using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using System;

public class CameraShakeController : SceneService
{
    [SerializeField] private float screenShakeDuration;
    [SerializeField] private GameObject cameraShakeGO;

    public Action OnCameraShakeStart;
    public Action OnCameraShakeEnd;

    private Player player;

    protected override void OnActivate()
    {
        player = Context.Player;
    }

    public IEnumerator PlayCameraShake()
    {
        OnCameraShakeStart?.Invoke();

        Time.timeScale = 0;
        cameraShakeGO.SetActive(true);

        yield return new WaitForSecondsRealtime(screenShakeDuration);

        if (!Context.GameManager.IsGamePaused)
            Time.timeScale = 1;

        cameraShakeGO.SetActive(false);

        OnCameraShakeEnd?.Invoke();
    }
}
