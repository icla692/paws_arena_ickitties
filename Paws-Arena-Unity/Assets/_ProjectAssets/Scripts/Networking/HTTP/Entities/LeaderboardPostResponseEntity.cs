using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardPostResponseEntity
{
    [SerializeField]
    public int points;
    [SerializeField]
    public int oldPoints;
    [SerializeField]
    public int gameResultType = -1;
    [SerializeField]
    public string reason = "";
}
