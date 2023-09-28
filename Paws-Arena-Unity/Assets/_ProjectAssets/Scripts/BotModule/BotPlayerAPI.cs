using Anura.Templates.MonoSingleton;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayerAPI : MonoSingleton<BotPlayerAPI>
{
    public Action<float> onMove;
    public Action onMainAction;

    [Header("Dependencies")]
    public PlayerActionsBar playerActionsBar;

    public Action onMoveCancelled;
    public Action onJumpStarted;

    [Header("Params")]
    [Range(0, 3)]
    public int weaponIdx = 0;
    [Range(0, 360)]
    public float shootAngle = 0;
    [Range(0, 1)]
    public float shootPower = 1;

    private IndicatorInputCircleBehaviour indicatorCircle;
    private bool isEnabled = false;
    private bool wasLastTurnOfBot = false;

    private void OnDestroy()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    public void Init(PlayerMotionBehaviour playerMotionBehaviour, IndicatorInputCircleBehaviour indicatorCircle)
    {
        playerMotionBehaviour.RegisterBotAPICallbacks(this);
        this.indicatorCircle = indicatorCircle;

        RoomStateManager.OnStateUpdated += OnStateUpdated;

        if (!LuckyWheelWhoPlaysFirst.DoIPlayFirst)
        {
            wasLastTurnOfBot = true;
        }
    }

    private void OnStateUpdated(IRoomState state)
    {
        isEnabled = state is BotTurnState;

        if(state is BotTurnState)
        {
            wasLastTurnOfBot = true;
        }else if(state is MyTurnState)
        {
            wasLastTurnOfBot = false;
        }
    }

    public void Disable()
    {
        CancelMove();
        isEnabled = false;
    }

    [Button("Move Left")]
    public void MoveLeft()
    {
        Move(-1);
    }
    [Button("Move Right")]
    public void MoveRight()
    {
        Move(1);
    }

    //-1 = left, 1 right
    public void Move(float dir)
    {
        if (!isEnabled) return;
        onMove?.Invoke(dir);
    }

    [Button("Cancel Move")]
    public void CancelMove()
    {
        if (!isEnabled) return;
        onMoveCancelled?.Invoke();
    }

    [Button("Jump")]
    public void Jump()
    {
        if (!isEnabled) return;
        onJumpStarted?.Invoke();
    }

    [Button("Select Weapon")]
    public void SelectWeapon()
    {
        if (!isEnabled) return;
        playerActionsBar.WeaponOut(weaponIdx);
    }

    [Button("Select Angle/Power")]
    public void SelectAnglePower()
    {
        if (!isEnabled) return;
        indicatorCircle.CheckPointerClick(shootAngle, shootPower);
    }

    [Button("Shoot")]
    public void Shoot()
    {
        if (wasLastTurnOfBot)
        {
            if(RoomStateManager.Instance.currentState is BotTurnState)
            {
                playerActionsBar.Shoot();
            }
            
            if(RoomStateManager.Instance.currentState is BotTurnState || RoomStateManager.Instance.currentState is ProjectileLaunchedState)
            {
                onMainAction?.Invoke();
            }
        }
    }
}
