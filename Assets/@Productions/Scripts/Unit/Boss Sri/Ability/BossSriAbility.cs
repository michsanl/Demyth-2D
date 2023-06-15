using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CustomTools.Core;
using System;
using Sirenix.OdinInspector;

public class BossSriAbility : SceneService
{
    [Title("Summoned Object")]
    [SerializeField] protected GameObject groundNailSingle;
    [SerializeField] protected GameObject groundNailPenta;
    [SerializeField] protected GameObject fireBallProjectile;
    [SerializeField] protected NailProjectile nailProjectile;

    [Title("Summon Spawn Point")]
    [SerializeField] protected Transform fireBallSpawnPoint;

    [Title("Attack Ability Collider")]
    [SerializeField] private GameObject horizontalSlashCollider;
    [SerializeField] private GameObject verticalSlashCollider;
    [SerializeField] private GameObject spinNailCollider;
    [SerializeField] private GameObject nailAOECollider;

    [Title("Other Component")]
    [SerializeField] protected Animator animator;
    [SerializeField] private AudioClipSriSO audioClipSriSO;

    protected AudioManager audioManager;
    protected LookOrientation lookOrientation;
    protected MovementController movementController;
    protected bool isBusy;
    protected bool isMoving;
    protected bool isIntroPlayed;

    protected int INTRO = Animator.StringToHash("Intro");
    protected int UP_SLASH = Animator.StringToHash("Up_Slash");
    protected int DOWN_SLASH = Animator.StringToHash("Down_Slash");
    protected int HORIZONTAL_SLASH = Animator.StringToHash("Horizontal_Slash");
    protected int NAIL_AOE = Animator.StringToHash("Nail_AOE");
    protected int SPIN_CLAW = Animator.StringToHash("Spin_Claw");
    protected int FIRE_BALL = Animator.StringToHash("Fire_Ball");
    protected int NAIL_SUMMON_1 = Animator.StringToHash("Nail_Summon_1");

    protected override void OnInitialize()
    {
        lookOrientation = GetComponent<LookOrientation>();
        movementController = GetComponent<MovementController>();
        audioManager = Context.AudioManager;
    }

#region PlayerToBossPositionInfo
    protected bool IsPlayerAbove()
    {
        return transform.position.y < Context.Player.transform.position.y;
    }

    protected bool IsPlayerBelow()
    {
        return transform.position.y > Context.Player.transform.position.y;
    }

    protected bool IsPlayerToRight()
    {
        return transform.position.x < Context.Player.transform.position.x;
    }

    protected bool IsPlayerToLeft()
    {
        return transform.position.x > Context.Player.transform.position.x;
    }

    protected bool IsPlayerAtSamePosX()
    {
        return Mathf.Approximately(transform.position.x, Context.Player.transform.position.x) ;
    }

    protected bool IsPlayerAtSamePosY()
    {
        return Mathf.Approximately(transform.position.y, Context.Player.transform.position.y);
    }

    protected bool IsPlayerNearby()
    {
        return Vector2.Distance(transform.position, Context.Player.transform.position) < 1.5f;
    }
#endregion

    protected IEnumerator PlayMove(Vector2 direction)
    {
        isMoving = true;

        Vector2 moveTargetPosition = (Vector2)transform.position + direction;
        moveTargetPosition = GetRoundedVector(moveTargetPosition);

        float moveDuration = .25f;
        float actionDelay = 1f;

        transform.DOMove(moveTargetPosition, moveDuration).SetEase(Ease.OutExpo);
        yield return Helper.GetWaitForSeconds(actionDelay);

        isMoving = false;
    }


    protected IEnumerator PlayIntro()
    {
        isBusy = true;

        animator.Play(INTRO);
        yield return Helper.GetWaitForSeconds(4.1f);
        isIntroPlayed = true;

        isBusy = false;
    }

    protected void PlayTeleport()
    {
        Vector3 playerPosition = Context.Player.MoveTargetPosition;
        int randomIndex = UnityEngine.Random.Range(0, 4);
        switch (randomIndex)
        {
            case 0:
                playerPosition.x = playerPosition.x + 2;
                break;
            case 1:
                playerPosition.x = playerPosition.x - 2;
                break;
            case 2:
                playerPosition.y = playerPosition.y + 2;
                break;
            case 3:
                playerPosition.y = playerPosition.y - 2;
                break;
        }
        transform.position = playerPosition;
    }

    protected IEnumerator PlayRightSlash(float targetPosition)
    {
        isBusy = true;

        //lookOrientation.SetFacingDirection(Vector2.right);
        
        targetPosition = Mathf.Clamp(Mathf.Round(targetPosition), -6, 6);

        float frontSwing = .833f;
        float swing = .233f;
        float backSwing = 1.267f;

        animator.Play(HORIZONTAL_SLASH);
        audioManager.PlaySound(audioClipSriSO.HorizontalSlash);

        yield return Helper.GetWaitForSeconds(frontSwing);
        horizontalSlashCollider.SetActive(true);
        yield return transform.DOMoveX(targetPosition, swing).SetEase(Ease.OutExpo).WaitForCompletion();
        horizontalSlashCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwing);


