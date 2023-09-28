using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameResolveState
{
    NO_WIN,
    DRAW, 
    PLAYER_1_WIN,
    PLAYER_2_WIN,
}

public class GameResolveStateUtils{
    public static int CheckIfIWon(GameResolveState state)
    {
        if ((state == GameResolveState.PLAYER_1_WIN && PhotonNetwork.LocalPlayer.IsMasterClient) ||
            (state == GameResolveState.PLAYER_2_WIN && !PhotonNetwork.LocalPlayer.IsMasterClient))
        {
            return 1;
        }
        else if ((state == GameResolveState.PLAYER_1_WIN && !PhotonNetwork.LocalPlayer.IsMasterClient) ||
            (state == GameResolveState.PLAYER_2_WIN && PhotonNetwork.LocalPlayer.IsMasterClient))
        {
            return -1;
        }
        return 0;
    }
}