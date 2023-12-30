using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using Core;

public class MusicController : SceneService
{
    [SerializeField] private AudioClip _levelBGM;
    [SerializeField] private AudioClip _petraFightIntroBGM;
    [SerializeField] private AudioClip _petraFightLoopBGM;
    [SerializeField] private AudioClip _sriCutsceneBGM;
    [SerializeField] private AudioClip _sriIntroBGM;
    [SerializeField] private AudioClip _sriLoopBGM;
    [SerializeField] private AudioClip _epilogueVer1BGM;

    public void FadeOutLevelBGM(float fadeDuration)
    {
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 1, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));
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

    private IEnumerator StartPetraBossFightMusicCoroutine()
    {
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        Helper.PlayMusic(_petraFightIntroBGM, .25f, 2, false);

        yield return MMCoroutine.WaitForUnscaled(26f);

        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        Helper.PlayMusic(_petraFightLoopBGM, .25f, 3, true);
    }

    private IEnumerator EndPetraBossFightMusicCoroutine()
    {
        float fadeDuration = 10f;
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 3, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 2, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));
    
        yield return MMCoroutine.WaitFor(fadeDuration);

        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        Helper.PlayMusic(_levelBGM, .2f, 1, true);
    }

    private IEnumerator StartSriCutsceneMusicCoroutine()
    {
        float fadeDuration = 5f;
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 1, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));

        yield return MMCoroutine.WaitForUnscaled(5f);

        Helper.PlayMusic(_sriCutsceneBGM, .25f, 4, false);
    }

    private IEnumerator StartSriBossFightMusicCoroutine()
    {
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        Helper.PlayMusic(_sriIntroBGM, .25f, 5, false);

        yield return MMCoroutine.WaitForUnscaled(21f);

        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        Helper.PlayMusic(_sriLoopBGM, .25f, 6, true);
    }

    private IEnumerator EndSriBossFightMusicCoroutine()
    {
        float fadeDuration = 10f;
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 5, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.PlayFade, 6, fadeDuration, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));
    
        yield return MMCoroutine.WaitFor(fadeDuration);

        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
        Helper.PlayMusic(_epilogueVer1BGM, .25f, 1, true);
    }
}
