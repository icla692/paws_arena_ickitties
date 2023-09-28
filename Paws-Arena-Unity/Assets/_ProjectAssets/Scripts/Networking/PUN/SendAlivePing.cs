using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAlivePing : MonoBehaviour
{
    private PhotonView _photonView;

    private void OnEnable()
    {
        _photonView = GetComponent<PhotonView>();
        InvokeRepeating("SendPing", 0, 1);
    }

    private void OnDisable()
    {
        CancelInvoke("SendPing");
    }

    public void SendPing()
    {
        _photonView.RPC("SendPingToAllRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void SendPingToAllRPC()
    {

    }
}
