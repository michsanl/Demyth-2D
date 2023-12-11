using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipAraSO : ScriptableObject
{
    public AudioClip Move;
    public AudioClip Potion;
    public AudioClip Lantern;
    public AudioClip[] MoveBox;
    public AudioClip[] PanHit;

    public AudioClip GetRandomMoveBoxClip()
    {
        return MoveBox[Random.Range(0, MoveBox.Length)];
    }

    public AudioClip GetRandomPanHitClip()
    {
        return PanHit[Random.Range(0, PanHit.Length)];
    }
}
