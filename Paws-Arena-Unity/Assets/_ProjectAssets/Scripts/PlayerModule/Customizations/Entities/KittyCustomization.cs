using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KittyCustomization
{
    public Dictionary<EquipmentType, Equipment> playerEquipmentConfig;
    public Dictionary<EquipmentType, Equipment> originalConfig;

    [System.Serializable]
    public class KittyCustomizationSerializable
    {
        [SerializeField]
        public SimpleSerializableDictionary<EquipmentType, Equipment> playerEquipmentConfig;
        [SerializeField]
        public SimpleSerializableDictionary<EquipmentType, Equipment> originalConfig;

        public KittyCustomizationSerializable(KittyCustomization customization)
        {
            playerEquipmentConfig = new SimpleSerializableDictionary<EquipmentType, Equipment>(customization.playerEquipmentConfig);
            originalConfig = new SimpleSerializableDictionary<EquipmentType, Equipment>(customization.originalConfig);
        }


        public KittyCustomization GetNonSerializable()
        {
            KittyCustomization customization = new KittyCustomization();
            customization.playerEquipmentConfig = playerEquipmentConfig.GetDictionary();
            customization.originalConfig = originalConfig.GetDictionary();

            return customization;
        }
    }

    public KittyCustomizationSerializable GetSerializableObject()
    {
        return new KittyCustomizationSerializable(this);
    }
}