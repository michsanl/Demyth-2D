using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Clip SO/New Sri Clip")]
public class SriClipSO : ScriptableObject
{
    public AudioClip HorizontalSlash;
    public AudioClip VerticalSlash;
    public AudioClip SpinClaw;
    public AudioClip Fireball;
    public AudioClip NailAOE;
    public AudioClip NailSummon;
    public AudioClip GroundNail;
    public AudioClip FlyingNailSummon;
    public AudioClip FlyingNailShot;
    public AudioClip FlyingNailImpact;
    public AudioClip[] Damage;
    
    [Range(0, 1)]
    public float HorizontalSlashVolume = 1f;
    [Range(0, 1)]
    public float VerticalSlashVolume = 1f;
    [Range(0, 1)]
    public float SpinClawVolume = 1f;
    [Range(0, 1)]
    public float FireballVolume = 1f;
    [Range(0, 1)]
    public float NailAOEVolume = 1f;
    [Range(0, 1)]
    public float NailSummonVolume = 1f;
    [Range(0, 1)]
    public float GroundNailVolume = 1f;
    [Range(0, 1)]
    public float FlyingNailSummonVolume = 1f;
    [Range(0, 1)]
    public float FlyingNailShotVolume = 1f;
    [Range(0, 1)]
    public float FlyingNailImpactVolume = 1f;
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
