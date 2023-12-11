using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using UISystem;
using UnityEngine;

namespace Demyth.UI
{
    public class LevelResetUI : MonoBehaviour
    {

        
        [SerializeField] private BoxPuzzleLevelReset _boxPuzzleLevelReset;
        [SerializeField] private TuyulChaseLevelReset _tuyulChaseLevelReset;

        private void Awake()
        {
            _boxPuzzleLevelReset.OnBoxPuzzleLevelResetEnabled += BoxLevelReset_OnBoxLevelResetEnabled;
            _boxPuzzleLevelReset.OnBoxPuzzleLevelResetDisabled += BoxLevelReset_OnBoxLevelResetDisabled;
            _tuyulChaseLevelReset.OnTuyulLevelResetEnabled += TuyulLevelResetEnabled_OnTuyulLevelResetEnabled;
            _tuyulChaseLevelReset.OnTuyulLevelResetDisabled += TuyulLevelResetEnabled_OnTuyulLevelResetDisabled;
        }

        private void Start() 
        {
            Hide();
        }

        private void OnDestroy()
        {
            _boxPuzzleLevelReset.OnBoxPuzzleLevelResetEnabled -= BoxLevelReset_OnBoxLevelResetEnabled;
            _boxPuzzleLevelReset.OnBoxPuzzleLevelResetDisabled -= BoxLevelReset_OnBoxLevelResetDisabled;
            _tuyulChaseLevelReset.OnTuyulLevelResetEnabled -= TuyulLevelResetEnabled_OnTuyulLevelResetEnabled;
            _tuyulChaseLevelReset.OnTuyulLevelResetDisabled -= TuyulLevelResetEnabled_OnTuyulLevelResetDisabled;
        }

        private void BoxLevelReset_OnBoxLevelResetEnabled()
        {
            Show();
        }

        private void BoxLevelReset_OnBoxLevelResetDisabled()
        {
            Hide();
        }

        private void TuyulLevelResetEnabled_OnTuyulLevelResetEnabled()
        {
            Show();
        }

        private void TuyulLevelResetEnabled_OnTuyulLevelResetDisabled()
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
