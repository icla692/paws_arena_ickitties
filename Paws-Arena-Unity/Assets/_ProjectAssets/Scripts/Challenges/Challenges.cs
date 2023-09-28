using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class Challenges
{
    private List<ChallengeData> challengesData = new List<ChallengeData>();
    private DateTime nextReset = DateTime.MinValue;
    private bool claimedLuckySpin;

    [JsonIgnore] public Action UpdatedClaimedLuckySpin;

    public List<ChallengeData> ChallengesData
    {
        get => challengesData;
        set=>  challengesData = value;
    }

    public DateTime NextReset
    {
        get => nextReset;
        set => nextReset = value;
    }

    public bool ClaimedLuckySpin
    {
        get => claimedLuckySpin;
        set
        {
            claimedLuckySpin = value;
            UpdatedClaimedLuckySpin?.Invoke();
        }
    }
}
