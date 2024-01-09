using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Clip SO/New Ara Clip")]
public class AraClipSO : ScriptableObject
{
    public AudioClip Move;
    public AudioClip Potion;
    public AudioClip Lantern;
    public AudioClip[] MoveBox;
    public AudioClip[] PanHit;
    
    [Range(0, 1)]
    public float MoveVolume = 1f;
    [Range(0, 1)]
    public float PotionVolume = 1f;
    [Range(0, 1)]
    public float LanternVolume = 1f;
    [Range(0, 1)]
    public float MoveBox1Volume = 1f;
    [Range(0, 1)]
    public float MoveBox2Volume = 1f;
    [Range(0, 1)]
    public float MoveBox3Volume = 1f;
    [Range(0, 1)]
    public float PanHit1Volume = 1f;
    [Range(0, 1)]
    public float PanHit2Volume = 1f;
    [Range(0, 1)]
    public float PanHit3Volume = 1f;

    public float GetMoveBoxVolume(int index)
    {
        switch (index)
        {
            case 0:
                return MoveBox1Volume;
            case 1:
                return MoveBox2Volume;
            case 2:
                return MoveBox3Volume;
            default:
                return 1f;
        }
    }
}
