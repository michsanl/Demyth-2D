using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using echo17.Signaler.Core;
using Demyth.Gameplay;

namespace Demyth.UI
{
    public class GameHUD : MonoBehaviour, ISubscriber
    {
        [Title("Health/Shield Bar Parameter")]
        [SerializeField] private float barChangeDuration;
        [SerializeField] private float barPositionRange = 219;

        [Title("Components")]
        [SerializeField] private Transform healthBarTransform;
        [SerializeField] private Transform shieldBarTransform;
        [SerializeField] private Image healthPotionEmptyImage;
        [SerializeField] private Image senterLightOnImage;

        private HealthPotion playerHealthPotion;
        private Health playerHealth;
        private Shield playerShield;

        private GameObject _playerObject;

        private float healthPositionX;
        private float minimumHealthPositionY;
        private float shieldPositionX;
        private float minimumShieldPositionY;

        private void Awake()
        {
            Signaler.Instance.Subscribe<PlayerSpawnEvent>(this, OnPlayerSpawned);
            Signaler.Instance.Subscribe<PlayerDespawnEvent>(this, OnPlayerDespawned);
            GetHealthBarPosition();
            GetShieldBarPosition();
        }

        private void Start()
        {
            //DialogueManager.instance.conversationStarted += DialogueManager_ConversationStarted;
            //DialogueManager.instance.conversationEnded += DialogueManager_ConversationEnded;
        }

        private void OnDestroy()
        {
            if (DialogueManager.instance != null)
            {
                //DialogueManager.instance.conversationStarted -= DialogueManager_ConversationStarted;
                //DialogueManager.instance.conversationEnded -= DialogueManager_ConversationEnded;
            }
        }

        private bool OnPlayerSpawned(PlayerSpawnEvent signal)
        {
            _playerObject = signal.Player;
            playerHealthPotion = _playerObject.GetComponent<HealthPotion>();
            playerShield = _playerObject.GetComponent<Shield>();
            playerHealth = _playerObject.GetComponent<Health>();

            playerHealthPotion.OnPotionAmountChanged += PlayerHealthPotion_OnUsePotion;
            playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
            playerShield.OnShieldAmountChanged += PlayerShield_OnShieldAmountChanged;

            return true;
        }

        private bool OnPlayerDespawned(PlayerDespawnEvent signal)
        {
            playerHealthPotion.OnPotionAmountChanged -= PlayerHealthPotion_OnUsePotion;
            playerHealth.OnHealthChanged -= PlayerHealth_OnHealthChanged;
            playerShield.OnShieldAmountChanged -= PlayerShield_OnShieldAmountChanged;

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
            var healthAmountRatio = (float)playerHealth.CurrentHP / playerHealth.MaxHP;
            var newHealthPositionY = healthAmountRatio * barPositionRange + minimumHealthPositionY;

            Vector3 targetPosition = new Vector3(healthPositionX, newHealthPositionY, 0);
            healthBarTransform.DOLocalMove(targetPosition, barChangeDuration).SetEase(Ease.OutExpo);
        }

        private void UpdateShieldBar()
        {
            var shieldAmountRatio = (float)playerShield.CurrentShield / playerShield.MaxShield;
            var newShieldPositionY = shieldAmountRatio * barPositionRange + minimumShieldPositionY;

            Vector3 targetPosition = new Vector3(shieldPositionX, newShieldPositionY, 0);
            shieldBarTransform.DOKill();
            shieldBarTransform.DOLocalMove(targetPosition, barChangeDuration).SetEase(Ease.OutExpo);
            // shieldBarTransform.localPosition = targetPosition;
        }

        private void Player_OnSenterToggle(bool senterState)
        {
            senterLightOnImage.gameObject.SetActive(senterState);
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

        private void GetShieldBarPosition()
        {
            shieldPositionX = shieldBarTransform.localPosition.x;
            minimumShieldPositionY = shieldBarTransform.localPosition.y;
        }

        private void GetHealthBarPosition()
        {
            healthPositionX = healthBarTransform.localPosition.x;
            minimumHealthPositionY = healthBarTransform.localPosition.y;
        }
    }
}
