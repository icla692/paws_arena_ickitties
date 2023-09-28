using System;
using UnityEngine;

public class ChallengeAlert : MonoBehaviour
{
    [SerializeField] private GameObject alert;
    private void OnEnable()
    {
        ChallengesPanel.OnClosed += CheckForAlert;
        CheckForAlert();
    }

    private void OnDisable()
    {
        ChallengesPanel.OnClosed -= CheckForAlert;
    }

    private void CheckForAlert()
    {
        foreach (var _challenge in DataManager.Instance.PlayerData.Challenges.ChallengesData)
        {
            if (_challenge.Completed&&!_challenge.Claimed)
            {
                alert.SetActive(true);
                return;
            }
        }
        
        alert.SetActive(false);
    }
}
