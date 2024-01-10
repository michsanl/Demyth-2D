using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animation Properties SO/New Animation Properties")]
public class AnimationPropertiesSO : ScriptableObject
{
    public float FrontSwingDuration;
    public float SwingDuration;
    public float BackSwingDuration;

    [Space]

    [Range(0, 5)]
    public float AnimationSpeedMultiplier = 1f;

    public float GetFrontSwingDuration()
    {
        return FrontSwingDuration / AnimationSpeedMultiplier;
    }

    public float GetSwingDuration()
    {
        return SwingDuration / AnimationSpeedMultiplier;
    }

    public float GetBackSwingDuration()
    {
        return BackSwingDuration / AnimationSpeedMultiplier;
    }
}
