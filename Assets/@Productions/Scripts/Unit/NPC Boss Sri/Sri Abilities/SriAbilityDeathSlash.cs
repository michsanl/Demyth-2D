using System.Collections;
using System.Collections.Generic;
using CustomTools.Core;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using Lean.Pool;

public class SriAbilityDeathSlash : MonoBehaviour
{
    [SerializeField] private float teleportStartDuration;
    [SerializeField] private float teleportEndDuration;
    [SerializeField] private GameObject dialogueCollider;
    [SerializeField] private GameObject nailWavePrefab;
    [SerializeField] private SriClipSO _sriClipSO;

    private int TELEPORT_START = Animator.StringToHash("Teleport_Start");
    private int TELEPORT_END = Animator.StringToHash("Teleport_End");
    private int DOWN_SLASH = Animator.StringToHash("Down_Slash");
    private int NAIL_WAVE = Animator.StringToHash("Intro");

    public IEnumerator DeathSlash(Animator animator)
    {
        animator.Play(TELEPORT_START);
        yield return Helper.GetWaitForSeconds(teleportStartDuration);

        transform.position = new Vector3(0, 3, 0);

        animator.Play(TELEPORT_END);
        yield return Helper.GetWaitForSeconds(teleportEndDuration);

        animator.Play(NAIL_WAVE);
        Helper.PlaySFX(_sriClipSO.NailAOE, _sriClipSO.NailAOEVolume);

        yield return Helper.GetWaitForSeconds(.5f);
        LeanPool.Spawn(nailWavePrefab, Vector3.zero, Quaternion.identity);

        yield return Helper.GetWaitForSeconds(3.7f);
        
        animator.Play(DOWN_SLASH);
        Helper.PlaySFX(_sriClipSO.VerticalSlash, _sriClipSO.VerticalSlashVolume);
        
        yield return Helper.GetWaitForSeconds(0.6f);
        dialogueCollider.SetActive(true);
        yield return transform.DOMoveY(-4f, .233f).SetEase(Ease.OutExpo).WaitForCompletion();
        
        yield return Helper.GetWaitForSeconds(1f);
        dialogueCollider.SetActive(false);
    }
}
