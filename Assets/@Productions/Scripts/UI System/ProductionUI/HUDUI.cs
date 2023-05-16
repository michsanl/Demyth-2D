using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;

public class HUDUI : SceneService
{
    private Animator animator;

    protected override void OnInitialize()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
        animator.Play("HUD_Open");
    }

    public void Close()
    {
        animator.Play("HUD_Close");
    }
}
