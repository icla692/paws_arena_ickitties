using System;
using UnityEngine;
using Anura.ConfigurationModule.Managers;
using Photon.Pun;

public class DamageDealingDisplay : MonoBehaviour
{
    public static Action<int> OnExpEarned;
    public GameObject damageDealPrefab;
    public BasePlayerComponent basePlayerComponent;
    [SerializeField] private GameObject experiencePrefab;
    private bool isBotPlayer;
    private PhotonView photonView;
    private Vector3 damageOffset = new Vector3(0, 1, 0);
    private int amountOfShowingDamageTexts = 0;

    private void OnEnable()
    {
        isBotPlayer = GetComponentInParent<BotPlayerComponent>();
        if (PhotonNetwork.CurrentRoom.PlayerCount==2)
        {
            photonView = GetComponent<PhotonView>();
        }

        basePlayerComponent.onDamageTaken += OnDamageTaken;
        DamageDealingText.Finished += DeduceAmountOfTexts;
    }

    private void OnDisable()
    {
        basePlayerComponent.onDamageTaken -= OnDamageTaken;
        DamageDealingText.Finished -= DeduceAmountOfTexts;
    }

    private void DeduceAmountOfTexts()
    {
        amountOfShowingDamageTexts--;
    }

    private void OnDamageTaken(int damage)
    {
        amountOfShowingDamageTexts++;
        var go = GameObject.Instantiate(damageDealPrefab, transform.position, Quaternion.identity, null);
        go.transform.localPosition += (amountOfShowingDamageTexts * damageOffset);
        go.transform.GetChild(0).position = new Vector2(UnityEngine.Random.Range(-2.0f, 2.0f), 0);
        go.transform.GetChild(0).GetComponent<DamageDealingText>().Init(damage);
        SpawnExperience(damage);
    }

    private void SpawnExperience(int _damageTaken)
    {
        if (DataManager.Instance.GameData.HasSeasonEnded)
        {
            return;
        }

        if (photonView!=null)
        {
            if (photonView.IsMine)
            {
                return;
            }
        }
        else
        {
            if (!isBotPlayer)
            {
                return;
            }
        }

        DataManager.Instance.PlayerData.Experience += _damageTaken;
        EventsManager.OnGotExperience?.Invoke(_damageTaken);
        EventsManager.OnDealtDamageToOpponent?.Invoke(_damageTaken);
        for (int i = 0; i < _damageTaken; i += 5)
        {
            GameObject _experience = Instantiate(experiencePrefab);
            _experience.transform.position = transform.position;
        }
    }
    
}
