using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Global Config", menuName = "Core Scene/Global Config")]
public class GlobalConfig : ScriptableObject
{
    public const string GLOBAL_CONFIG_PATH = "Global/Global Config";

    [Header("Scene Loader")]
    public SceneLoader SceneLoader;

    [Header("Scenes")]
    public SceneReferenceGlobal GameplayScene;
    public SceneReferenceGlobal EmptyScene;
}