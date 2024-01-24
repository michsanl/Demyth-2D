using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventTrigger : MonoBehaviour
{
    
    [SerializeField] private UIClipSO _uiClipSO;

    public void PlayButtonHighlightSound()
    {
        Helper.PlaySFXIgnorePause(_uiClipSO.ButtonHighlight, _uiClipSO.ButtonHighlightVolume);
    }

    public void PlayButtonClickSound()
    {
        Helper.PlaySFXIgnorePause(_uiClipSO.ButtonClickYes, _uiClipSO.ButtonYesVolume);
    }

    public void PlayButtonClickNoSound()
    {
        Helper.PlaySFXIgnorePause(_uiClipSO.ButtonClickNo, _uiClipSO.ButtonNoVolume);
    }

    public void PlayDialogContinueButtonSound()
    {
        Helper.PlaySFXIgnorePause(_uiClipSO.DialogContinueButton, _uiClipSO.DialogContinueButtonVolume);
    }
}
