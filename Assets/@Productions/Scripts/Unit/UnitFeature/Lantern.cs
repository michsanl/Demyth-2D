using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using System;

public class Lantern : MonoBehaviour
{

    public Action<bool> OnLanternTogglePerformed;
    
    [SerializeField] private GameObject _lanternGameObject;

    private bool _isSenterEnabled;

    public void ToggleLantern()
    {
        if (_lanternGameObject.activeInHierarchy)
        {
            StartCoroutine(TurnOffLanternCoroutine());
        }
        else
        {
            StartCoroutine(TurnOnLanternCoroutine());
        }
    }

    public void TurnOnLantern()
    {
        StartCoroutine(TurnOnLanternCoroutine());
    }

    public void TurnOffLantern()
    {
        StartCoroutine(TurnOffLanternCoroutine());
    }

    private IEnumerator TurnOffLanternCoroutine()
    {
        // Move the object away to trigger OnCollisonExit
        _lanternGameObject.transform.localPosition = new Vector3(100, 100, 0);
        yield return Helper.GetWaitForSeconds(0.05f);

        _lanternGameObject.SetActive(false);
        _isSenterEnabled = false;

        OnLanternTogglePerformed?.Invoke(_isSenterEnabled);
    }

    private IEnumerator TurnOnLanternCoroutine()
    {
        // Move the object in to trigger OnCollisonEnter
        _lanternGameObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        yield return Helper.GetWaitForSeconds(0.05f);

        _lanternGameObject.SetActive(true);
        _isSenterEnabled = true;

        OnLanternTogglePerformed?.Invoke(_isSenterEnabled);
    }
}
