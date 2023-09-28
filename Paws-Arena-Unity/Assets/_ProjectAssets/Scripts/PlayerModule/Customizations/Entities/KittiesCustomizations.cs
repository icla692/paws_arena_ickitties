using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KittiesCustomizations
{
    public Dictionary<string, KittyCustomization> customizationByCatUrl;

    [System.Serializable]
    public class KittiesCustomizationsSerializable
    {
        [SerializeField]
        public SimpleSerializableDictionary<string, KittyCustomization.KittyCustomizationSerializable> customizationByCatUrl;

        public KittiesCustomizationsSerializable(KittiesCustomizations customizations)
        {
            Dictionary<string, KittyCustomization.KittyCustomizationSerializable> serializedKittyCustomizations = new Dictionary<string, KittyCustomization.KittyCustomizationSerializable>();
            foreach (var kittyCustomization in customizations.customizationByCatUrl)
            {
                //Skip non-customized kitties
                if (kittyCustomization.Value.playerEquipmentConfig == null || kittyCustomization.Value.playerEquipmentConfig.Count == 0) continue;

                serializedKittyCustomizations.Add(kittyCustomization.Key, kittyCustomization.Value.GetSerializableObject());
            }

            customizationByCatUrl = new SimpleSerializableDictionary<string, KittyCustomization.KittyCustomizationSerializable>(serializedKittyCustomizations);
        }

        public KittiesCustomizations GetNonSerializable()
        {
            KittiesCustomizations customizations = new KittiesCustomizations();
            Dictionary<string, KittyCustomization> nonSerializableDict = new Dictionary<string, KittyCustomization>();

            for (int i = 0; i < customizationByCatUrl.keys.Count; i++)
            {
                nonSerializableDict.Add(customizationByCatUrl.keys[i], customizationByCatUrl.values[i].GetNonSerializable());
            }

            customizations.customizationByCatUrl = nonSerializableDict;

            return customizations;
        }
    }

    public KittiesCustomizations()
    {
        customizationByCatUrl = new Dictionary<string, KittyCustomization>();
    }

    public static KittiesCustomizations GetFromJson(string json)
    {
        KittiesCustomizationsSerializable serializable = JsonUtility.FromJson<KittiesCustomizationsSerializable>(json);
        return serializable.GetNonSerializable();
    }

    public string Serialize()
    {
        KittiesCustomizationsSerializable serializable = new KittiesCustomizationsSerializable(this);
        return JsonUtility.ToJson(serializable);
    }
}