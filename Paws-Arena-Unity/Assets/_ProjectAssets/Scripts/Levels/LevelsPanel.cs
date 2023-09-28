using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class LevelsPanel : MonoBehaviour
{
    [SerializeField] private LevelRewardDisplay[] normalLevelHolders;
    [SerializeField] private LevelRewardDisplay[] premiumLevelHolders;
    [SerializeField] private TextMeshProUGUI[] levelsDisplay;
    [SerializeField] private Image[] levelsBackgroundDisplay;
    [SerializeField] private TextMeshProUGUI levelDisplay;
    [SerializeField] private TextMeshProUGUI seasonNumberDisplay;
    [SerializeField] private TextMeshProUGUI seasonEndDisplay;
    [SerializeField] private Image progressDispaly;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button showPrevious;
    [SerializeField] private Button showNext;
    [SerializeField] private Sprite reachedLevelBackground;
    [SerializeField] private Sprite notReachedLevelBackground;
    [SerializeField] private Button claimAllButton;

    private int firstRewardLevel = 1;
    private int maxLevel;

    public void Setup()
    {
        gameObject.SetActive(true);
        ShowRewards();
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
        showPrevious.onClick.AddListener(ShowPrevious);
        showNext.onClick.AddListener(ShowNext);
        claimAllButton.onClick.AddListener(ClaimAll);

        DataManager.Instance.PlayerData.UpdatedSnacks += SetupDisplays;

        SetupDisplays();

        foreach (var _reward in DataManager.Instance.GameData.SeasonRewards)
        {
            if (_reward.Level>maxLevel)
            {
                maxLevel = _reward.Level;
            }
        }
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        showPrevious.onClick.RemoveListener(ShowPrevious);
        showNext.onClick.RemoveListener(ShowNext);
        claimAllButton.onClick.RemoveListener(ClaimAll);

        DataManager.Instance.PlayerData.UpdatedSnacks -= SetupDisplays;
    }

    private void SetupDisplays()
    {
        levelDisplay.text = DataManager.Instance.PlayerData.Level.ToString();
        seasonNumberDisplay.text = "Season "+DataManager.Instance.GameData.SeasonNumber;
        ShowSeasonEndDiplay();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void ShowPrevious()
    {
        firstRewardLevel--;
        if (firstRewardLevel < 1)
        {
            firstRewardLevel = 1;
        }
        ShowRewards();
    }

    private void ShowNext()
    {
        firstRewardLevel++;
        if (firstRewardLevel >= maxLevel - 3)
        {
            firstRewardLevel -= 1;
        }

        ShowRewards();
    }

    private void ShowRewards()
    {
        for (int i = firstRewardLevel,j=0; j < 5; i++,j++)
        {
            normalLevelHolders[j].SetupEmpty();
            premiumLevelHolders[j].SetupEmpty();
            levelsDisplay[j].text = i.ToString();

            List <LevelReward> _rewardsOnLevel = new List<LevelReward>();
            foreach (var _reward in DataManager.Instance.GameData.SeasonRewards)
            {
                if (_reward.Level==i)
                {
                    _rewardsOnLevel.Add(_reward);
                }
            }

            foreach (var _reward in _rewardsOnLevel)
            {
                if (_reward.IsPremium)
                {
                    premiumLevelHolders[j].Setup(_reward,_reward.Level);
                }
                else
                {
                    normalLevelHolders[j].Setup(_reward,_reward.Level);
                }
            }
        }
        ShowLevelProgress();
    }

    private void ShowLevelProgress()
    {
        int _progressLevel = DataManager.Instance.PlayerData.Level - firstRewardLevel;
        for (int i = 0; i < levelsBackgroundDisplay.Length; i++)
        {
            levelsBackgroundDisplay[i].sprite = i < _progressLevel ? reachedLevelBackground : notReachedLevelBackground;
        }
        _progressLevel = Mathf.Clamp(_progressLevel, 0, 5);
        if (DataManager.Instance.PlayerData.Level<firstRewardLevel)
        {
            progressDispaly.fillAmount = 0;
        }
        else if (DataManager.Instance.PlayerData.Level==firstRewardLevel)
        {
            progressDispaly.fillAmount = 0.15f;
        }
        else if (DataManager.Instance.PlayerData.Level == firstRewardLevel+1)
        {
            progressDispaly.fillAmount = 0.25f;
        }
        else if (DataManager.Instance.PlayerData.Level == firstRewardLevel+2)
        {
            progressDispaly.fillAmount = 0.50f;
        }
        else if (DataManager.Instance.PlayerData.Level == firstRewardLevel+3)
        {
            progressDispaly.fillAmount = 0.75f;
        }
        else if (DataManager.Instance.PlayerData.Level == firstRewardLevel+4)
        {
            progressDispaly.fillAmount = 0.90f;
        }
        else
        {
            progressDispaly.fillAmount = 1;
        }
    }

    private void Update()
    {
        ShowSeasonEndDiplay();
    }

    private void ShowSeasonEndDiplay()
    {
        if (DataManager.Instance.GameData.HasSeasonEnded)
        {
            seasonEndDisplay.text = "Ended";
        }
        else
        {
            TimeSpan _seasonEndsIn = DataManager.Instance.GameData.SeasonEnds - DateTime.UtcNow;
            if (_seasonEndsIn.TotalDays>1)
            {
                seasonEndDisplay.text = $"Season ends: {(int)_seasonEndsIn.TotalDays}days";
            }
            else if (_seasonEndsIn.Hours>1)
            {
                seasonEndDisplay.text = $"Season ends: {(int)_seasonEndsIn.TotalDays}hours";
            }
            else if (_seasonEndsIn.Minutes>1)
            {
                seasonEndDisplay.text = $"Season ends: {(int)_seasonEndsIn.Minutes}minutes";
            }
            else
            {
                seasonEndDisplay.text = $"Season ends: {(int)_seasonEndsIn.Seconds}secounds";
            }
        }
    }

    private void ClaimAll()
    {
        for (int i = 0; i < normalLevelHolders.Length; i++)
        {
            if (normalLevelHolders[i].CanClaim)
            {
                LevelRewardPanel.OnClosePressed += ClaimNext;
                normalLevelHolders[i].ClaimReward();
                return;
            }
        }

        for (int i = 0; i < premiumLevelHolders.Length; i++)
        {
            if (premiumLevelHolders[i].CanClaim)
            {
                LevelRewardPanel.OnClosePressed += ClaimNext;
                premiumLevelHolders[i].ClaimReward();
                return;
            }
        }
    }

    private void ClaimNext()
    {
        StartCoroutine(ClaimNextRoutine());
        
       IEnumerator ClaimNextRoutine()
        {
            yield return new WaitForSeconds(1);
            LevelRewardPanel.OnClosePressed -= ClaimNext;
            ClaimAll();
        }
    }
}
