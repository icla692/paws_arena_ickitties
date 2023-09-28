using Anura.ConfigurationModule.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetsController : MonoBehaviour
{
    public Action onBetsRoutineFinished;

    public GameObject prizesWrapper;
    public TMPro.TextMeshProUGUI gamePrizeValue;
    public TMPro.TextMeshProUGUI monthlyPotValue;

    public CoinSpawner player1LocalCoins;
    public CoinSpawner player2LocalCoins;

    public CoinSpawner player1JackpotCoins;
    public CoinSpawner player2JackpotCoins;

    private int betValue;
    private int monthlyPot;

    private void Start()
    {
        prizesWrapper.SetActive(false);
        betValue = ConfigurationManager.Instance.Config.GetBetValue();
        monthlyPot = 54355;
    }

    [ContextMenu("Show Prizes")]
    public void Test_ShowPrizes()
    {
        StartCoroutine(ShowPrizesCoroutine());
    }

    public void ShowPrizes()
    {
        StartCoroutine(ShowPrizesCoroutine());
    }

    private IEnumerator ShowPrizesCoroutine()
    {
        prizesWrapper.SetActive(true);

        AnimationUtils.GrowValue(gamePrizeValue, 0, Mathf.FloorToInt(1.5f * betValue), 2f);
        AnimationUtils.GrowValue(monthlyPotValue, monthlyPot, monthlyPot + Mathf.FloorToInt(0.5f * betValue), 2f);

        yield return new WaitForSeconds(.5f);

        float localDuration = player1LocalCoins.SpawnCoins(5, 0.3f, 1f);
        player2LocalCoins.SpawnCoins(5, 0.3f, 1f);

        yield return new WaitForSeconds(.5f);

        float jackpotDuration = player1JackpotCoins.SpawnCoins(3, 0.3f, 1f);
        player2JackpotCoins.SpawnCoins(3, 0.3f, 1f);

        yield return new WaitForSeconds(Math.Max(localDuration - 0.5f, jackpotDuration));
        onBetsRoutineFinished?.Invoke();
    }
}
