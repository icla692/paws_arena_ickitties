using Anura.ConfigurationModule.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class KittiesCustomizationService
{
    public static KittiesCustomizations config;
    private static string CONFIGURATIONS_KEY = "KITTIES_CONFIGURATIONS";

    static KittiesCustomizationService()
    {
        config = new KittiesCustomizations();

        if (PlayerPrefs.HasKey(CONFIGURATIONS_KEY))
        {
            string configurationsJson = PlayerPrefs.GetString(CONFIGURATIONS_KEY);

            config = KittiesCustomizations.GetFromJson(configurationsJson);
            if (ConfigurationManager.Instance.GameConfig.enableDevLogs)
            {
                Debug.Log("Loading " + configurationsJson);
                Debug.Log("Got " + config.customizationByCatUrl.Count + " configurations");
            }
        }
    }

    public static KittyCustomization GetCustomization(string url)
    {
        var customizations = config.customizationByCatUrl;
        if (customizations.ContainsKey(url))
        {
            return customizations[url];
        }

        return null;
    }

    public static void SaveCustomConfig(string url, Dictionary<EquipmentType, Equipment> playerEquipmentConfig)
    {

        if (config.customizationByCatUrl.ContainsKey(url))
        {
            config.customizationByCatUrl[url].playerEquipmentConfig = playerEquipmentConfig;
        }
        else
        {
            KittyCustomization customization = new KittyCustomization()
            {
                playerEquipmentConfig = playerEquipmentConfig
            };

            config.customizationByCatUrl.Add(url, customization);
        }

        PlayerPrefs.SetString(CONFIGURATIONS_KEY, config.Serialize());
    }

    public static KittyCustomization SaveOriginalConfig(string url, Dictionary<EquipmentType, Equipment> playerEquipmentConfig)
    {
        if (config.customizationByCatUrl.ContainsKey(url))
        {
            config.customizationByCatUrl[url].originalConfig = playerEquipmentConfig;
        }
        else
        {
            KittyCustomization customization = new KittyCustomization()
            {
                originalConfig = playerEquipmentConfig
            };

            config.customizationByCatUrl.Add(url, customization);
        }
        return config.customizationByCatUrl[url];
    }

    public static void RemoveCustomizations(string url)
    {
        if (config.customizationByCatUrl.ContainsKey(url))
        {
            config.customizationByCatUrl[url].playerEquipmentConfig = null;
        }
    }
}