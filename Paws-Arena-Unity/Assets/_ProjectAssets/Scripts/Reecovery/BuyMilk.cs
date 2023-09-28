using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BuyMilk : MonoBehaviour
{
    [SerializeField] private Button doneButton;
    [SerializeField] private Button buyJugOfMilkButton;
    [SerializeField] private Button buyGlassOfMilkButton;

    [SerializeField] private TextMeshProUGUI jugOfMilkDisplay;
    [SerializeField] private TextMeshProUGUI glassOfMilkDisplay;

    [SerializeField] private Color normalAmountColor;
    [SerializeField] private Color zeroAmountColor;

    [SerializeField] private TextMeshProUGUI glassOfMilkPriceDisplay;
    [SerializeField] private TextMeshProUGUI jugOfMilkPriceDisplay;

    public void Setup()
    {
        ShowGlassOfMilk();
        ShowJugOfMilk();

        doneButton.onClick.AddListener(Done);
        buyJugOfMilkButton.onClick.AddListener(BuyJugOfMilk);
        buyGlassOfMilkButton.onClick.AddListener(BuyGlassOfMIlk);

        DataManager.Instance.PlayerData.UpdatedJugOfMilk += ShowJugOfMilk;
        DataManager.Instance.PlayerData.UpdatedGlassOfMilk += ShowGlassOfMilk;

        glassOfMilkPriceDisplay.text = DataManager.Instance.GameData.GlassOfMilkPrice.ToString();
        jugOfMilkPriceDisplay.text = DataManager.Instance.GameData.JugOfMilkPrice.ToString();

        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        doneButton.onClick.AddListener(Done);
        buyJugOfMilkButton.onClick.AddListener(BuyJugOfMilk);
        buyGlassOfMilkButton.onClick.AddListener(BuyGlassOfMIlk);

        DataManager.Instance.PlayerData.UpdatedJugOfMilk -= ShowJugOfMilk;
        DataManager.Instance.PlayerData.UpdatedGlassOfMilk -= ShowGlassOfMilk;
    }

    private void ShowJugOfMilk()
    {
        jugOfMilkDisplay.text = DataManager.Instance.PlayerData.JugOfMilk.ToString();
        jugOfMilkDisplay.color = DataManager.Instance.PlayerData.JugOfMilk == 0 ? zeroAmountColor : normalAmountColor;
    }

    private void ShowGlassOfMilk()
    {
        glassOfMilkDisplay.text = DataManager.Instance.PlayerData.GlassOfMilk.ToString();
        glassOfMilkDisplay.color = DataManager.Instance.PlayerData.GlassOfMilk == 0 ? zeroAmountColor : normalAmountColor;
    }

    private void BuyJugOfMilk()
    {
        StartCoroutine(BuyCooldown());
        if (DataManager.Instance.PlayerData.Snacks<DataManager.Instance.GameData.JugOfMilkPrice)
        {
            return;
        }

        DataManager.Instance.PlayerData.Snacks -= DataManager.Instance.GameData.JugOfMilkPrice;
        DataManager.Instance.PlayerData.JugOfMilk++;
    }

    private void BuyGlassOfMIlk()
    {
        StartCoroutine(BuyCooldown());
        if (DataManager.Instance.PlayerData.Snacks< DataManager.Instance.GameData.GlassOfMilkPrice)
        {
            return;
        }

        DataManager.Instance.PlayerData.Snacks -= DataManager.Instance.GameData.GlassOfMilkPrice;
        DataManager.Instance.PlayerData.GlassOfMilk++;
    }

    private void Done()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator BuyCooldown()
    {
        buyJugOfMilkButton.interactable = false;
        buyGlassOfMilkButton.interactable = false;
        yield return new WaitForSeconds(1);
        buyJugOfMilkButton.interactable = true;
        buyGlassOfMilkButton.interactable = true;
    }
}
