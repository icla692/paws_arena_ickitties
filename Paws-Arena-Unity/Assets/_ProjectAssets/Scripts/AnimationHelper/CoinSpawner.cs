using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    private RectTransform sourceParent;
    [SerializeField]
    private RectTransform targetParent;
    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private int maxCoinNumber = 5;

    private List<GameObject> coinPool;
    private void Start()
    {
        coinPool = new List<GameObject>();

        CreateCoinsInPool();
    }

    [ContextMenu("SpawnCoins")]
    public void Test_SpawnCoins()
    {
        StartCoroutine(SpawnCoinsCoroutine(5, 0.3f, 1f));
    }


    public float SpawnCoins(int count, float delay, float travelDuration)
    {
        StartCoroutine(SpawnCoinsCoroutine(count, delay, travelDuration));
        return count * delay + travelDuration;
    }
    private IEnumerator SpawnCoinsCoroutine(int count, float delay, float travelDuration)
    {
        for (int i = 0; i < count; i++)
        {
            coinPool[i].SetActive(true);
            coinPool[i].GetComponent<RectTransform>().SetParent(targetParent, true);
            SFXManager.Instance.PlayOneShot(ConfigurationManager.Instance.SFXConfig.GetCoinSound());

            GameObject myCoin = coinPool[i];
            LeanTween.move(myCoin.GetComponent<RectTransform>(), Vector2.zero, travelDuration).setEaseInOutExpo().setOnComplete(() =>
            {
                InitCoin(myCoin);
            });
            yield return new WaitForSeconds(delay);
        }
    }

    private void CreateCoinsInPool()
    {
        for (int i = 0; i < maxCoinNumber; i++)
        {
            GameObject go = Instantiate(coinPrefab, sourceParent.GetComponent<RectTransform>());
            InitCoin(go);
            coinPool.Add(go);
        }
    }

    private void InitCoin(GameObject go)
    {
        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.SetParent(sourceParent, true);
        rectTransform.anchoredPosition = Vector3.zero;
        go.SetActive(false);
    }
}
