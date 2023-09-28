using System;
using Newtonsoft.Json;

[Serializable]
public class GuildPlayerData
{
    public string Id;
    public string Name;
    public int Points;
    public int Level;
    public bool IsLeader;
    [JsonIgnore] public int Place;
}
