using UnityEngine;

public static class Global
{
    public static GlobalConfig GlobalSettings   { get; private set; }
    public static SceneLoader SceneLoader   { get; private set; }

    private static bool isInitialized;

    #region UNITY ATTRIBUTE METHOD
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InitializeSubSystem()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        Application.quitting -= OnApplicationQuit;
        Application.quitting += OnApplicationQuit;

        isInitialized = true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeSceneLoad()
    {
        Initialize();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeAfterSceneLoad()
    {
        // You can unpause network services here
    }
    #endregion

    private static void Initialize()
    {
        if (!isInitialized) return;

        var globalConfig = Resources.Load<GlobalConfig>(GlobalConfig.GLOBAL_CONFIG_PATH);
        GlobalSettings = Object.Instantiate(globalConfig);

        var sceneLoader = Object.Instantiate(GlobalSettings.SceneLoader);
        Object.DontDestroyOnLoad(sceneLoader);
        SceneLoader = sceneLoader;

        isInitialized = true;
    }

    private static void DeInitialize()
    {
        if (!isInitialized) return;
        isInitialized = false;
    }

    private static void OnApplicationQuit()
    {
        DeInitialize();
    }

    private static T CreateStaticObject<T>() where T : Component
    {
        GameObject gameObject = new GameObject(typeof(T).Name);
        Object.DontDestroyOnLoad(gameObject);

        return gameObject.AddComponent<T>();
    }
}
