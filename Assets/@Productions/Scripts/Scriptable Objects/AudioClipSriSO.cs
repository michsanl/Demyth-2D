using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipSriSO : ScriptableObject
{
    public AudioClip[] Damage;
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

    public AudioClip GetRandomDamageClip()
    {
        return Damage[Random.Range(0, Damage.Length)];
    }
}
