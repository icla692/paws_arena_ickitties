using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundCounterBehaviour : MonoBehaviour
{
    public TMPro.TextMeshProUGUI roundCounterText;

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
        if((state is MyTurnState && ConfigurationManager.Instance.Config.GetGameType() == Anura.ConfigurationModule.ScriptableObjects.GameType.SINGLEPLAYER) ||
            (state is MyTurnState && PhotonNetwork.LocalPlayer.IsMasterClient) || 
            (state is OtherPlayerTurnState && !PhotonNetwork.LocalPlayer.IsMasterClient))
        {
            int round = RoomStateManager.Instance.roundNumber;
            roundCounterText.text = $"{round}/{ConfigurationManager.Instance.Config.GetMaxNumberOfRounds()}";
        }
    }
}
