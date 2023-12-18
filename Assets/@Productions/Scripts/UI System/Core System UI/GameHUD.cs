using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using echo17.Signaler.Core;
using Demyth.Gameplay;
using Core;
using System;

namespace Demyth.UI
{
    public class GameHUD : MonoBehaviour, ISubscriber
    {
        [Title("Health/Shield Bar Parameter")]
        [SerializeField] private float _barChangeDuration = 0.5f;
        [SerializeField] private float _barPositionRange = 205f;

        [Title("Components")]
        [SerializeField] private RectTransform healthBarTransform;
        [SerializeField] private RectTransform shieldBarTransform;
        [SerializeField] private Image healthPotionEmptyImage;
        [SerializeField] private Image healthPotionFullImage;
        [SerializeField] private Image lanternOnImage;
        [SerializeField] private Image lanternOffImage;

        private GameObject _playerObject;
        private Player _player;
        private HealthPotion _playerHealthPotion;
        private Health _playerHealth;
        private Shield _playerShield;
        private Lantern _playerLantern;

        private float _yPositionAtZeroHealth;
        private float _yPositionAtZeroShield;

        private void Awake()
        {
            // Signaler.Instance.Subscribe<PlayerSpawnEvent>(this, OnPlayerSpawned);
            // Signaler.Instance.Subscribe<PlayerDespawnEvent>(this, OnPlayerDespawned);
            
            _player = SceneServiceProvider.GetService<PlayerManager>().Player;
            _playerHealthPotion = _player.GetComponent<HealthPotion>();
            _playerShield = _player.GetComponent<Shield>();
            _playerHealth = _player.GetComponent<Health>();
            _playerLantern = _player.GetComponent<Lantern>();

            _player.OnLanternValueChanged += Player_OnLanternUnlockedChanged;
            _player.OnHealthPotionUnlockedValueChanged += Player_OnPotionUnlockedChanged;
            _playerLantern.OnLanternTogglePerformed += Player_OnLanternTogglePerformed;
            _playerHealthPotion.OnPotionAmountChanged += PlayerHealthPotion_OnUsePotion;
            _playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
            _playerShield.OnShieldAmountChanged += PlayerShield_OnShieldAmountChanged;

            GetHealthBarPositionAtZeroShield();
            GetShieldBarPositionAtZeroHealth();
        }

        private void Player_OnLanternUnlockedChanged(bool isUnlocked)
        {
            lanternOffImage.gameObject.SetActive(isUnlocked);
        }

        private void Player_OnPotionUnlockedChanged(bool isUnlocked)
        {
            healthPotionFullImage.gameObject.SetActive(isUnlocked);;
        }

        // this object is not active when the signaler is broadcasting or something
        private bool OnPlayerSpawned(PlayerSpawnEvent signal)
        {
            _playerObject = signal.Player;
            _player = _playerObject.GetComponent<Player>();
            _playerHealthPotion = _playerObject.GetComponent<HealthPotion>();
            _playerShield = _playerObject.GetComponent<Shield>();
            _playerHealth = _playerObject.GetComponent<Health>();
            _playerLantern = _playerObject.GetComponent<Lantern>();

            _playerLantern.OnLanternTogglePerformed += Player_OnLanternTogglePerformed;
            _playerHealthPotion.OnPotionAmountChanged += PlayerHealthPotion_OnUsePotion;
            _playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
            _playerShield.OnShieldAmountChanged += PlayerShield_OnShieldAmountChanged;

            return true;
        }

        private bool OnPlayerDespawned()
        {
            _playerLantern.OnLanternTogglePerformed -= Player_OnLanternTogglePerformed;
            _playerHealthPotion.OnPotionAmountChanged -= PlayerHealthPotion_OnUsePotion;
            _playerHealth.OnHealthChanged -= PlayerHealth_OnHealthChanged;
            _playerShield.OnShieldAmountChanged -= PlayerShield_OnShieldAmountChanged;

            return true;
        }

        private void PlayerHealth_OnHealthChanged()
        {
            UpdateHelathBar();
        }

        private void PlayerShield_OnShieldAmountChanged()
        {
            UpdateShieldBar();
        }

        private void UpdateHelathBar()
        {
            var healthAmountPercentage = _playerHealth.GetHealthPercentage();
            var newHealthPositionY = _yPositionAtZeroHealth + (healthAmountPercentage * _barPositionRange);

            healthBarTransform.DOKill();
            healthBarTransform.DOLocalMoveY(newHealthPositionY, _barChangeDuration).SetEase(Ease.OutExpo);
        }

        private void UpdateShieldBar()
        {
            var shieldAmountPercentage = _playerShield.GetShieldPercentage();
            var newShieldPositionY = _yPositionAtZeroShield + (shieldAmountPercentage * _barPositionRange);

            shieldBarTransform.DOKill();
            shieldBarTransform.DOLocalMoveY(newShieldPositionY, _barChangeDuration).SetEase(Ease.OutExpo);
        }

        private void Player_OnLanternTogglePerformed(bool senterState)
        {
            lanternOnImage.gameObject.SetActive(senterState);
        }

        private void PlayerHealthPotion_OnUsePotion(int healthPotionAmount)
        {
            if (healthPotionAmount == 0)
            {
                healthPotionEmptyImage.gameObject.SetActive(true);
            }
            else
            {
                healthPotionEmptyImage.gameObject.SetActive(false);
            }
        }

        private void GetHealthBarPositionAtZeroShield()
        {
            _yPositionAtZeroHealth = healthBarTransform.localPosition.y - _barPositionRange;
        }

        private void GetShieldBarPositionAtZeroHealth()
        {
            _yPositionAtZeroShield = shieldBarTransform.localPosition.y - _barPositionRange;
        }
    }
}
