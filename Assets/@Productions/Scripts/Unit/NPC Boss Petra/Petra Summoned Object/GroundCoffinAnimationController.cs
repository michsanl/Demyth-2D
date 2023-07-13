using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCoffinAnimationController : MonoBehaviour
{
    
    [SerializeField] private float anticipationDuration;
    [SerializeField] private float attackDuration;

    private int COFFIN_IN = Animator.StringToHash("Ground_Coffin_In");
    private int COFFIN_ATTACK = Animator.StringToHash("Ground_Coffin_Attack");
    private int COFFIN_OUT = Animator.StringToHash("Ground_Coffin_Out");
    private Animator animator;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        animator.Play(COFFIN_IN);
        yield return Helper.GetWaitForSeconds(anticipationDuration);
        animator.Play(COFFIN_ATTACK);
        yield return Helper.GetWaitForSeconds(attackDuration);
        animator.Play(COFFIN_OUT);
    }
}
