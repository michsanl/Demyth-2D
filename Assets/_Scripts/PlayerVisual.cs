using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Player player; 
    [SerializeField] private TemporarySaveDataSO temporarySaveDataSO;
    [SerializeField] private Animator animator;

    private void Start() 
    {
        // player.OnTurningRight += Player_OnTurningRight;
        // player.OnTurningLeft += Player_OnTurningLeft;
        player.OnMove += Player_OnMove;
        player.OnMovementInputPressed += Player_OnMovementInputPressed;

        if (temporarySaveDataSO.level01.playerDirection != Vector3.zero)
        {
            transform.localScale = temporarySaveDataSO.level01.playerDirection;
        }
    }

    // ngatur madep kanan kiri
    private void Player_OnMovementInputPressed(object sender, Player.OnMovementInputPressedEventArgs e)
    {
        Vector3 localScale = transform.localScale;
        Vector3 newScale = new Vector3(e.inputVectorX, localScale.y, localScale.z);
        transform.localScale = newScale;
    }

    // private void Player_OnTurningRight(object sender, EventArgs e)
    // {
    //     transform.localScale = new Vector3(1,1,1);
    // }

    // private void Player_OnTurningLeft(object sender, EventArgs e)
    // {
    //     transform.localScale = new Vector3(-1,1,1);
    // }

    private void Player_OnMove(object sender, EventArgs e)
    {
        animator.SetTrigger("Dash");
    }

    
    // nge save posisi madep si player
    private void OnApplicationQuit() {
        temporarySaveDataSO.level01.playerDirection = transform.localScale;
    }
}
