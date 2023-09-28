using UnityEngine;
using UnityEngine.UI;

public class MarketplaceHandler : MonoBehaviour
{
    private const string MARKETPLACE_URL_KEY = "https://toniq.io/marketplace/ickitties";
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(GoToMarketplace);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GoToMarketplace);
    }

    private void GoToMarketplace()
    {
        Application.OpenURL(MARKETPLACE_URL_KEY);
    }
}
