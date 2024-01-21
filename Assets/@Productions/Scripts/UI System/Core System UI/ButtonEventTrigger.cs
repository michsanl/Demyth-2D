using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventTrigger : MonoBehaviour
{
    
    [SerializeField] private UIClipSO _uiClipSO;

    public void PlayButtonHighlightSound()
    {
        Helper.PlaySFX(_uiClipSO.ButtonHighlight, _uiClipSO.ButtonHighlightVolume);
    }

    public void PlayButtonClickSound()
    {
        Helper.PlaySFX(_uiClipSO.ButtonClickYes, _uiClipSO.ButtonYesVolume);
    }

    public void PlayButtonClickNoSound()
    {
        Helper.PlaySFX(_uiClipSO.ButtonClickNo, _uiClipSO.ButtonNoVolume);
    }

    public void PlayDialogContinueButtonSound()
    {
        Helper.PlaySFX(_uiClipSO.DialogContinueButton, _uiClipSO.DialogContinueButtonVolume);
    }
}
