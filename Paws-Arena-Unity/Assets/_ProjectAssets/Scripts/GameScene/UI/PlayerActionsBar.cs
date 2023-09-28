using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsBar : MonoBehaviour
{
    public static event Action OnShoot;
    public static event Action<int> WeaponIndexUpdated;

    public GameObject playerActionsWrapper;
    public GameObject weaponsBar;

    private void OnEnable()
    {
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDisable()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {
        if (state is MyTurnState)//|| state is BotTurnState
        {
            playerActionsWrapper.SetActive(true);
            weaponsBar.SetActive(true);
        }
        else
        {
            playerActionsWrapper.SetActive(false);
        }
    }

    public void Shoot()
    {
        OnShoot?.Invoke();
    }

    public void WeaponOut(int value)
    {
        WeaponIndexUpdated?.Invoke(value);
    }

    public void EnableWeaponsBar()
    {
        playerActionsWrapper.SetActive(true);
        weaponsBar.SetActive(true);
    }
}
