using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimerBehaviour : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    private RoomStateManager roomStateManager;
    private PhotonView photonView;
    private int turnTime;
    private float startTime;

    public float TimeLeft { get; private set; }

    private void OnEnable()
    {
        roomStateManager = RoomStateManager.Instance;
        roomStateManager.Timer = this;
        photonView = GetComponent<PhotonView>();

        turnTime = ConfigurationManager.Instance.Config.GetTurnDurationInSeconds();
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDisable()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void Update()
    {
        if (startTime <= 0) return;

        IRoomState state = RoomStateManager.Instance.currentState;

        if (state is MyTurnState || state is OtherPlayerTurnState || state is BotTurnState)
        {
            UpdateTimer(turnTime, () => { roomStateManager.SetState(new ProjectileLaunchedState()); });
        }
    }

    private void OnStateUpdated(IRoomState state)
    {
        if (state is MyTurnState || state is OtherPlayerTurnState || state is BotTurnState)
        {
            startTime = Time.time;
        }else
        {
            SetTimerText("-");
            startTime = -1;
        }
    }

    private void UpdateTimer(int totalTime, Action onFinished)
    {
        float passedTime = Time.time - startTime;
        if (passedTime >= totalTime)
        {
            startTime = -1;
            SetTimerText("-");
            onFinished?.Invoke();
        }
        else
        {
            TimeLeft = totalTime - passedTime;
            SetTimerText(TimeLeft);
        }
    }

    private void SetTimerText(string val)
    {
        text.text = val;
    }
    private void SetTimerText(float time)
    {
        text.text = "" + (int)Math.Floor(time);
    }
}
