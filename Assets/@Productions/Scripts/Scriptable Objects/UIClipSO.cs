using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Clip SO/New UI Clip")]
public class UIClipSO : ScriptableObject
{
    public AudioClip HUDOpen;
    public AudioClip HUDClose;
    public AudioClip ButtonHighlight;
    public AudioClip ButtonClickYes;
    public AudioClip ButtonClickNo;
    public AudioClip DialogContinueButton;
    public AudioClip PaperDraw;
    public AudioClip GameOver;
    
    [Range(0, 1)]
    public float HUDOpenVolume = 1f;
    [Range(0, 1)]
    public float HUDCloseVolume = 1f;
    [Range(0, 1)]
    public float ButtonHighlightVolume = 1f;
    [Range(0, 1)]
    public float ButtonYesVolume = 1f;
    [Range(0, 1)]
    public float ButtonNoVolume = 1f;
    [Range(0, 1)]
    public float DialogContinueButtonVolume = 1f;
    [Range(0, 1)]
    public float PaperDrawVolume = 1f;
    [Range(0, 1)]
    public float GameOverVolume = 1f;
}
