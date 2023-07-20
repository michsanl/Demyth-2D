using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarLight : Interactable
{
    [SerializeField] private int hitToActivate = 6;
    [SerializeField] private GameObject lightModel;

    private int hitCount;

    public override void Interact(Vector3 direction = default)
    {
        if (!lightModel.activeInHierarchy)
        {
            hitCount++;
            if (hitCount >= hitToActivate)
            {
                lightModel.SetActive(true);
                hitCount = 0;
            }
        }
        else
        {
            lightModel.SetActive(false);
        }
    }
}
