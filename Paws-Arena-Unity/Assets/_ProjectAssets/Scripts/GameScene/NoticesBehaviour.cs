using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticesBehaviour : MonoBehaviour
{
    public GameObject wrapper;
    public TMPro.TextMeshProUGUI label;

    private void OnEnable()
    {
        wrapper.SetActive(false);
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDisable()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {

        if (state is WaitingForAllPlayersToJoinState)
        {
            SetStatus("Waiting for players to join");
        }else if(state is StartingGameState)
        {
            SetStatus("Let's go");
        }else if(state is MyTurnState)
        {
            SetStatus("");
        }
        else if(state is OtherPlayerTurnState)
        {
            SetStatus("");
        }
        else if (state is ProjectileLaunchedState)
        {
            SetStatus("Attack Launched");
        }else if(state is ResolvingGameState)
        {
            SetStatus("Match decided!");
        }
    }

    private void SetStatus(string value)
    {
        wrapper.SetActive(true);
        label.text = value;
    }
}
