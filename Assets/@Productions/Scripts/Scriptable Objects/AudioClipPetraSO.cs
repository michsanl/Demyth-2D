using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipPetraSO : ScriptableObject
{
    public AudioClip[] Damage;
    public AudioClip RunCharge;
    public AudioClip JumpSlam;
    public AudioClip CoffinSwing;
    public AudioClip ChargeSlam;
    public AudioClip BasicSlam;
    public AudioClip GroundCoffinEmerge;

    public AudioClip GetRandomDamageClip()
    {
        return Damage[Random.Range(0, Damage.Length)];
    }
}
