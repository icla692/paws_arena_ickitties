using Anura.Templates.MonoSingleton;
using UnityEngine;
using System;
using Anura.ConfigurationModule.Managers;
using Photon.Pun;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public event Action<int> onHealthUpdated;
    public event Action<int> onDamageTaken;

    [SerializeField]
    private GameObject player1SpawnSquare;
    [SerializeField]
    private GameObject player2SpawnSquare;

    [HideInInspector]
    public PlayerComponent myPlayer; 
    [HideInInspector]
    public PlayerComponent OtherPlayerComponent;
    [HideInInspector]
    public int myPlayerHealth;
    [HideInInspector]
    public int otherPlayerHealth = int.MaxValue;
    [HideInInspector]
    public Transform otherPlayerTransform;

    private int maxHP;

    public static int HealthAtEnd;
    

    public void RegisterMyPlayer(PlayerComponent playerComponent)
    {
        myPlayer = playerComponent;
        myPlayer.GetComponent<BasePlayerComponent>().onDamageTaken += OnDamageTaken;

        maxHP = ConfigurationManager.Instance.Config.GetPlayerTotalHealth();
        SetMyPlayerHealth(maxHP);
    }

    private void OnDisable()
    {
        try
        {

        }
        catch
        {
            myPlayer.GetComponent<BasePlayerComponent>().onDamageTaken -= OnDamageTaken;
        }
    }

    private void OnDestroy()
    {
        HealthAtEnd = myPlayerHealth;
        // float _minutesItWillTakeToRecover =((float)RecoveryHandler.RecoveryInMinutes / maxHP) * (maxHP - myPlayerHealth);
        // DateTime _recoveryEnds = DateTime.UtcNow.AddMinutes(_minutesItWillTakeToRecover);
        // GameState.selectedNFT.RecoveryEndDate = _recoveryEnds;
        // RecoveryEntrie _recoveryEntry = new RecoveryEntrie()
        // {
        //     EndDate = _recoveryEnds,
        //     KittyImageUrl = GameState.selectedNFT.imageUrl
        // };
        // DataManager.Instance.PlayerData.AddRecoveringKittie(_recoveryEntry);
    }

    [ContextMenu("Test_Take50Damage")]
    public void TEST_TakeDamage()
    {
        OnDamageTaken(50);
    }

    private void OnDamageTaken(int damage)
    {
        if (ConfigurationManager.Instance.GameConfig.enableDevLogs)
        {
            Debug.Log($"Got damage {myPlayerHealth} - {damage} = {myPlayerHealth - damage}");
        }

        SetMyPlayerHealth(myPlayerHealth - damage);
    }

    public GameResolveState GetWinnerByDeath()
    {
        bool isPlayer1 = CheckIAmPlayer1();
        if (myPlayerHealth > 0 && otherPlayerHealth > 0)
        {
            return GameResolveState.NO_WIN;
        }
        else if (myPlayerHealth <= 0 && otherPlayerHealth <= 0)
        {
            return GameResolveState.DRAW;
        }
        else if ((myPlayerHealth > 0 && isPlayer1) || (otherPlayerHealth > 0 && !isPlayer1))
        {
            return GameResolveState.PLAYER_1_WIN;
        }
        else return GameResolveState.PLAYER_2_WIN;
    }

    private bool CheckIAmPlayer1()
    {
        if(ConfigurationManager.Instance.Config.GetGameType() == Anura.ConfigurationModule.ScriptableObjects.GameType.TUTORIAL 
            || ConfigurationManager.Instance.Config.GetGameType() == Anura.ConfigurationModule.ScriptableObjects.GameType.SINGLEPLAYER)
        {
            return true;
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            return true;
        }

        return false;
    }

    public GameResolveState GetWinnerByHealth()
    {
        if (myPlayerHealth > otherPlayerHealth)
        {
            return GameResolveState.PLAYER_1_WIN;
        }
        else if (myPlayerHealth < otherPlayerHealth)
        {
            return GameResolveState.PLAYER_2_WIN;
        }
        else return GameResolveState.DRAW;
    }

    public GameResolveState GetWinnerByLoserIndex(int idx)
    {
        if (idx == 0)
        {
            return GameResolveState.PLAYER_2_WIN;
        }
        else return GameResolveState.PLAYER_1_WIN;
    }

    private void SetMyPlayerHealth(int value)
    {
        value = Math.Max(0, value);
        value = Math.Min(maxHP, value);

        myPlayerHealth = value;
        onHealthUpdated?.Invoke(myPlayerHealth);
    }

    public void Heal(int healValue)
    {
        Debug.Log($"Healing: {healValue}. New HP: {myPlayerHealth + healValue}");
        SetMyPlayerHealth(myPlayerHealth + healValue);
    }


    public void DirectDamage(int damage)
    {
        SetMyPlayerHealth(myPlayerHealth - damage);
    }

    public Vector2 GetPlayer1SpawnPos()
    {
        return GetRandomPosInSquare(player1SpawnSquare);
    }

    public Vector2 GetPlayer2SpawnPos()
    {
        return GetRandomPosInSquare(player2SpawnSquare);
    }

    private Vector2 GetRandomPosInSquare(GameObject square)
    {
        Vector2 minRange = square.transform.position - square.transform.lossyScale / 2.0f;
        Vector2 maxRange = square.transform.position + square.transform.lossyScale / 2.0f;
        float xPos = UnityEngine.Random.Range(minRange.x, maxRange.x);
        float yPos = UnityEngine.Random.Range(minRange.y, maxRange.y);
        return new Vector2(xPos, yPos);
    }
}
