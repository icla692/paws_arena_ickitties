using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    private int seasonNumber;
    private DateTime seasonEnds;
    private int levelBaseExp;
    private int levelBaseScaler;
    private int respinPrice;
    private int glassOfMilkPrice;
    private int jugOfMilkPrice;
    private List<LevelReward> seasonRewards = new ();
    private int guildPrice;
    private int guildMaxPlayers;
    private Dictionary<string, GuildData> guilds = new ();

    public bool HasSeasonEnded => DateTime.UtcNow > SeasonEnds;

    public int SeasonNumber
    {
        get
        {
            return seasonNumber;
        }
        set
        {
            seasonNumber = value;
        }
    }

    public DateTime SeasonEnds
    {
        get
        {
            return seasonEnds;
        }
        set
        {
            seasonEnds = value;
        }
    }

    public int LevelBaseExp
    {
        get
        {
            return levelBaseExp;
        }
        set
        {
            levelBaseExp = value;
        }
    }

    public int LevelBaseScaler
    {
        get
        {
            return levelBaseScaler;
        }
        set
        {
            levelBaseScaler = value;
        }
    }

    public int RespinPrice
    {
        get
        {
            return respinPrice;
        }
        set
        {
            respinPrice = value;
        }
    }

    public int GlassOfMilkPrice
    {
        get
        {
            return glassOfMilkPrice;
        }
        set
        {
            glassOfMilkPrice = value;
        }
    }

    public int JugOfMilkPrice
    {
        get
        {
            return jugOfMilkPrice;
        }
        set
        {
            jugOfMilkPrice = value;
        }
    }

    public List<LevelReward> SeasonRewards
    {
        get => seasonRewards;
        set => seasonRewards = value;
    }

    public int GuildPrice
    {
        get => guildPrice;
        set => guildPrice = value;
    }

    public Dictionary<string, GuildData> Guilds
    {
        get => guilds;
        set => guilds = value;
    }

    public int GuildMaxPlayers
    {
        get=> guildMaxPlayers;
        set => guildMaxPlayers=value;
    }

    public GuildRankingBorders RankingBorders;

}
