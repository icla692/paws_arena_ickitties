using Anura.ConfigurationModule.Managers;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static BotAIAim;
using Random = UnityEngine.Random;

[Serializable]
public struct WeaponConfigOverride
{
    public Weapon weapon;
    public int weight;
}

public enum BotAccuracyMode
{
    Percentage,
    MaxMargin,
    Alternating
}

public class BotPreset : MonoBehaviour
{
    [Tooltip("Affects how the 'accuracy' parameter is used.\n" +
        "[Percentage]: The 'accuracy' parameter represents a % chance of hitting the target. When not hitting, the misses will be proportionally bad to the accuracy.\n" +
        "[MaxMargin]: Always shoots randomly within a defined margin. The size of the margin is proportional to the 'accuracy' parameter.\n" +
        "[Alternating]: An alternating combination of [Percentage] and [MaxMargin].")]
    public BotAccuracyMode accuracyMode;

    [Range(0, 100)]
    public int accuracy = 100;

    [Header("Mode: Percentage")]
    [Tooltip("The bot has an 'accuracy'% chance of shooting within this power margin. " +
        "For low values, this will always result in a direct hit.\n")]
    public float botAccuracyPowerMargin = 0.005f;

    [Header("Mode: MaxMargin")]
    [Tooltip("The bot will always shoot within a random margin of (100 - 'accuracy')% of this.")]
    public float botAccuracyPowerMarginMax = 0.15f;    

    [Header("Set fields to -1 to inherit from default configuration.")]
    public BotConfiguration configurationOverrides;

    public List<WeaponConfigOverride> weaponOverrides;

    public void Setup(BotConfiguration configuration, ref List<WeaponData> weaponData)
    {
        if (ConfigurationManager.Instance.GameConfig.enableDevLogs)
        {
        }
        if (configurationOverrides != null)
        {
            foreach (var field in typeof(BotConfiguration).GetFields())
            {
                string ovrride = field.GetValue(configurationOverrides).ToString();
                if (ovrride == "-1")
                {
                    var dfault = field.GetValue(configuration);
                    field.SetValue(configurationOverrides, dfault);
                }
            }
        }

        if (weaponOverrides != null)
        {
            for (int w = 0; w < weaponOverrides.Count; w++)
            {
                if (weaponData != null)
                {
                    WeaponData? existing = null;
                    int index = 0;
                    for (int i = 0; i < weaponData.Count; i++)
                    {
                        if (weaponData[i].weapon == weaponOverrides[w].weapon)
                        {
                            existing = weaponData[i];
                            index = i;
                        }
                    }

                    if (existing is WeaponData wd)
                    {
                        wd.weight = weaponOverrides[w].weight;
                        weaponData[index] = wd;
                    }
                }
            }
        }
    }

    private bool alternating = false;
    public float GetPowerAccuracyPenalty(float targetPower)
    {
        if (accuracyMode == BotAccuracyMode.Percentage)
        {
            return GetPowerAccuracyPenalty_Percentage(targetPower);
        }
        else if (accuracyMode == BotAccuracyMode.MaxMargin)
        {
            return GetPowerAccuracyPenalty_MaxMargin(targetPower);
        }
        else if (accuracyMode == BotAccuracyMode.Alternating)
        {
            alternating = !alternating;
            if (alternating) return GetPowerAccuracyPenalty_Percentage(targetPower);
            else return GetPowerAccuracyPenalty_MaxMargin(targetPower);
        }
        return 0;
    }

    private float GetPowerAccuracyPenalty_Percentage(float targetPower)
    {
        if (accuracy == 100) return 0;

        List<float> possibleResults = new List<float>();
        int lowerPowers = (100 - accuracy) / 2;
        int higherPowers = (100 - accuracy) - lowerPowers;

        int extraLowerPowers = 0;
        int extraHigherPowers = 0;
        for (int i = 2; i <= lowerPowers + 1; i++)
            if (targetPower - (i * botAccuracyPowerMargin) < 0)
                extraHigherPowers++;
        for (int i = 2; i <= higherPowers + 1; i++)
            if (targetPower + (i * botAccuracyPowerMargin) > 1)
                extraLowerPowers++;

        lowerPowers -= extraHigherPowers;
        lowerPowers += extraLowerPowers;
        higherPowers -= extraLowerPowers;
        higherPowers += extraHigherPowers;

        for (int i = 2; i <= lowerPowers + 1; i++)
            possibleResults.Add(-i * botAccuracyPowerMargin);
        for (int i = 2; i <= higherPowers + 1; i++)
            possibleResults.Add(i * botAccuracyPowerMargin);

        int remaining = 100 - lowerPowers - higherPowers;
        for (int i = 0; i < remaining; i++)
            possibleResults.Add(Mathf.Lerp(-botAccuracyPowerMargin, botAccuracyPowerMargin, Mathf.InverseLerp(0, remaining - 1, i)));

        return possibleResults[Random.Range(0, possibleResults.Count)];
    }

    private float GetPowerAccuracyPenalty_MaxMargin(float targetPower)
    {
        if (accuracy == 100) return 0;

        float margin = botAccuracyPowerMarginMax * ((100 - accuracy) / 100f);

        float lowerAddon = targetPower - margin;
        float higherAddon = targetPower + margin - 1;

        float marginMin = margin;
        float marginMax = margin;

        if (lowerAddon < 0) marginMax += -lowerAddon;
        if (higherAddon > 0) marginMin -= higherAddon;

        return Random.Range(-marginMin, marginMax);
    }
}