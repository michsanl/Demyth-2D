using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using UnityEngine.UI;
using System;
using PixelCrushers.DialogueSystem;
using DG.Tweening;
using Sirenix.OdinInspector;

public class HUDUI : SceneService
{
    [Title("Health/Shield Bar Parameter")]
    [SerializeField] private float barChangeDuration;
    [SerializeField] private float barPositionRange = 219;

    [Title("Components")]
    [SerializeField] private Transform healthBarTransform;
    [SerializeField] private Transform shieldBarTransform;
    [SerializeField] private Image healthPotionEmptyImage;
    [SerializeField] private Image senterLightOnImage;

    private Animator animator;
    private HealthPotion playerHealthPotion;
    private Health playerHealth;
    private Shield playerShield;
    private bool isOpen;

    private float healthPositionX;
    private float minimumHealthPositionY;
    private float shieldPositionX;
    private float minimumShieldPositionY;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        playerHealthPotion = Context.Player.GetComponent<HealthPotion>();
        playerHealth = Context.Player.GetComponent<Health>();
        playerShield = Context.Player.GetComponent<Shield>();
        animator = GetComponent<Animator>();

        GetHealthBarPosition();
        GetShieldBarPosition();

        playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
        playerShield.OnShieldAmountChanged += PlayerShield_OnShieldAmountChanged;

        Open();
    }

    protected override void OnActivate()
    {
        base.OnActivate();

        playerHealthPotion.OnPotionAmountChanged += PlayerHealthPotion_OnUsePotion;
        Context.Player.OnSenterToggle += Player_OnSenterToggle;
        DialogueManager.instance.conversationStarted += DialogueManager_ConversationStarted;
        DialogueManager.instance.conversationEnded += DialogueManager_ConversationEnded;
    }

    private void DialogueManager_ConversationStarted(Transform t)
    {
        Close();
    }

    private void DialogueManager_ConversationEnded(Transform t)
    {
        Open();
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

        Vector3 targetPosition = new Vector3 (healthPositionX, newHealthPositionY, 0);
        healthBarTransform.DOLocalMove(targetPosition, barChangeDuration).SetEase(Ease.OutExpo);
    }

    private void UpdateShieldBar()
    {
        var shieldAmountRatio = (float)playerShield.CurrentShield / playerShield.MaxShield;
        var newShieldPositionY = shieldAmountRatio * barPositionRange + minimumShieldPositionY;

        Vector3 targetPosition = new Vector3 (shieldPositionX, newShieldPositionY, 0);
        shieldBarTransform.DOLocalMove(targetPosition, barChangeDuration).SetEase(Ease.OutExpo);
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

    public void Open()
    {
        if (isOpen)
            return;

        animator.Play("HUD_Open");
        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen)
            return;

        animator.Play("HUD_Close");
        isOpen = false;
    }
}
