using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSri_Testing : BossSri_Base
{

    protected bool isOn;

    void Update()
    {
        if (isBusy)
            return;

        if (!isOn)
        {
            StartCoroutine(PlayRightSlash(2f));
            isOn = !isOn;
        } else
        {
            StartCoroutine(PlayLeftSlash(-4f));
            isOn = !isOn;
        }
    }
}
