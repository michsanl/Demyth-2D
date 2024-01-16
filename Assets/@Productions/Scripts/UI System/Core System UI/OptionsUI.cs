using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Core;
using Core.UI;
using MoreMountains.Tools;
using PixelCrushers.DialogueSystem;
using TMPro;
using PixelCrushers;

namespace UISystem
{
    public class OptionsUI : MonoBehaviour
    {

        [SerializeField] private Button _changeLanguageButton;
        [SerializeField] private TextMeshProUGUI _selectedLanguageText;
        [Space]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        
        private UIPage _uiPage;

        private void Awake()
        {
            _uiPage = GetComponent<UIPage>();
        }

        private void Start()
        {
            MMSoundManager.Current.LoadSettings();
            
            if (PlayerPrefs.GetString("SelectedLanguage") == "id")
            {
                SetLanguageToIndonesia();
            }
            else
            {
                SetLanguageToDefault();
            }

            _uiPage.OnOpen.AddListener(UpdateSettingsValue);
            _changeLanguageButton.onClick.AddListener(ToggleLanguage);
            _masterVolumeSlider.onValueChanged.AddListener(SetMMSoundMasterVolume);
            _musicVolumeSlider.onValueChanged.AddListener(SetMMSoundMusicVolume);
            _sfxVolumeSlider.onValueChanged.AddListener(SetMMSoundSfxVolume);
        }

        private void ToggleLanguage()
        {
            if (Localization.isDefaultLanguage)
            {
                SetLanguageToIndonesia();
            }
            else
            {
                SetLanguageToDefault();
            }
        }

        private void UpdateSettingsValue()
        {
            if (Localization.isDefaultLanguage)
            {
                _selectedLanguageText.text = "English";
            }
            else
            {
                _selectedLanguageText.text = "Indonesia";
            }

            _masterVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, false);
            _musicVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, false);
            _sfxVolumeSlider.value = MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, false);
        }

        private void SetLanguageToDefault()
        {
            DialogueManager.SetLanguage("");
            PlayerPrefs.SetString("SelectedLanguage", "default");

            _selectedLanguageText.text = "English";

            Debug.Log("set language to default");
        }

        private void SetLanguageToIndonesia()
        {
            DialogueManager.SetLanguage("id");
            PlayerPrefs.SetString("SelectedLanguage", "id");

            _selectedLanguageText.text = "Indonesia";

            Debug.Log("set language to id");
        }

        private void SetMMSoundMasterVolume(float volume)
        {
            MMSoundManager.Current.SetVolumeMaster(volume);
            MMSoundManager.Current.SaveSettings();
        }

        private void SetMMSoundMusicVolume(float volume)
        {
            MMSoundManager.Current.SetVolumeMusic(volume);
            MMSoundManager.Current.SaveSettings();
        }

        private void SetMMSoundSfxVolume(float volume)
        {
            MMSoundManager.Current.SetVolumeSfx(volume);
            MMSoundManager.Current.SaveSettings();
        }
    }
}
