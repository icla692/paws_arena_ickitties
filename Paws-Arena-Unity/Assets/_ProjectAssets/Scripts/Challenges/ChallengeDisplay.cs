using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeDisplay : MonoBehaviour
{
    public static Action<ChallengeData> OnClaimPressed;
    [SerializeField] private Button claimButton;
    [SerializeField] private GameObject claimHolder;
    [SerializeField] private Image rewardDisplay;
    [SerializeField] private TextMeshProUGUI descDisplay;
    [SerializeField] private TextMeshProUGUI progressDisplay;
    [SerializeField] private GameObject completed;

    private ChallengeData challengeData;
    
    public void Setup(ChallengeData _data)
    {
        completed.SetActive(false);
        claimHolder.SetActive(false);
        challengeData = _data;
        ChallengeSO _challengeSO = ChallengesManager.Instance.Get(_data.Id);
        if (_data.Completed)
        {
            if (_data.Claimed)
            {
                completed.SetActive(true);
            }
            else
            {
                claimHolder.SetActive(true);
            }
            descDisplay.text = string.Empty;
            progressDisplay.text = string.Empty;
        }
        else
        {
            descDisplay.text = _challengeSO.Description;
            progressDisplay.text = $"{_data.Value}/{_challengeSO.AmountNeeded}";
        }

        rewardDisplay.sprite = _challengeSO.RewardSprite;
    }

    private void OnEnable()
    {
        claimButton.onClick.AddListener(Claim);
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(Claim);
    }

    private void Claim()
    {
        OnClaimPressed?.Invoke(challengeData);
        Setup(challengeData);
    }
}
