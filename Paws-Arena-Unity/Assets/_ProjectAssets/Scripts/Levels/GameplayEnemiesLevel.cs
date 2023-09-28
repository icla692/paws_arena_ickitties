using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayEnemiesLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelDisplay;
    [SerializeField] private Image levelBar;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (photonView == null)
        {
            SetBotsLevel();
        }
        else
        {
            SetOpponentsLevel();
        }
    }

    private void SetBotsLevel()
    {
        if (DataManager.Instance.GameData.SeasonEnds <= DateTime.Now)
        {
            levelDisplay.text = "1";
            levelBar.fillAmount = 0;
            return;
        }

        float _maxExp = 57000;

        int _timePassed = (int)(DateTime.Now - DataManager.Instance.GameData.SeasonEnds.AddMonths(-1)).TotalSeconds;

        if (_timePassed<=0)
        {
            levelDisplay.text = "0";
            levelBar.fillAmount = 0;
            return;
        }

        float _expPerSecond = _maxExp / (int)(DataManager.Instance.GameData.SeasonEnds-DataManager.Instance.GameData.SeasonEnds.AddMonths(-1)).TotalSeconds;

        float _collectedExp = _timePassed * _expPerSecond;
        ShowProgress((int)_collectedExp);
    }

    private void SetOpponentsLevel()
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
