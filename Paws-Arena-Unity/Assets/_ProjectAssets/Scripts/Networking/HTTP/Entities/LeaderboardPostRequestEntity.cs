using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatchStatus
{
    MatchStartedForPlayer1 = 1,//Hp=null, Winner=null
    MatchStartedForPlayer2 = 2,//Hp=null, Winner=null
    MatchStartedForBothPlayers = 3,//
    MatchFinishedForPlayer1 = 4,//Hp=<int>, Winner=0/1/2
    MatchFinishedForPlayer2 = 5,//Hp=<int>, Winner=0/1/2
    MatchFinishedForBothPlayers = 6,
    MatchInvalid = 7
}

public enum GameResult
{
    Draw = 0,
    Player1 = 1,
    Player2 = 2
}

[System.Serializable]
public class LeaderboardPostRequestEntity
{
    [SerializeField]
    public string matchId;
    [SerializeField]
    public string kittyUrl;
    [SerializeField]
    public GameResult winner;
    [SerializeField]
    public int hp;
    [SerializeField]
    public MatchStatus status;

}
