using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Clip SO/New Petra Clip")]
public class PetraClipSO : BossClipSO
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

    public override AudioClip GetDamageAudioClip(int index)
    {
        return Damage[index];
    }

    public override int GetDamageAudioLength()
    {
        return Damage.Length;
    }

    public override float GetDamageVolume(int index)
    {
        return index switch
        {
            0 => Damage1Volume,
            1 => Damage2Volume,
            2 => Damage3Volume,
            _ => 1f,
        };
    }
}
