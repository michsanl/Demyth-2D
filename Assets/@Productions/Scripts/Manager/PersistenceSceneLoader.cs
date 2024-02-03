using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UISystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceSceneLoader : MyPersistenceSingleton<PersistenceSceneLoader>
{
    private bool _isLoadInProgress;

    public void LoadScene(int targetSceneIndex)
    {
        if (_isLoadInProgress)
        {
            Debug.LogWarning("Trying to load scene while load scene is in progress, LoadScene aborted");
        }

        StartCoroutine(LoadSceneCoroutine(targetSceneIndex));
    }
    
    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        _isLoadInProgress = true;

        if (SceneManager.GetSceneByBuildIndex(sceneIndex) == null)
        {
            Debug.LogError("No Scene On Build");
            yield break;
        }

        var loadAsync = SceneManager.LoadSceneAsync(0);
        loadAsync.allowSceneActivation = false;

        // Loading SHOW
        yield return StartCoroutine(PersistenceLoadingUI.Instance.OpenLoadingPage());

        // Load new scene
        DOTween.CompleteAll();
        loadAsync.allowSceneActivation = true;
        yield return null;
        yield return null;

        // Loading HIDE
        yield return StartCoroutine(PersistenceLoadingUI.Instance.CloseLoadingPage());

        _isLoadInProgress = false;
    }
    
    
}
