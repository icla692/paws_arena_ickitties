using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

[System.Serializable]
public class EquipmentData
{
    public int Id;
    public Sprite Thumbnail;
    public EquipmentRarity Rarity;
}

[Serializable]
public enum EquipmentRarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4
}

[CreateAssetMenu(fileName = "EquipmentsConfig", menuName = "Configurations/EquipmentsConfig", order = 4)]
public class EquipmentsConfig : ScriptableObject
{
    public List<EquipmentData> Eyes;
    public List<EquipmentData> Head;
    public List<EquipmentData> Mouth;
    public List<EquipmentData> Body;

    public List<EquipmentData> TailsOverlay;
    public List<EquipmentData> TailsFloating;
    public List<EquipmentData> TailsAnimated;

    public EquipmentData CraftItem(CraftingRecepieSO _craftingRecepie)
    {
        EquipmentRarity _rarity;
        switch (_craftingRecepie.Inggrdiant)
        {
            case ItemType.Common:
                _rarity = EquipmentRarity.Common;
                break;
            case ItemType.Uncommon:
                _rarity = EquipmentRarity.Uncommon;
                break;
            case ItemType.Rare:
                _rarity = EquipmentRarity.Rare;
                break;
            case ItemType.Epic:
                _rarity = EquipmentRarity.Epic;
                break;
            case ItemType.Legendary:
                _rarity = EquipmentRarity.Legendary;
                break;
            default:
                throw new Exception("Don't know how to create item for receipt with ingredient: " + _craftingRecepie.Inggrdiant);
        }
        
        return CraftItem(_rarity);
    }

    public EquipmentData CraftItem(EquipmentRarity _equipmentRarity)
    {
        EquipmentData _equipmentData;
        while (true)
        {
            _equipmentData = GenerateRandomItem();
            if (_equipmentData.Rarity==_equipmentRarity)
            {
                break;
            }
        }

        return _equipmentData;
    }

    public EquipmentData CraftItem()
    {
        return GenerateRandomItem();
    }

    private EquipmentData GenerateRandomItem()
    {
        List<EquipmentData> _equipments;
        int _random = Random.Range(0, 7);
        switch (_random)
        {
            case 0:
                _equipments = Eyes;
                break;
            case 1:
                _equipments = Head;
                break; 
            case 2:
                _equipments = Mouth; 
                break;
            case 3:
                _equipments = Body;
                break;
            case 4:
                _equipments = TailsAnimated;
                break;
            case 5:
                _equipments = TailsFloating;
                break;
            case  6:
                _equipments = TailsOverlay;
                break;
            default:
                _equipments = Body;
                break;
        }

        return _equipments[Random.Range(0, _equipments.Count)];
    }

    public EquipmentData GetEquipmentData(int _id)
    {
        foreach (var _equipmentData in Eyes)
        {
            if (_equipmentData.Id==_id)
            {
                return _equipmentData;
            }
        }
        
        foreach (var _equipmentData in Head)
        {
            if (_equipmentData.Id==_id)
            {
                return _equipmentData;
            }
        }
        
        foreach (var _equipmentData in Mouth)
        {
            if (_equipmentData.Id==_id)
            {
                return _equipmentData;
            }
        }
        
        foreach (var _equipmentData in Body)
        {
            if (_equipmentData.Id==_id)
            {
                return _equipmentData;
            }
        }
        
        foreach (var _equipmentData in TailsOverlay)
        {
            if (_equipmentData.Id==_id)
            {
                return _equipmentData;
            }
        }
        
        foreach (var _equipmentData in TailsFloating)
        {
            if (_equipmentData.Id==_id)
            {
                return _equipmentData;
            }
        }
        
        foreach (var _equipmentData in TailsAnimated)
        {
            if (_equipmentData.Id==_id)
            {
                return _equipmentData;
            }
        }

        throw new Exception("Cant find item with id: "+_id);
    }
}
