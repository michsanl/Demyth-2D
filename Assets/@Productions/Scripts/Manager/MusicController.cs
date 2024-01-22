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
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private MusicClipSO _musicClipSO;
    [SerializeField] private UIClipSO _uiClipSO;
    
    private GameStateService _gameStateService;
    private Coroutine _fadeCoroutine;
    private float _currentMusicDefaultVolume;

    private void Awake()
    {
        _gameStateService = SceneServiceProvider.GetService<GameStateService>();
        _gameStateService[GameState.MainMenu].onEnter += MainMenu_OnEnter;
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
        _musicAudioSource.Stop();
    }

    public void FadeInCurrentMusic(float duration)
    {
        if (_fadeCoroutine !=null) StopFade();
        _fadeCoroutine = StartCoroutine(StartFadeCoroutine(_musicAudioSource, duration, 0, _currentMusicDefaultVolume));
    }

    public void FadeOutCurrentMusic(float duration)
    {
        if (_fadeCoroutine !=null) StopFade();
        _fadeCoroutine = StartCoroutine(StartFadeCoroutine(_musicAudioSource, duration, _musicAudioSource.volume, 0f));
    }

    public void StopFade()
    {
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
        // MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        // PlayMusic(_petraFightIntroBGM, .25f, 2, false);

        // yield return MMCoroutine.WaitForUnscaled(26f);

        // MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        // PlayMusic(_petraFightLoopBGM, .25f, 3, true);

        PlayMusic(_musicClipSO.PetraFightIntroBGM, _musicClipSO.PetraFightIntroBGMVolume, false);
        yield return MMCoroutine.WaitForUnscaled(26f);
        PlayMusic(_musicClipSO.PetraFightLoopBGM, _musicClipSO.PetraFightLoopBGMVolume, true);
    }

    private IEnumerator EndPetraBossFightMusicCoroutine()
    {
        float fadeDuration = 10f;
        // MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 3, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));
        // MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 2, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));
    
        // yield return MMCoroutine.WaitFor(fadeDuration);

        // MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        // MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.FreeTrack, MMSoundManager.MMSoundManagerTracks.Music);
        // PlayMusic(_levelBGM, .2f, 1, true);

        FadeOutCurrentMusic(fadeDuration);
        yield return MMCoroutine.WaitFor(fadeDuration);
        PlayMusic(_musicClipSO.LevelBGM, _musicClipSO.LevelBGMVolume, true);
    }

    private IEnumerator StartSriCutsceneMusicCoroutine()
    {
        float fadeDuration = 5f;
        // MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 1, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));

        // yield return MMCoroutine.WaitForUnscaled(5f);

        // MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        // PlayMusic(_sriCutsceneBGM, .25f, 4, false);

        FadeOutCurrentMusic(fadeDuration);
        yield return MMCoroutine.WaitForUnscaled(fadeDuration);
        PlayMusic(_musicClipSO.SriCutsceneBGM, _musicClipSO.SriCutsceneBGMVolume, true);
    }

    private IEnumerator StartSriBossFightMusicCoroutine()
    {
        // MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        // PlayMusic(_sriIntroBGM, .25f, 5, false);

        // yield return MMCoroutine.WaitForUnscaled(21f);

        // MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        // PlayMusic(_sriLoopBGM, .25f, 6, true);

        PlayMusic(_musicClipSO.SriIntroBGM, _musicClipSO.SriIntroBGMVolume, false);
        yield return MMCoroutine.WaitForUnscaled(21f);
        PlayMusic(_musicClipSO.SriLoopBGM, _musicClipSO.SriLoopBGMVolume, true);
    }

    private IEnumerator EndSriBossFightMusicCoroutine()
    {
        float fadeDuration = 10f;
        // MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 5, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));
        // MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 6, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));

        // yield return MMCoroutine.WaitForUnscaled(fadeDuration);

        // MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        // PlayMusic(_epilogueVer1BGM, .25f, 1, true);

        FadeOutCurrentMusic(fadeDuration);
        yield return MMCoroutine.WaitForUnscaled(fadeDuration);
        PlayMusic(_musicClipSO.EpilogueVer1BGM, _musicClipSO.EpilogueVer1BGMVolume, true);
    }

    private void PlayMusic(AudioClip clip, float volume, bool loop)
    {
        if (_fadeCoroutine !=null) StopFade();

        _currentMusicDefaultVolume = volume;

        _musicAudioSource.clip = clip;
        _musicAudioSource.volume = volume;
        _musicAudioSource.loop = loop;
        _musicAudioSource.Play();
    }

    private void PlayMusicMM(AudioClip audioClip, float volume, int id, bool loop)
    {
        MMSoundManagerPlayOptions playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.Volume = volume;
        playOptions.ID = id;
        playOptions.Loop = loop;
        playOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Music;

        MMSoundManagerSoundPlayEvent.Trigger(audioClip, playOptions);
    }

    private IEnumerator StartFadeCoroutine(AudioSource audioSource, float duration, float startVolume, float targetVolume)
    {
        float currentTime = 0;
        float start = startVolume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    private void MainMenu_OnEnter(GameState state)
    {
        StopAllCoroutines();
    }
}
