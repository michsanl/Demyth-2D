using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarLight : Interactable
{
    [SerializeField] private GameObject lightModel;

    public override void Interact(Vector3 direction = default)
    {
        lightModel.SetActive(!lightModel.activeInHierarchy);
    }
}
