using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UISystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MyPersistenceSingleton<SceneLoadManager>
{
    [SerializeField] private UIClipSO _uiClipSO;
    [SerializeField] private Animator _loadingAnimator;
    [Space]
    [SerializeField] private float _openPageDuration;
    [SerializeField] private float _closePageDuration;

    private bool _isLoadInProgress;

    public void LoadScene(int sceneIndex)
    {
        // Ada progress yang sedang berjalan
        if (_isLoadInProgress) return;

        StartCoroutine(LoadSceneCoroutine(sceneIndex));
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
        yield return StartCoroutine(ShowLoadingScreen(true));

        // Load new scene
        DOTween.CompleteAll();
        loadAsync.allowSceneActivation = true;
        yield return null;
        yield return null;

        // Loading HIDE
        yield return StartCoroutine(ShowLoadingScreen(false));

        _isLoadInProgress = false;
    }

    private IEnumerator ShowLoadingScreen(bool isShow)
    {
        if (isShow)
        {
            SetPageVisibility(true);
            _loadingAnimator.SetTrigger("OpenPage");
            Helper.PlaySFXIgnorePausePersistent(_uiClipSO.HUDOpen, _uiClipSO.HUDOpenVolume);
            yield return new WaitForSeconds(_openPageDuration);
        }
        else
        {
            _loadingAnimator.SetTrigger("ClosePage");
            Helper.PlaySFXIgnorePausePersistent(_uiClipSO.HUDClose, _uiClipSO.HUDCloseVolume);
            yield return new WaitForSeconds(_closePageDuration);
            SetPageVisibility(false);
        }
    }

    private void SetPageVisibility(bool condition)
    {
        _loadingAnimator.gameObject.SetActive(condition);
    }
    
    
}
