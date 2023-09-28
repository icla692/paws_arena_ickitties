using Anura.Templates.MonoSingleton;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformPose
{
    [SerializeField]
    public int seatIdx;
    [SerializeField]
    public Vector3 pos;
    [SerializeField]
    public Vector3 scale;
}

public class SyncPlatformsBehaviour : MonoSingleton<SyncPlatformsBehaviour>
{
    public static Action onPlayersChanged;

    public PUNRoomUtils punRoomUtils;
    [SerializeField]
    private GameMatchingScreen gameMatchingScreen;

    [Space]
    public GameObject syncPlayerPlatformPrefab;

    public PlatformPose player1Pose;
    public PlatformPose player2Pose;
    // Start is called before the first frame update
    private void Start()
    {
        var go = PhotonNetwork.Instantiate(syncPlayerPlatformPrefab.name, player1Pose.pos, Quaternion.identity);
        go.GetComponent<SyncPlayerPlatformBehaviour>().punRoomUtils = punRoomUtils;
    }

    private void OnEnable()
    {
        PUNRoomUtils.onPlayerJoined += RepositionCats;
        PUNRoomUtils.onPlayerLeft += RepositionCats;
    }

    private void OnDisable()
    {
        PUNRoomUtils.onPlayerJoined -= RepositionCats;
        PUNRoomUtils.onPlayerLeft -= RepositionCats;
    }
    private void RepositionCats(string nickname, string userId)
    {
        RepositionCats();
    }

    private void RepositionCats()
    {
        onPlayersChanged?.Invoke();
        gameMatchingScreen.SetSeats();
    }

    public void InstantiateBot()
    {
        var go = GameObject.Instantiate(syncPlayerPlatformPrefab, player2Pose.pos, Quaternion.identity);
        go.GetComponent<SyncPlayerPlatformBehaviour>().isBot = true;
        go.GetComponent<SyncPlayerPlatformBehaviour>().punRoomUtils = punRoomUtils;
    }
    public PlatformPose GetMySeatPosition(PhotonView photonView, bool isBot)
    {
        if (isBot)
        {
            return player2Pose;
        }

        if(photonView == null)
        {
            return player1Pose;
        }

        bool isLocalPlayerMaster = PhotonNetwork.CurrentRoom.masterClientId == PhotonNetwork.LocalPlayer.ActorNumber;
        if ((photonView.IsMine && isLocalPlayerMaster) ||
            (!photonView.IsMine && !isLocalPlayerMaster))
        {
            return player1Pose;
        }
        else
        {
            return player2Pose;
        }

        //List<Player> players = punRoomUtils.GetOtherPlayers();
        //if (players.Count == 1)
        //{
        //    int otherPlayerSeat = Int32.Parse(players[0].CustomProperties["seat"].ToString());
        //    if ((photonView.IsMine && otherPlayerSeat == 1) || (!photonView.IsMine && otherPlayerSeat == 0))
        //        return player1Pose;
        //    if ((photonView.IsMine && otherPlayerSeat == 0) || (!photonView.IsMine && otherPlayerSeat == 1))
        //        return player2Pose;
        //}

        //return player1Pose;
    }
}
