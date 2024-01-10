using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Clip SO/New Petra Clip")]
public class PetraClipSO : ScriptableObject
{
    public AudioClip RunCharge;
    public AudioClip JumpSlam;
    public AudioClip CoffinSwing;
    public AudioClip ChargeSlam;
    public AudioClip BasicSlam;
    public AudioClip GroundCoffinEmerge;
    public AudioClip[] Damage;
    
    [Range(0, 1)]
    public float RunChargeVolume = 1f;
    [Range(0, 1)]
    public float JumpSlamVolume = 1f;
    [Range(0, 1)]
    public float CoffinSwingVolume = 1f;
    [Range(0, 1)]
    public float ChargeSlamVolume = 1f;
    [Range(0, 1)]
    public float BasicSlamVolume = 1f;
    [Range(0, 1)]
    public float GroundCoffinEmergeVolume = 1f;
    [Range(0, 1)]
    public float Damage1Volume = 1f;
    [Range(0, 1)]
    public float Damage2Volume = 1f;
    [Range(0, 1)]
    public float Damage3Volume = 1f;

    public float GetDamageVolume(int index)
    {
        switch (index)
        {
            case 0:
                return Damage1Volume;
            case 1:
                return Damage2Volume;
            case 2:
                return Damage3Volume;
            default:
                return 1f;
        }
    }
    
    
}
