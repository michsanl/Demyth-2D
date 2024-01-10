using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookOrientation : MonoBehaviour
{
    [SerializeField]
    private Transform model;
    private float xScale = 1f;

    private void Start()
    {
        if (model == null) 
            return;
        xScale = model.localScale.x;
    }

    public void SetFacingDirection(Vector2 moveValue)
    {
        if(moveValue.y == 0)
        {
            if (moveValue.x > 0)
            {
                FaceDirection(1);
            }
            else
            {
                FaceDirection(-1);
            }
        }
    }

    private void FaceDirection(float direction)
    {
        if (model != null)
        {
            model.transform.localScale = new Vector2(Mathf.Abs(xScale) * direction, model.localScale.y);
        }
    }
}
