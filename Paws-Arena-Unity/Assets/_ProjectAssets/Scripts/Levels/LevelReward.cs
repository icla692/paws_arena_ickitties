using UnityEngine;
using System;

[Serializable]
public class LevelReward
{
    public int Level;
    public string Name;
    public LevelRewardType Type;
    public bool IsPremium;
    public int Parameter1;

    public void Claim()
    {
        switch (Type)
        {
            case LevelRewardType.CommonShard:
                DataManager.Instance.PlayerData.Crystals.CommonCrystal += Parameter1;
                break;
            case LevelRewardType.UncommonShard:
                DataManager.Instance.PlayerData.Crystals.UncommonCrystal += Parameter1;
                break;
            case LevelRewardType.RareShard:
                DataManager.Instance.PlayerData.Crystals.RareCrystal += Parameter1;
                break;
            case LevelRewardType.EpicShard:
                DataManager.Instance.PlayerData.Crystals.EpicCrystal += Parameter1;
                break;
            case LevelRewardType.LegendaryShard:
                DataManager.Instance.PlayerData.Crystals.LegendaryCrystal += Parameter1;
                break;
            case LevelRewardType.Snack:
                DataManager.Instance.PlayerData.Snacks += Parameter1;
                break;
            case LevelRewardType.JugOfMilk:
                DataManager.Instance.PlayerData.JugOfMilk += Parameter1;
                break;
            case LevelRewardType.GlassOfMilk:
                DataManager.Instance.PlayerData.GlassOfMilk += Parameter1;
                break;
            case LevelRewardType.Item:
                DataManager.Instance.PlayerData.AddOwnedEquipment(Parameter1);
                break;
            case LevelRewardType.Emote:
                DataManager.Instance.PlayerData.AddOwnedEmoji(Parameter1);
                break;
            default:
                throw new Exception("Dont know how to claim level reward of type: " + Type);
        }
    }
}
