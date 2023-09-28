using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LeaderboardGetResponseEntity
{
    [SerializeField]
    public List<PlayerStatsEntity> leaderboard;
    [SerializeField]
    public string first;
    [SerializeField]
    public string second;
    [SerializeField]
    public string third;
}
