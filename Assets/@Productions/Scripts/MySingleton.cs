using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

// script nyolong 
// biar kalo mau buat class Singleton/Peristance tinggal inherit 

public abstract class MyStaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

public abstract class MySingleton<T> : MyStaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance !=null) Destroy(gameObject);
        base.Awake();
    }
}

public abstract class MyPersistanceSingleton<T> : MySingleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
