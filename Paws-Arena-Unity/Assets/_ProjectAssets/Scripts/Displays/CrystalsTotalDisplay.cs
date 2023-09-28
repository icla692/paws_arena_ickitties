using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CrystalsTotalDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private TextMeshProUGUI glowDisplay;

    private void OnEnable()
    {
        DataManager.Instance.PlayerData.Crystals.UpdatedCommonCrystal += Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedUncommonCrystal += Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedRareCrystal += Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedLegendaryCrystal += Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedEpicCrystal += Show;

        Show();
    }

    private void OnDisable()
    {
        DataManager.Instance.PlayerData.Crystals.UpdatedCommonCrystal -= Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedUncommonCrystal -= Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedRareCrystal -= Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedLegendaryCrystal -= Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedEpicCrystal -= Show;
    }

    private void Show()
    {
        display.text = DataManager.Instance.PlayerData.Crystals.TotalCrystalsAmount.ToString();
        glowDisplay.text = DataManager.Instance.PlayerData.Crystals.TotalCrystalsAmount.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        glowDisplay.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        glowDisplay.gameObject.SetActive(false);
    }
}
