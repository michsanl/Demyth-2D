using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class NailProjectile : MonoBehaviour
{

    [SerializeField] private float moveDistanceX;
    [SerializeField] private float moveDistanceY;
    [SerializeField] private float moveDuration;
    [SerializeField] private float moveDelay;
    [SerializeField] private float destroyTime;

    private void Awake() 
    {
        Destroy(gameObject, destroyTime);

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return Helper.GetWaitForSeconds(moveDelay);
        transform.DOMove(GetMoveTargetPosition(), moveDuration);
    }

    private Vector3 GetMoveTargetPosition()
    {
        var movePositionIncrement = new Vector3(moveDistanceX, moveDistanceY, 0);
        var moveTarget = transform.position + movePositionIncrement;
        return moveTarget;
    }
}
