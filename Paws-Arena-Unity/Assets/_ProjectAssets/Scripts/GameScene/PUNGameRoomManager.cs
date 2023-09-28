using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PUNGameRoomManager : MonoBehaviourPunCallbacks
{
    private const string roomKey_usersInScene = "usersInScene";
    private const string playerKey_seat= "seat";



    #region SINGLETON
    private static PUNGameRoomManager _instance;
    public static PUNGameRoomManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PUNGameRoomManager>();

                if (_instance == null)
                {
                    Debug.LogError("No PUNGameRoomManager found in scene!");
                }
            }

            return _instance;
        }
    }
    #endregion
    public int GetMySeat()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(playerKey_seat))
        {
            return 0;
        }

        bool tryParse = Int32.TryParse(PhotonNetwork.LocalPlayer.CustomProperties[playerKey_seat].ToString(), out int result);
        
        return result;
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        //if (propertiesThatChanged.TryGetValue(roomKey_usersInScene, out object userCountObject))
        //{
        //}
    }
}
