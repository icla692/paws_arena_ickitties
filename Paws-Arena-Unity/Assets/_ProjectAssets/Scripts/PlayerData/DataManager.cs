using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerData PlayerData { get; private set; }
    public GameData GameData { get; private set; }

    private const string SNACKS = "Snacks";
    private const string JUG_OF_MILK = "JugOfMilk";
    private const string GLASS_OF_MILK = "GlassOfMilk";
    private const string COMMON_CRISTAL = "Crystals/CommonCrystal";
    private const string UNCOMMON_CRISTAL = "Crystals/UncommonCrystal";
    private const string RARE_CRISTAL = "Crystals/RareCrystal";
    private const string EPIC_CRISTAL = "Crystals/EpicCrystal";
    private const string LEGENDARY_CRISTAL = "Crystals/LegendaryCrystal";
    private const string CRAFTING_PROCESS = "CraftingProcess";
    private const string EXPERIENCE = "Experience";
    private const string CLAIMED_LEVELS = "ClaimedLevelRewards";
    private const string HAS_PASS = "HasPass";
    private const string RECOVERING_KITTIES = "RecoveringKitties";
    private const string OWNED_EQUIPTABLES = "OwnedEquiptables";
    private const string SEASON_NUMBER = "SeasonNumber";
    private const string OWNED_EMOJIS= "OwnedEmojis";
    private const string CHALLENGES= "Challenges";
    private const string CHALLENGES_DATA= "Challenges/ChallengesData";
    private const string GOT_LUCKY_SPIN = "Challenges/ClaimedLuckySpin";
    private const string GUILD = "GuildId";

    private bool hasSubscribed = false;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerData(string _data)
    {
        PlayerData = JsonConvert.DeserializeObject<PlayerData>(_data);
        if (PlayerData.SeasonNumber!=GameData.SeasonNumber)
        {
            PlayerData.SeasonNumber = GameData.SeasonNumber;
            PlayerData.Experience = 0;
            PlayerData.ClaimedLevelRewards.Clear();
            PlayerData.HasPass = false;
            SaveClaimedLevels();
            SaveExp();
            SaveHasPass();
            SaveSeasonNumber();
        }
        
    }

    public void SetGuildsData(Dictionary<string, GuildData> _guilds)
    {
        GameData.Guilds = _guilds;
        if (GameData.Guilds==null)
        {
            GameData.Guilds = new();
        }
    }

    public void CreatePlayerDataEmpty()
    {
        PlayerData = new PlayerData();
        PlayerData.SetStartingValues();
    }

    public void SetGameData(string _data)
    {
        GameData = JsonConvert.DeserializeObject<GameData>(_data);
    }

    public void SubscribeHandlers()
    {
        hasSubscribed = true;
        PlayerData.UpdatedSnacks += SaveSnacks;
        PlayerData.UpdatedJugOfMilk += SaveJugOfMilk;
        PlayerData.UpdatedGlassOfMilk += SaveGlassOfMilk;
        PlayerData.Crystals.UpdatedCommonCrystal += SaveCommonCristal;
        PlayerData.Crystals.UpdatedUncommonCrystal += SaveUncommonCristal;
        PlayerData.Crystals.UpdatedRareCrystal += SaveRareCristal;
        PlayerData.Crystals.UpdatedEpicCrystal += SaveEpicCristal;
        PlayerData.Crystals.UpdatedLegendaryCrystal += SaveLegendaryCristal;
        PlayerData.UpdatedCraftingProcess += SaveCraftingProcess;
        PlayerData.UpdatedClaimedLevels += SaveClaimedLevels;
        PlayerData.UpdatedHasPass += SaveHasPass;
        PlayerData.UpdatedExp += SaveExp;
        PlayerData.UpdatedRecoveringKitties += SaveRecoveringKittes;
        PlayerData.UpdatedEquiptables += SaveEquiptables;
        PlayerData.UpdatedSeasonNumber += SaveSeasonNumber;
        PlayerData.UpdatedOwnedEmojis += SaveOwnedEmojis;
        ChallengeData.UpdatedProgress += SaveChallengeProgress;
        PlayerData.Challenges.UpdatedClaimedLuckySpin += SaveClaimedLuckySpin;
        PlayerData.UpdatedGuild += SaveGuild;
    }

    private void OnDestroy()
    {
        if (!hasSubscribed)
        {
            return;
        }
        PlayerData.UpdatedSnacks -= SaveSnacks;
        PlayerData.UpdatedJugOfMilk -= SaveJugOfMilk;
        PlayerData.UpdatedGlassOfMilk -= SaveGlassOfMilk;
        PlayerData.Crystals.UpdatedCommonCrystal -= SaveCommonCristal;
        PlayerData.Crystals.UpdatedUncommonCrystal -= SaveUncommonCristal;
        PlayerData.Crystals.UpdatedRareCrystal -= SaveRareCristal;
        PlayerData.Crystals.UpdatedEpicCrystal -= SaveEpicCristal;
        PlayerData.Crystals.UpdatedLegendaryCrystal -= SaveLegendaryCristal;
        PlayerData.UpdatedCraftingProcess -= SaveCraftingProcess;
        PlayerData.UpdatedClaimedLevels -= SaveClaimedLevels;
        PlayerData.UpdatedHasPass -= SaveHasPass;
        PlayerData.UpdatedExp -= SaveExp;
        PlayerData.UpdatedRecoveringKitties -= SaveRecoveringKittes;
        PlayerData.UpdatedEquiptables -= SaveEquiptables;
        PlayerData.UpdatedSeasonNumber -= SaveSeasonNumber;
        PlayerData.UpdatedOwnedEmojis -= SaveOwnedEmojis;
        ChallengeData.UpdatedProgress -= SaveChallengeProgress;
        PlayerData.Challenges.UpdatedClaimedLuckySpin -= SaveClaimedLuckySpin;
        PlayerData.UpdatedGuild -= SaveGuild;
    }

    private void SaveSnacks()
    {
        FirebaseManager.Instance.SaveValue(SNACKS,PlayerData.Snacks);
    }

    private void SaveJugOfMilk()
    {
        FirebaseManager.Instance.SaveValue(JUG_OF_MILK, PlayerData.JugOfMilk);
    }

    private void SaveGlassOfMilk()
    {
        FirebaseManager.Instance.SaveValue(GLASS_OF_MILK, PlayerData.GlassOfMilk);
    }

    private void SaveCommonCristal()
    {
        FirebaseManager.Instance.SaveValue(COMMON_CRISTAL, PlayerData.Crystals.CommonCrystal);
    }

    private void SaveUncommonCristal()
    {
        FirebaseManager.Instance.SaveValue(UNCOMMON_CRISTAL, PlayerData.Crystals.UncommonCrystal);
    }

    private void SaveRareCristal()
    {
        FirebaseManager.Instance.SaveValue(RARE_CRISTAL, PlayerData.Crystals.RareCrystal);
    }

    private void SaveEpicCristal()
    {
        FirebaseManager.Instance.SaveValue(EPIC_CRISTAL, PlayerData.Crystals.EpicCrystal);
    }

    private void SaveLegendaryCristal()
    {
        FirebaseManager.Instance.SaveValue(LEGENDARY_CRISTAL, PlayerData.Crystals.LegendaryCrystal);
    }

    private void SaveCraftingProcess()
    {
        FirebaseManager.Instance.SaveValue(CRAFTING_PROCESS, JsonConvert.SerializeObject(PlayerData.CraftingProcess));
    }

    private void SaveClaimedLevels()
    {
        FirebaseManager.Instance.SaveValue(CLAIMED_LEVELS, JsonConvert.SerializeObject(PlayerData.ClaimedLevelRewards));
    }

    private void SaveHasPass()
    {
        FirebaseManager.Instance.SaveValue(HAS_PASS, JsonConvert.SerializeObject(PlayerData.HasPass));
    }

    private void SaveExp()
    {
        FirebaseManager.Instance.SaveValue(EXPERIENCE,PlayerData.Experience);
    }

    private void SaveRecoveringKittes()
    {
        FirebaseManager.Instance.SaveValue(RECOVERING_KITTIES, JsonConvert.SerializeObject(PlayerData.RecoveringKitties));
    }

    private void SaveEquiptables()
    {
        FirebaseManager.Instance.SaveValue(OWNED_EQUIPTABLES, JsonConvert.SerializeObject(PlayerData.OwnedEquiptables));
    }

    private void SaveSeasonNumber()
    {
        FirebaseManager.Instance.SaveValue(SEASON_NUMBER,PlayerData.SeasonNumber);
    }

    private void SaveOwnedEmojis()
    {
        FirebaseManager.Instance.SaveValue(OWNED_EMOJIS, JsonConvert.SerializeObject(PlayerData.OwnedEmojis));
    }

    public void SaveChallenges()
    {
        FirebaseManager.Instance.SaveValue(CHALLENGES,JsonConvert.SerializeObject(PlayerData.Challenges));
    }

    public void SaveChallengeProgress(int _id)
    {
        int _childNumber = 0;
        ChallengeData _challengeData = null;
        for (int i = 0; i < PlayerData.Challenges.ChallengesData.Count; i++)
        {
            if (PlayerData.Challenges.ChallengesData[i].Id == _id)
            {
                _challengeData = PlayerData.Challenges.ChallengesData[i];
                _childNumber = i;
            }
        }
        FirebaseManager.Instance.SaveValue(CHALLENGES_DATA+"/"+_childNumber,JsonConvert.SerializeObject(_challengeData));
    }

    private void SaveClaimedLuckySpin()
    {
        FirebaseManager.Instance.SaveValue(GOT_LUCKY_SPIN,JsonConvert.SerializeObject(PlayerData.Challenges.ClaimedLuckySpin));
    }

    private void SaveGuild()
    {
        FirebaseManager.Instance.SaveString(GUILD,PlayerData.GuildId);
    }
}
