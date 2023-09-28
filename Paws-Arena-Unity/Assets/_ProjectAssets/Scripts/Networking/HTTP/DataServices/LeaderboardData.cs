using Anura.ConfigurationModule.Managers;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardData : MonoBehaviour
{
    [HideInInspector]
    public LeaderboardGetResponseEntity leaderboard = null;
    public async UniTask<LeaderboardGetResponseEntity> GetLeaderboard()
    {
        if (leaderboard != null && leaderboard.leaderboard.Count > 0)
        {
            return leaderboard;
        }

        Debug.Log("[HTTP] Grabbing leaderboard...");
        string resp = await NetworkManager.GETRequestCoroutine("/leaderboard",
            (code, err) =>
            {
                Debug.LogWarning("Couldn't retrieve Leaderboard!");
            });

        if (string.IsNullOrEmpty(resp)) return null;


        if (ConfigurationManager.Instance.GameConfig.enableDevLogs)
        {
            Debug.Log(resp);
        }
        leaderboard = JsonUtility.FromJson<LeaderboardGetResponseEntity>(resp);

        Debug.Log($"[HTTP] Grabbed {leaderboard.leaderboard.Count} players...");
        return leaderboard;
    }
}
