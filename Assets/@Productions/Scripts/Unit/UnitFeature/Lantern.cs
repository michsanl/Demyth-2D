using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using System;

public class Lantern : MonoBehaviour
{

    public Action<bool> OnLanternTogglePerformed;
    public bool EnableAutoTurnOff => _enableAutoTurnOff;
    
    [SerializeField] private GameObject _lanternGameObject;
    [SerializeField] private bool _enableAutoTurnOff;

    private bool _isSenterEnabled;
    private Coroutine _turnOffRandomCoroutine;

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

        if (_turnOffRandomCoroutine != null) StopCoroutine(_turnOffRandomCoroutine);
        
        OnLanternTogglePerformed?.Invoke(_isSenterEnabled);
    }

    private IEnumerator TurnOnLanternCoroutine()
    {
        // Move the object in to trigger OnCollisonEnter
        _lanternGameObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        yield return Helper.GetWaitForSeconds(0.05f);

        _lanternGameObject.SetActive(true);
        _isSenterEnabled = true;

        if (_enableAutoTurnOff) _turnOffRandomCoroutine = StartCoroutine(StartLanternTurnOffTimer());
        
        OnLanternTogglePerformed?.Invoke(_isSenterEnabled);
    }

    private IEnumerator StartLanternTurnOffTimer()
    {
        var timer = UnityEngine.Random.Range(5f, 11f);
        yield return Helper.GetWaitForSeconds(timer);

        StartCoroutine(TurnOffSenterWithFlicker());
    }

    private IEnumerator TurnOffSenterWithFlicker()
    {
        _lanternGameObject.SetActive(false);
        yield return Helper.GetWaitForSeconds(.05f);
        _lanternGameObject.SetActive(true);
        yield return Helper.GetWaitForSeconds(.07f);
        _lanternGameObject.SetActive(false);
        yield return Helper.GetWaitForSeconds(.04f);
        _lanternGameObject.SetActive(true);
        yield return Helper.GetWaitForSeconds(.02f);
        _lanternGameObject.SetActive(false);
        yield return Helper.GetWaitForSeconds(.08f);
        _lanternGameObject.SetActive(true);
        yield return Helper.GetWaitForSeconds(.4f);

        TurnOffLantern();
    }
}
