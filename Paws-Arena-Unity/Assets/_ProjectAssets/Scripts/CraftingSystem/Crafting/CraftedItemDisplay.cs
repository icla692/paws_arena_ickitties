using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftedItemDisplay : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Image iconDisplay;
    [SerializeField] private TextMeshProUGUI nameDisplay;

    public void Setup(EquipmentData _equipmentData)
    {
        closeButton.onClick.AddListener(Close);

        nameDisplay.text = _equipmentData.Thumbnail.name;
        iconDisplay.sprite = _equipmentData.Thumbnail;
        iconDisplay.SetNativeSize();
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
