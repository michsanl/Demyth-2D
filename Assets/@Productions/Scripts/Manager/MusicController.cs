using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using Core;
using Demyth.Gameplay;
using System;
using PixelCrushers.DialogueSystem;

public class MusicController : SceneService
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private MusicClipSO _musicClipSO;
    [SerializeField] private UIClipSO _uiClipSO;
    
    private GameStateService _gameStateService;
    private Coroutine _fadeCoroutine;
    private float _currentMusicDefaultVolume;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _gameStateService[GameState.MainMenu].onEnter += MainMenu_OnEnter;
        _gameStateService[GameState.Pause].onEnter += Pause_OnEnter;
        _gameStateService[GameState.Pause].onExit += Pause_OnExit;
        _musicSource.ignoreListenerPause = true;
    }

    private void MainMenu_OnEnter(GameState state)
    {
        StopAllCoroutines();
    }

    private void Pause_OnEnter(GameState state)
    {
    }

    private void Pause_OnExit(GameState state)
    {
    }

    public void PlayLevelBGM()
    {
        if (DialogueLua.GetVariable("Level_7_Done").asBool)
        {
            PlayMusic(_musicClipSO.EpilogueVer1BGM, _musicClipSO.EpilogueVer1BGMVolume, true);
        }
        else
        {
            PlayMusic(_musicClipSO.LevelBGM, _musicClipSO.LevelBGMVolume, true);
        }
    }

    public void PlayMainMenuBGM()
    {
        PlayMusic(_musicClipSO.MainMenuBGM, _musicClipSO.MainMenuBGMVolume, true);
    }
    
    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void FadeInCurrentMusic(float duration)
    {
        StopFade();
        _fadeCoroutine = StartCoroutine(StartFadeCoroutine(_musicSource, duration, 0, _currentMusicDefaultVolume));
    }

    public void FadeOutCurrentMusic(float duration)
    {
        StopFade();
        _fadeCoroutine = StartCoroutine(StartFadeCoroutine(_musicSource, duration, _musicSource.volume, 0f));
    }

    public void StopFade()
    {
        if (_fadeCoroutine == null) return;
        StopCoroutine(_fadeCoroutine);
    }

    public void StartPetraBossFightMusic()
    {
        StartCoroutine(StartPetraBossFightMusicCoroutine());
    }

    public void EndPetraBossFightMusic()
    {
        StartCoroutine(EndPetraBossFightMusicCoroutine());
    }

    public void StartSriCutsceneMusic()
    {
        StartCoroutine(StartSriCutsceneMusicCoroutine());
    }

    public void StartSriBossFightMusic()
    {
        StartCoroutine(StartSriBossFightMusicCoroutine());
    }

    public void EndSriBossFightMusic()
    {
        StartCoroutine(EndSriBossFightMusicCoroutine());
    }

    public void PlayPaperDrawSound()
    {
        Helper.PlaySFX(_uiClipSO.PaperDraw, _uiClipSO.PaperDrawVolume);
    }

    private IEnumerator StartPetraBossFightMusicCoroutine()
    {
        float introMusicLength = _musicClipSO.PetraFightIntroBGM.length;

        PlayMusic(_musicClipSO.PetraFightIntroBGM, _musicClipSO.PetraFightIntroBGMVolume, false);
        yield return new WaitUntil(() => _musicSource.time >= introMusicLength);
        PlayMusic(_musicClipSO.PetraFightLoopBGM, _musicClipSO.PetraFightLoopBGMVolume, true);
    }

    private IEnumerator StartSriBossFightMusicCoroutine()
    {
        float introMusicLength = _musicClipSO.SriIntroBGM.length;

        PlayMusic(_musicClipSO.SriIntroBGM, _musicClipSO.SriIntroBGMVolume, false);
        yield return new WaitUntil(() => _musicSource.time >= introMusicLength);
        PlayMusic(_musicClipSO.SriLoopBGM, _musicClipSO.SriLoopBGMVolume, true);
    }

    private IEnumerator EndPetraBossFightMusicCoroutine()
    {
        float fadeDuration = 10f;

        FadeOutCurrentMusic(fadeDuration);
        yield return Helper.GetWaitForSeconds(fadeDuration);
        PlayMusic(_musicClipSO.LevelBGM, _musicClipSO.LevelBGMVolume, true);
    }

    private IEnumerator EndSriBossFightMusicCoroutine()
    {
        float fadeDuration = 10f;

        FadeOutCurrentMusic(fadeDuration);
        yield return Helper.GetWaitForSeconds(fadeDuration);
        PlayMusic(_musicClipSO.EpilogueVer1BGM, _musicClipSO.EpilogueVer1BGMVolume, true);
    }

    private IEnumerator StartSriCutsceneMusicCoroutine()
    {
        if (_musicSource.clip == _musicClipSO.SriCutsceneBGM) yield break;

        float fadeDuration = 5f;

        FadeOutCurrentMusic(fadeDuration);
        yield return Helper.GetWaitForSeconds(fadeDuration);
        PlayMusic(_musicClipSO.SriCutsceneBGM, _musicClipSO.SriCutsceneBGMVolume, true);
    }

    private void PlayMusic(AudioClip clip, float volume, bool loop)
    {
        StopFade();

        _currentMusicDefaultVolume = volume;

        _musicSource.clip = clip;
        _musicSource.volume = volume;
        _musicSource.loop = loop;
        _musicSource.Play();
    }

    private IEnumerator StartFadeCoroutine(AudioSource audioSource, float duration, float startVolume, float targetVolume)
    {
        float currentTime = 0;
        float start = startVolume;
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    private IEnumerator StartFadeListenerCoroutine(float duration, float startVolume, float targetVolume)
    {
        float currentTime = 0;
        float start = startVolume;
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            AudioListener.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
