using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisenchantmentItemDisplay : MonoBehaviour
{
    public static Action<EquipmentData> OnEquipmentClicked;
    [SerializeField] private Image itemDisplay;
    [SerializeField] private GameObject selectedDisplay;
    private Button button;
    private EquipmentData equipmentData;

    public void Setup(EquipmentData _equipmentData)
    {
        equipmentData = _equipmentData;
        itemDisplay.sprite = equipmentData.Thumbnail;
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        OnEquipmentClicked += CheckIfThisIsSelected;
        button.onClick.AddListener(ShowEquipment);
    }

    private void OnDisable()
    {
        OnEquipmentClicked -= CheckIfThisIsSelected;
        button.onClick.RemoveListener(ShowEquipment);
    }

    private void ShowEquipment()
    {
        OnEquipmentClicked(equipmentData);
    }

    private void CheckIfThisIsSelected(EquipmentData _equipmentData)
    {
        selectedDisplay.SetActive(_equipmentData == equipmentData);
    }
}
