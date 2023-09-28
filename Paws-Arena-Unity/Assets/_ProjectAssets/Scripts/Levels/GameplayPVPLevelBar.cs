using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPVPLevelBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelDisplay;
    [SerializeField] private Image levelBar;
    [SerializeField] private bool isForMasterClient;

    private PhotonView photonView;

    private bool isMine => (PhotonNetwork.IsMasterClient&& isForMasterClient)||(!PhotonNetwork.IsMasterClient&&!isForMasterClient);

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    
    private void OnEnable()
    {
        if (isMine)
        {
            DataManager.Instance.PlayerData.UpdatedExp += TellOpponentThatIEarnedExp;
        }
    }

    private void OnDisable()
    {
        if (isMine)
        {
            DataManager.Instance.PlayerData.UpdatedExp -= TellOpponentThatIEarnedExp;
        }
    }

    private void Start()
    {
        if (isMine)
        {
            TellOpponentMyLevel();
            ShowMy();
        }
    }

    private void TellOpponentThatIEarnedExp()
    {
        ShowMy();
        TellOpponentMyLevel();
    }
    
    private void ShowMy()
    {
        levelDisplay.text = DataManager.Instance.PlayerData.Level.ToString();
        levelBar.fillAmount = (float)DataManager.Instance.PlayerData.ExperienceOnCurrentLevel / DataManager.Instance.PlayerData.ExperienceForNextLevel;
    }

    private void TellOpponentMyLevel()
    {
        int _experience=0;
        if (DataManager.Instance.GameData.SeasonEnds > DateTime.Now)
        {
            _experience = DataManager.Instance.PlayerData.Experience;
        }
        photonView.RPC(nameof(TellOpponentMyExp),RpcTarget.Others,_experience);
    }

    private void ShowProgress(int _experience)
    {
        int _level;
        int _expForNextLevel;
        int _expOnCurrentLevel;

        PlayerData.CalculateLevel(_experience, out _level, out _expForNextLevel, out _expOnCurrentLevel);

        levelDisplay.text = _level.ToString();
        levelBar.fillAmount = (float)_expOnCurrentLevel / _expForNextLevel;
    }

    [PunRPC]
    private void TellOpponentMyExp(int _experience)
    {
        ShowProgress(_experience);
    }
}
