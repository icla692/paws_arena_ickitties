using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengesPanel : MonoBehaviour
{
    public static  Action OnClosed;
    [SerializeField] private ChallengeDisplay[] challengeDisplays;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI progressDisplay;
    [SerializeField] private TextMeshProUGUI timerDisplay;
    [SerializeField] private LuckyWheelUI luckyWheel;

    public void Setup()
    {
        int _completedChallenges = 0;
        for (int i = 0; i < challengeDisplays.Length; i++)
        {
            ChallengeData _challengeData = DataManager.Instance.PlayerData.Challenges.ChallengesData[i];
            challengeDisplays[i].Setup(_challengeData);
            if (_challengeData.Claimed)
            {
                _completedChallenges++;
            }
        }

        int _totalAmountOfChellenges = DataManager.Instance.PlayerData.Challenges.ChallengesData.Count;
        
        progressDisplay.text = $"{_completedChallenges}/{_totalAmountOfChellenges} Completed";
        gameObject.SetActive(true);
        StartCoroutine(ShowTimer());
        if (_completedChallenges==_totalAmountOfChellenges&& !DataManager.Instance.PlayerData.Challenges.ClaimedLuckySpin)
        {
            luckyWheel.RequestReward();
            luckyWheel.ShowReward();
            DataManager.Instance.PlayerData.Challenges.ClaimedLuckySpin = true;
        }
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        OnClosed?.Invoke();
        gameObject.SetActive(false);
    }

    private IEnumerator ShowTimer()
    {
        while (gameObject.activeSelf)
        {
            TimeSpan _timeLeft = DataManager.Instance.PlayerData.Challenges.NextReset - DateTime.UtcNow;
            string _output = string.Empty;
            _output += _timeLeft.Hours < 10 ? "0" + _timeLeft.Hours : _timeLeft.Hours;
            _output += "h ";
            _output += _timeLeft.Minutes < 10 ? "0" + _timeLeft.Minutes : _timeLeft.Minutes;
            _output += "m ";
            _output += _timeLeft.Seconds < 10 ? "0" + _timeLeft.Seconds : _timeLeft.Seconds;
            _output += "s Remaining";
            timerDisplay.text = _output;
            yield return new WaitForSeconds(1);
        }
    }
}
