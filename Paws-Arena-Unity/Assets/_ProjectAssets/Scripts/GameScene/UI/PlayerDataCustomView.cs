using Anura.ConfigurationModule.Managers;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataCustomView : MonoBehaviour
{
    public static PlayerDataCustomView npcBar;
    [SerializeField]
    private TMPro.TextMeshProUGUI nicknameText;

    [SerializeField]
    private HealthUIBehaviour healthUIBehaviour;

    [SerializeField]
    private string parentPath;

    [SerializeField]
    private PhotonView photonview;

    [SerializeField]
    public bool isForNPC = false;

    private bool isMultiplayer;

    private void Start()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();

        healthUIBehaviour.Init();

        if (!isForNPC)
        {
            string nickname = !isMultiplayer ? GameState.nickname : PhotonNetwork.NickName;

            PlayerManager.Instance.onHealthUpdated += OnHealthUpdated;
            SingleAndMultiplayerUtils.RpcOrLocal(this, photonview, true, "SetNickname", RpcTarget.All, nickname);
            OnHealthUpdated(ConfigurationManager.Instance.Config.GetPlayerTotalHealth());
        }
        else
        {
            npcBar = this; 
            SingleAndMultiplayerUtils.RpcOrLocal(this, photonview, true, "SetNickname", RpcTarget.All, GameState.botInfo.nickname);
        }
        Init();
    }

    public void Init()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.SetParent(GameObject.Find(parentPath).transform);
        rt.localScale = Vector3.one;

        int myseat = isMultiplayer ? PUNGameRoomManager.Instance.GetMySeat() : 0;

        bool isMyPlayer = !isForNPC && (!isMultiplayer || (myseat == 0 && photonview.IsMine || myseat == 1 && !photonview.IsMine));
        rt.anchorMin = rt.anchorMax = rt.pivot = isMyPlayer ? new Vector2(0, 1) : new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, 0);

        //Mirror UI for right UI
        bool isPlayer2Data = isForNPC || (isMultiplayer && (PhotonNetwork.LocalPlayer.IsMasterClient && !photonview.IsMine) || (!PhotonNetwork.LocalPlayer.IsMasterClient && photonview.IsMine));
        if (isPlayer2Data)
        {
            healthUIBehaviour.SetOrientationRight();
            nicknameText.alignment = TMPro.TextAlignmentOptions.Right;
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-225, GetComponent<RectTransform>().anchoredPosition.y); 
            healthUIBehaviour.OverrideColor(new Color(0.9254901960784314f,0.49411764705882355f,0.803921568627451f));
            
        }

        if(isMyPlayer)
        {
            healthUIBehaviour.SetTag();
        }

    }

    private void OnHealthUpdated(int val)
    {
        SingleAndMultiplayerUtils.RpcOrLocal(this, photonview, true, "SetHealth", RpcTarget.All, val);
    }

    [PunRPC]
    public void SetNickname(string nickname)
    {
        nicknameText.text = nickname;
    }

    [PunRPC]
    public void SetHealth(int val)
    {
        healthUIBehaviour.OnHealthUpdated(val);
        if (isForNPC || (isMultiplayer && photonview != null && !photonview.IsMine))
        {
            PlayerManager.Instance.otherPlayerHealth = val;
        }
    }
}
