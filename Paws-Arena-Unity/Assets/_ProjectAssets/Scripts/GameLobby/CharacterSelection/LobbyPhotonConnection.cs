using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyPhotonConnection : MonoBehaviour
{
    [Header("Managers")]
    public PhotonManager photonManager;
    public LobbyUIManager lobbyUIManager;

    [Header("Internals")]
    public GameObject startButton;
    public GameObject joinRoomLog;


    private void OnEnable()
    {
        Init();

        photonManager.OnCreatingRoom += OnCreatingRoom;
    }

    private void OnDisable()
    {
        photonManager.OnCreatingRoom -= OnCreatingRoom;
    }

    private void Init()
    {
        startButton.SetActive(true);
        joinRoomLog.GetComponent<TextMeshProUGUI>().text = "Finding your opponent...";
        joinRoomLog.SetActive(false);
    }


    public void TryJoinRoom()
    {
        startButton.SetActive(false);
        joinRoomLog.SetActive(true);

        photonManager.ConnectToRandomRoom();
    }

    public void TryJoinSinglePlayerRoom()
    {
        startButton.SetActive(false);
        joinRoomLog.SetActive(true);

        photonManager.CreateSinglePlayerRoom();
    }

    private void OnCreatingRoom()
    {
        joinRoomLog.GetComponent<TextMeshProUGUI>().text = "No open match. Making a new one...";
    }
}
