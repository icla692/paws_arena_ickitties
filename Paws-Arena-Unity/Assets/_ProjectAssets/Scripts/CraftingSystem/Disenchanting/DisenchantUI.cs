using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisenchantUI : MonoBehaviour
{
    [SerializeField] private List<DisenchantmentItemBackground> backgrounds;
    [SerializeField] private Sprite[] crystalSprites;
    [SerializeField] private Image itemBackground;
    [SerializeField] private Button disenchantButton;
    [SerializeField] private Image selectedItemDisplay;
    [SerializeField] private DisenchantmentItemDisplay itemPrefab;
    [SerializeField] private Transform itemsHolder;
    [SerializeField] private DisenchantedItemDisplay rewardDisplay;
    [SerializeField] private Button upArrow;
    [SerializeField] private Button downArrow;
    [SerializeField] private TextMeshProUGUI rewardText;
    
    [Space] [SerializeField] private EquipmentsConfig equipments;

    private List<DisenchantmentItemDisplay> shownItems = new List<DisenchantmentItemDisplay>();
    private EquipmentData selectedEquipment;
    private float moveAmount = 1;
    
    public void Setup()
    {
        HideRightUI();
        ShowItems();
        gameObject.SetActive(true);
    }

    private void ShowItems()
    {
        foreach (var _ownedEquiptableId in DataManager.Instance.PlayerData.OwnedEquiptables)
        {
            EquipmentData _equipmentData = equipments.GetEquipmentData(_ownedEquiptableId);
            if (_equipmentData.Thumbnail.name=="none")
            {
                continue;
            }
            DisenchantmentItemDisplay _item = Instantiate(itemPrefab, itemsHolder);
            _item.Setup(_equipmentData);
            shownItems.Add(_item);
        }
    }

    private void OnEnable()
    {
        upArrow.onClick.AddListener(MoveContentUp);
        downArrow.onClick.AddListener(MoveContentDown);
        DisenchantmentItemDisplay.OnEquipmentClicked += ShowEquipment;
        disenchantButton.onClick.AddListener(Disenchant);
    }

    private void OnDisable()
    {
        upArrow.onClick.RemoveListener(MoveContentUp);
        downArrow.onClick.RemoveListener(MoveContentDown);
        DisenchantmentItemDisplay.OnEquipmentClicked += ShowEquipment;
        disenchantButton.onClick.RemoveListener(Disenchant);
    }

    private void MoveContentUp()
    {
        Vector3 _itemsPosition = itemsHolder.transform.position;
        _itemsPosition.y -= moveAmount;
        itemsHolder.transform.position = _itemsPosition;
    }

    private void MoveContentDown()
    {
        Vector3 _itemsPosition = itemsHolder.transform.position;
        _itemsPosition.y += moveAmount;
        itemsHolder.transform.position = _itemsPosition;
    }

    private void ShowEquipment(EquipmentData _equipmentData)
    {
        selectedEquipment = _equipmentData;
        itemBackground.sprite = backgrounds.Find(_element => _element.Rarity == selectedEquipment.Rarity).Background;
        selectedItemDisplay.sprite = selectedEquipment.Thumbnail;
        itemBackground.gameObject.SetActive(true);
        disenchantButton.gameObject.SetActive(true);
        selectedItemDisplay.gameObject.SetActive(true);
        CraftingRecepieSO _craftingSO = null;
        switch (_equipmentData.Rarity)
        {
            case EquipmentRarity.Common:
                _craftingSO = CraftingRecepieSO.Get(ItemType.Common);
                break;
            case EquipmentRarity.Uncommon:
                _craftingSO = CraftingRecepieSO.Get(ItemType.Uncommon);
                break;
            case EquipmentRarity.Rare:
                _craftingSO = CraftingRecepieSO.Get(ItemType.Rare);
                break;
            case EquipmentRarity.Epic:
                _craftingSO = CraftingRecepieSO.Get(ItemType.Epic);
                break;
            case EquipmentRarity.Legendary:
                _craftingSO = CraftingRecepieSO.Get(ItemType.Legendary);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        rewardText.text = $"Get 1 <color={_craftingSO.IngridiantColor}>{_equipmentData.Rarity}</color> shard by from disenchanting";

    }

    private void Disenchant()
    {
        HideRightUI();
        switch (selectedEquipment.Rarity)
        {
            case EquipmentRarity.Common:
                DataManager.Instance.PlayerData.Crystals.CommonCrystal += 1;
                rewardDisplay.Setup(crystalSprites[0]);
                break;
            case EquipmentRarity.Uncommon:
                DataManager.Instance.PlayerData.Crystals.UncommonCrystal += 1;
                rewardDisplay.Setup(crystalSprites[1]);
                break;
            case EquipmentRarity.Rare:
                DataManager.Instance.PlayerData.Crystals.RareCrystal += 1;
                rewardDisplay.Setup(crystalSprites[2]);
                break;
            case EquipmentRarity.Epic:
                DataManager.Instance.PlayerData.Crystals.EpicCrystal += 1;
                rewardDisplay.Setup(crystalSprites[3]);
                break;
            case EquipmentRarity.Legendary:
                DataManager.Instance.PlayerData.Crystals.LegendaryCrystal += 1;
                rewardDisplay.Setup(crystalSprites[4]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        DataManager.Instance.PlayerData.RemoveOwnedEquipment(selectedEquipment.Id);
        ClearShownItems();
        ShowItems();
        selectedEquipment = null;
    }

    private void HideRightUI()
    {
        itemBackground.gameObject.SetActive(false);
        disenchantButton.gameObject.SetActive(false);
        selectedItemDisplay.gameObject.SetActive(false);
    }

    public void Close()
    {
        ClearShownItems();
        gameObject.SetActive(false);
    }

    private void ClearShownItems()
    {
        foreach (var _item in shownItems)
        {
            Destroy(_item.gameObject);
        }
        
        shownItems.Clear();
    }
}