        isBusy = false;
    }

    protected IEnumerator PlayLeftSlash(float targetPosition)
    {
        isBusy = true;

        //lookOrientation.SetFacingDirection(Vector2.left);
        
        targetPosition = Mathf.Clamp(Mathf.Round(targetPosition), -6, 6);
        
        float frontSwing = .833f;
        float swing = .233f;
        float backSwing = 1.267f;

        animator.Play(HORIZONTAL_SLASH);
        audioManager.PlaySound(audioClipSriSO.HorizontalSlash);

        yield return Helper.GetWaitForSeconds(frontSwing);
        horizontalSlashCollider.SetActive(true);
        yield return transform.DOMoveX(targetPosition, swing).SetEase(Ease.OutExpo).WaitForCompletion();
        horizontalSlashCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwing);


        isBusy = false;
    }

    protected IEnumerator PlayUpSlash(float targetPosition)
    {
        isBusy = true;
        
        targetPosition = Mathf.Clamp(Mathf.Round(targetPosition), -4, 2);

        float frontSwing = .6f;
        float swing = .233f;
        float backSwing = 1.1f;

        animator.Play(UP_SLASH);
        audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwing);
        verticalSlashCollider.SetActive(true);
        yield return transform.DOMoveY(targetPosition, swing).SetEase(Ease.OutExpo).WaitForCompletion();
        verticalSlashCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwing);

        isBusy = false;
    }

    protected IEnumerator PlayDownSlash(float targetPosition)
    {
        isBusy = true;
        
        targetPosition = Mathf.Clamp(Mathf.Round(targetPosition), -4, 2);

        float frontSwing = .6f;
        float swing = .233f;
        float backSwing = 1.1f;

        animator.Play(DOWN_SLASH);
        audioManager.PlaySound(audioClipSriSO.VerticalSlash);

        yield return Helper.GetWaitForSeconds(frontSwing);
        verticalSlashCollider.SetActive(true);
        yield return transform.DOMoveY(targetPosition, swing).SetEase(Ease.OutExpo).WaitForCompletion();
        verticalSlashCollider.SetActive(false);
        yield return Helper.GetWaitForSeconds(backSwing);


        isBusy = false;
    }

    protected IEnumerator PlayNailAOE()
    {
        isBusy = true;

        // float animationDuration = 4.1f;
        float frontSwing = .7f;
        float swing = 3.133f;
        float backSwing = .266f;

        animator.Play(NAIL_AOE);
        audioManager.PlaySound(audioClipSriSO.NailAOE);

        yield return Helper.GetWaitForSeconds(frontSwing);
        
        nailAOECollider.SetActive(true);
        yield return Helper.GetWaitForSeconds(swing);
        nailAOECollider.SetActive(false);
        
        yield return Helper.GetWaitForSeconds(backSwing);

        isBusy = false;
    }

    protected IEnumerator PlaySpinClaw()
    {
        isBusy = true;

        //float animationDuration = 2.5f;
        float frontSwing = 1f;
        float swing = 1f;
        float backSwing = .5f;

        animator.Play(SPIN_CLAW);
        audioManager.PlaySound(audioClipSriSO.SpinClaw);

        yield return Helper.GetWaitForSeconds(frontSwing);
        
        spinNailCollider.SetActive(true);
        yield return Helper.GetWaitForSeconds(swing);
        spinNailCollider.SetActive(false);
        
        yield return Helper.GetWaitForSeconds(backSwing);

        isBusy = false;
    }

    protected IEnumerator PlayFireBall()
    {
        isBusy = true;

        float animationDuration = 2.0667f;
        animator.Play(FIRE_BALL);

        var fireBall = Instantiate(fireBallProjectile, fireBallSpawnPoint.position, Quaternion.identity);

        // audioManager.PlaySound(audioClipSriSO.Fireball);
        yield return Helper.GetWaitForSeconds(animationDuration);

        isBusy = false;
    }

    protected IEnumerator PlayNailSummon(GameObject nailGameObject)
    {
        isBusy = true;

        float animationDuration = 1.8f;

        animator.Play(NAIL_SUMMON_1);
        audioManager.PlaySound(audioClipSriSO.NailSummon);

        Vector2 playerPosition = GetRoundedVector(Context.Player.transform.position);

        Instantiate(nailGameObject, playerPosition, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(animationDuration);

        isBusy = false;
    }

    protected IEnumerator PlayNailAOEShootingNail()
    {
        isBusy = true;

        // float animationDuration = 4.1f;
        float frontSwing = .7f;
        float swing = 3.133f;
        float backSwing = .266f;

        animator.Play(NAIL_AOE);
        audioManager.PlaySound(audioClipSriSO.NailAOE);

        yield return Helper.GetWaitForSeconds(frontSwing);
        
        nailAOECollider.SetActive(true);
        StartCoroutine(SpawnNailProjectile());
        yield return Helper.GetWaitForSeconds(swing);
        nailAOECollider.SetActive(false);
        
        yield return Helper.GetWaitForSeconds(backSwing);

        isBusy = false;
    }

    protected IEnumerator SpawnNailProjectile()
    {
        var origin = transform.position + Vector3.up * .5f;
        var nailProjectile = Instantiate(this.nailProjectile, origin, Quaternion.identity);
        nailProjectile.Setup(Vector3.down);
        yield return Helper.GetWaitForSeconds(1f);
        
        nailProjectile = Instantiate(this.nailProjectile, origin, Quaternion.identity);
        nailProjectile.Setup(Vector3.right);
        yield return Helper.GetWaitForSeconds(1f);
        
        nailProjectile = Instantiate(this.nailProjectile, origin, Quaternion.identity);
        nailProjectile.Setup(Vector3.up);
        yield return Helper.GetWaitForSeconds(1f);
        
        nailProjectile = Instantiate(this.nailProjectile, origin, Quaternion.identity);
        nailProjectile.Setup(Vector3.left);
    }

    private Vector2 GetRoundedVector(Vector2 vector)
    {
        vector.x = Mathf.Round(vector.x);
        vector.y = Mathf.Round(vector.y);

        return vector;
    }

}
