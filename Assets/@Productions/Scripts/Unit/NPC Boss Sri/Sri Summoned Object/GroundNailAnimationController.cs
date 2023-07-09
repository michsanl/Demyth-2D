using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundNailAnimationController : MonoBehaviour
{
    [SerializeField] private float anticipationDuration;
    [SerializeField] private float attackDuration;

    private int NAIL_IN = Animator.StringToHash("Ground_Nail_In");
    private int NAIL_ATTACK = Animator.StringToHash("Ground_Nail_Attack");
    private int NAIL_OUT = Animator.StringToHash("Ground_Nail_Out");

    private Animator animator;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        animator.Play(NAIL_IN);
        yield return Helper.GetWaitForSeconds(anticipationDuration);
        
        animator.Play(NAIL_ATTACK);
        yield return Helper.GetWaitForSeconds(attackDuration);

        animator.Play(NAIL_OUT);
    }
}
