using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Demyth.Gameplay;

public class SriAbilityTeleport : Ability
{
    [SerializeField] private AnimationPropertiesSO _teleportProp;
    [SerializeField] private SriClipSO _sriClipSO;
    [SerializeField] private Animator _animator;

    private Player _player;
    private Vector2[] pillarPositionArray = new Vector2[] { new(-5, 1), new(-5, -3), new(5, 1), new(5, -3) };
    private int TELEPORT_START = Animator.StringToHash("Teleport_Start");
    private int TELEPORT_END = Animator.StringToHash("Teleport_End");
    private const int TeleportDirectionRight = 0;
    private const int TeleportDirectionLeft = 1;
    private const int TeleportDirectionUp = 2;
    private const int TeleportDirectionDown = 3;
    private int topBorder = 2;
    private int bottomBorder = -4;
    private int rightBorder = 6;
    private int leftBorder = -6;

    private void Awake()
    {
        _player = SceneServiceProvider.GetService<PlayerManager>().Player;
    }

    public override IEnumerator PlayAbility()
    {
        _animator.SetFloat("Teleport_Multiplier", _teleportProp.AnimationSpeedMultiplier);
        
        _animator.SetTrigger(TELEPORT_START);
        yield return Helper.GetWaitForSeconds(_teleportProp.GetFrontSwingDuration());

        var teleportTargetPosition = GetTeleportTargetPosition(_player.LastMoveTargetPosition);
        transform.position = teleportTargetPosition;

        _animator.SetTrigger(TELEPORT_END);
        yield return Helper.GetWaitForSeconds(_teleportProp.GetBackSwingDuration());
    }

    private Vector2 GetTeleportTargetPosition(Vector2 playerPosition)
    {
        var teleportTargetPosition = GetPositionAroundPlayer(playerPosition);

        while (IsSamePosition(teleportTargetPosition) || IsOutOfArena(teleportTargetPosition) || IsPillarPosition(teleportTargetPosition))
        {
            teleportTargetPosition = GetPositionAroundPlayer(playerPosition);
        }

        return teleportTargetPosition;
    }

    private Vector2 GetPositionAroundPlayer(Vector2 playerPosition)
    {
        var randomIndex = Random.Range(0, 4);

        switch (randomIndex)
        {
            case TeleportDirectionRight:
                playerPosition.x += GetPositionOffset();
                break;
            case TeleportDirectionLeft:
                playerPosition.x -= GetPositionOffset();
                break;
            case TeleportDirectionUp:
                playerPosition.y += GetPositionOffset();
                break;
            case TeleportDirectionDown:
                playerPosition.y -= GetPositionOffset();
                break;
        }

        return playerPosition;
    }

    private int GetPositionOffset()
    {
        return Random.Range(2, 4);
    }

    private bool IsSamePosition(Vector3 targetPosition)
    {
        return targetPosition == transform.position;
    }

    private bool IsOutOfArena(Vector3 targetPosition)
    {
        var positionY = targetPosition.y;
        var positionX = targetPosition.x;

        return positionY > topBorder || positionY < bottomBorder || positionX > rightBorder || positionX < leftBorder;
    }

    private bool IsPillarPosition(Vector2 targetPosition)
    {
        foreach (var pillarPosition in pillarPositionArray)
        {
            if (targetPosition == pillarPosition)
                return true;
        }
        return false;
    }
}
