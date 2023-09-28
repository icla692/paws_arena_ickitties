using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingToServer : MonoBehaviour
{
    [Header("Managers")]
    public LobbyUIManager lobbyUIManager;

    [Header("Internals")]
    public GameObject connectButton;
    public GameObject logText;

    [SerializeField] private GameObject loginFailed;

    private void OnDisable()
    {
        ExternalJSCommunication.Instance.onWalletConnected -= OnWalletConnected;
        ExternalJSCommunication.Instance.onNFTsReceived -= OnNFTsReceived;
    }
    public void ConnectToWallet()
    {
        ExternalJSCommunication.Instance.TryConnectWallet();
        connectButton.SetActive(false);
        logText.SetActive(true);
        var text = logText.GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "Waiting the connection with ICP Wallet to be approved...";

        ExternalJSCommunication.Instance.onWalletConnected += OnWalletConnected;
        ExternalJSCommunication.Instance.onNFTsReceived += OnNFTsReceived;
    }

    public void OnWalletConnected()
    {
        ExternalJSCommunication.Instance.onWalletConnected -= OnWalletConnected;
        var text = logText.GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "Connection made. Waiting for NFTs...";
    }

    public void OnNFTsReceived()
    {
        ExternalJSCommunication.Instance.onNFTsReceived -= OnNFTsReceived;
        GameState.walletId = "asd";
        FirebaseManager.Instance.TryLoginAndGetData(GameState.principalId,OnLoginFinished);
    }

    private void OnLoginFinished(bool _result)
    {
        if (_result)
        {
            DataManager.Instance.SubscribeHandlers();
            lobbyUIManager.OpenNFTSelectionScreen();
            // ChallengesManager.Instance.Init();
        }
        else
        {
            loginFailed.SetActive(true);
        }
    }
}
