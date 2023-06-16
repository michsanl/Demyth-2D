using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using System;

public class CameraShakeController : SceneService
{
    [SerializeField] private float screenShakeDuration;

    private GameObject cameraShakeGO;
    private Player player;

    protected override void OnActivate()
    {
        player = Context.Player;
        cameraShakeGO = Context.VCamCameraShake;
    }

    public IEnumerator PlayCameraShake()
    {
        Time.timeScale = 0;
        cameraShakeGO.SetActive(true);

        yield return new WaitForSecondsRealtime(screenShakeDuration);

        if (!player.IsGamePaused)
        {
            Time.timeScale = 1;
        }
        cameraShakeGO.SetActive(false);
    }
}
