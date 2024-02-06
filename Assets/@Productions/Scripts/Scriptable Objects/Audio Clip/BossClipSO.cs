using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossClipSO : ScriptableObject
{
    public abstract AudioClip GetDamageAudioClip(int index);
    public abstract int GetDamageAudioLength();
    public abstract float GetDamageVolume(int index);
}
