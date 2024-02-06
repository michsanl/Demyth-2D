using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Clip SO/New Music Clip")]
public class MusicClipSO : ScriptableObject
{
    
    public AudioClip LevelBGM;
    public AudioClip MainMenuBGM;
    public AudioClip PetraFightIntroBGM;
    public AudioClip PetraFightLoopBGM;
    public AudioClip SriCutsceneBGM;
    public AudioClip SriIntroBGM;
    public AudioClip SriLoopBGM;
    public AudioClip EpilogueVer1BGM;
    
    [Range(0, 1)]
    public float LevelBGMVolume = 1f;
    
    [Range(0, 1)]
    public float MainMenuBGMVolume = 1f;
    [Range(0, 1)]
    public float PetraFightIntroBGMVolume = 1f;
    [Range(0, 1)]
    public float PetraFightLoopBGMVolume = 1f;
    [Range(0, 1)]
    public float SriCutsceneBGMVolume = 1f;
    [Range(0, 1)]
    public float SriIntroBGMVolume = 1f;
    [Range(0, 1)]
    public float SriLoopBGMVolume = 1f;
    [Range(0, 1)]
    public float EpilogueVer1BGMVolume = 1f;
}
