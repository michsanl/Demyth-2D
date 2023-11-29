using CustomExtensions;
using CustomTools.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private LoadingScreen loadingScreen;

    private bool _isLoadInProgress;

    private SceneContext _context;

    public void LoadScene(SceneReferenceGlobal sceneReference)
    {
        // Ada progress yang sedang berjalan
        if (_isLoadInProgress)
        {
            return;
        }

        StartCoroutine(LoadSceneCoroutine(sceneReference));
    }

    private IEnumerator LoadSceneCoroutine(SceneReferenceGlobal sceneReference)
    {
        _isLoadInProgress = true;

        if (SceneManager.GetSceneByPath(sceneReference) == null)
        {
            Debug.LogError("No Scene On Build");
            yield break;
        }

        var emptySceneReference = Global.GlobalSettings.EmptyScene;
        yield return SceneManager.LoadSceneAsync(emptySceneReference, LoadSceneMode.Additive);

        yield return ShowLoadingScreen(true);
        
        //Unload current scene
        var activeScene = SceneManager.GetActiveScene();
        var prevCoreScene = activeScene.GetComponent<CoreScene>();
        
        float timeOut = 2f;
        while (prevCoreScene == null && timeOut > 0f)
        {
            yield return null;
            prevCoreScene = activeScene.GetComponent<CoreScene>();

            timeOut -= Time.deltaTime;
        }

        if (prevCoreScene != null)
        {
            // TODO : Inject to be use on another scene
            prevCoreScene.Deinitialize();
        }

        yield return SceneManager.UnloadSceneAsync(activeScene);

        //Load new scene        
        var sceneActivation = SceneManager.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);
        yield return sceneActivation;

        var newScene = SceneManager.GetSceneByPath(sceneReference);
        while (newScene.IsValid() == false)
        {
            yield return null;
        }

        var newCoreScene = newScene.GetComponent<CoreScene>();
        
        timeOut = 2f;
        while (newCoreScene == null && timeOut > 0f)
        {
            yield return null;
            newCoreScene = newScene.GetComponent<CoreScene>();

            timeOut -= Time.deltaTime;
        }

        if (newCoreScene != null)
        {
            // TODO : Inject new core scene context
        }

        // All progress is completed
        // Activate scene
        SceneManager.SetActiveScene(newScene);

        yield return null;

        var emptyScene = SceneManager.GetSceneByPath(emptySceneReference);
        if (emptyScene.IsValid())
            yield return SceneManager.UnloadSceneAsync(emptySceneReference);

        yield return ShowLoadingScreen(false);
        _isLoadInProgress = false;
    }

    private IEnumerator ShowLoadingScreen(bool isShow)
    {
        if (!loadingScreen.IsPlaying)
        {
            if (isShow)
            {
                loadingScreen.ShowLoading(true);
                loadingScreen.Open();
            }
            else
                loadingScreen.Close();
        }

        yield return new WaitUntil(() => loadingScreen.IsPlaying == false);

        loadingScreen.ShowLoading(isShow);
    }
}
