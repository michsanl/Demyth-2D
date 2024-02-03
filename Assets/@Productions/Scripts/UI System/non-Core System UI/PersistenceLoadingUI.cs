using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceLoadingUI : MyPersistenceSingleton<PersistenceLoadingUI>
{
    
    [SerializeField] private UIClipSO _uiClipSO;
    [SerializeField] private Animator _loadingAnimator;
    [Space]
    [SerializeField] private float _openPageDuration = 0.9f;
    [SerializeField] private float _closePageDuration = 0.9f;

    public IEnumerator OpenLoadingPage()
    {
        SetPageVisibility(true);
        _loadingAnimator.SetTrigger("OpenPage");
        Helper.PlaySFXIgnorePausePersistent(_uiClipSO.HUDOpen, _uiClipSO.HUDOpenVolume);
        yield return new WaitForSeconds(_openPageDuration);
    }

    public IEnumerator CloseLoadingPage()
    {
        _loadingAnimator.SetTrigger("ClosePage");
        Helper.PlaySFXIgnorePausePersistent(_uiClipSO.HUDClose, _uiClipSO.HUDCloseVolume);
        yield return new WaitForSeconds(_closePageDuration);
        SetPageVisibility(false);
    }

    private void SetPageVisibility(bool condition)
    {
        _loadingAnimator.gameObject.SetActive(condition);
    }
}
