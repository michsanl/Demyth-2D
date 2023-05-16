using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTools.Core;
using UnityEngine.UI;

public class HUDUI : SceneService
{
    [SerializeField] private Image healthPointImage;
    [SerializeField] private Image healthPotionImage;
    [SerializeField] private Image senterImage;

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

    public void SetActiveSenterImage(bool state)
    {
        senterImage.gameObject.SetActive(state);
    }
}
