using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using echo17.Signaler.Core;
using Demyth.Gameplay;
using Core;
using System;
using Core.UI;
using BrunoMikoski.AnimationSequencer;

namespace Demyth.UI
{
    public class GameHUD : SceneService, ISubscriber
    {
        [SerializeField] private bool _showOnStart;
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
        [SerializeField] private Image shieldImage;

        private GameStateService _gameStateService;
        private GameObject _playerObject;
        private Player _player;
        private HealthPotion _playerHealthPotion;
        private Health _playerHealth;
        private Shield _playerShield;
        private Lantern _playerLantern;
        private IPageAnimator _pageAnimator;

        private float _yPositionAtZeroHealth;
        private float _yPositionAtZeroShield;
        private bool _isOpen;

        private void Awake()
        {
            // Signaler.Instance.Subscribe<PlayerSpawnEvent>(this, OnPlayerSpawned);
            // Signaler.Instance.Subscribe<PlayerDespawnEvent>(this, OnPlayerDespawned);
            
            _gameStateService = SceneServiceProvider.GetService<GameStateService>();
            _player = SceneServiceProvider.GetService<PlayerManager>().Player;
            _playerHealthPotion = _player.GetComponent<HealthPotion>();
            _playerShield = _player.GetComponent<Shield>();
            _playerHealth = _player.GetComponent<Health>();
            _playerLantern = _player.GetComponent<Lantern>();
            _pageAnimator = GetComponent<IPageAnimator>();

            DialogueManager.Instance.conversationStarted += DialogueManager_OnConversationStarted;
            DialogueManager.Instance.conversationEnded += DialogueManager_OnConversationEnded;
            _player.OnLanternValueChanged += Player_OnLanternUnlockedChanged;
            _player.OnHealthPotionUnlockedValueChanged += Player_OnPotionUnlockedChanged;
            _player.OnShieldUnlockedValueChanged += Player_OnShieldUnlockedValueChanged;
            _playerLantern.OnLanternTogglePerformed += Player_OnLanternTogglePerformed;
            _playerHealthPotion.OnPotionAmountChanged += PlayerHealthPotion_OnUsePotion;
            _playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
            _playerShield.OnShieldAmountChanged += PlayerShield_OnShieldAmountChanged;

            GetHealthBarPositionAtZeroShield();
            GetShieldBarPositionAtZeroHealth();
        }

        private void Start()
        {
            if (_showOnStart)
            {
                Open();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void DialogueManager_OnConversationStarted(Transform t)
        {
            Close();
        }

        private void DialogueManager_OnConversationEnded(Transform t)
        {
            if (_gameStateService.CurrentState == GameState.GameOver) return;

            gameObject.SetActive(true);
            Open();
        }

        public void Open()
        {
            if (_isOpen) return;
            _isOpen = true;
            DOTween.CompleteAll();

            _pageAnimator?.PlayAnimation(() =>
            {
                
            });
        }

        public void Close()
        {
            if (!_isOpen) return;
            _isOpen = false;
            DOTween.CompleteAll();

            _pageAnimator?.CloseAnimation(() =>
            {
                
            });
        }

        private void Player_OnLanternUnlockedChanged(bool isUnlocked)
        {
            lanternOffImage.gameObject.SetActive(isUnlocked);
        }

        private void Player_OnPotionUnlockedChanged(bool isUnlocked)
        {
            healthPotionFullImage.gameObject.SetActive(isUnlocked);;
        }

        private void Player_OnShieldUnlockedValueChanged(bool isUnlocked)
        {
            shieldImage.gameObject.SetActive(isUnlocked);
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
