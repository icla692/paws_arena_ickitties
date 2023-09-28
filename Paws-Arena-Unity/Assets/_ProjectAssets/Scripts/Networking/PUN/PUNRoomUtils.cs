using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PUNRoomUtils : MonoBehaviourPunCallbacks
{
    public static event Action<string, string> onPlayerJoined;
    public static event Action onPlayerLeft;


    public void TryLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public void AddPlayerCustomProperty(string key, string value)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[key] = value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    public void AddRoomCustomProperty(string key, int value)
    {
        Hashtable props = new Hashtable();
        props.Add(key, value);
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    public object GetRoomCustomProperty(string key)
    {
        return PhotonNetwork.CurrentRoom.CustomProperties[key];
    }


    public List<Player> GetOtherPlayers()
    {
        return PhotonNetwork.CurrentRoom.Players.Select(kvp => kvp.Value).Where(player => !player.IsLocal).ToList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player Joined room {newPlayer.NickName}");
        onPlayerJoined?.Invoke(newPlayer.NickName, newPlayer.UserId);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"Player Left Room {otherPlayer.NickName}");
        onPlayerLeft?.Invoke();
    }
}
