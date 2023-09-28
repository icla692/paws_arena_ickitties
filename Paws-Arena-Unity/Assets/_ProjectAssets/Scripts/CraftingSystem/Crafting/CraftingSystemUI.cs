using UnityEngine;
using UnityEngine.UI;

public class CraftingSystemUI : MonoBehaviour
{
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private DisenchantUI disenchantUI;
    [SerializeField] private Button closeButton;
    public void Setup()
    {
        craftingUI.Setup();
        disenchantUI.Close();
        closeButton.onClick.AddListener(Close);
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    //called from UI buttons
    public void ShowCraftiongUI()
    {
        craftingUI.Setup();
        disenchantUI.Close();
    }


    //called from UI buttons
    public void ShowDisenchtUI()
    {
        craftingUI.Close();
        disenchantUI.Setup();
    }

    private void Close()
    {
        craftingUI.Close();
        disenchantUI.Close();
        gameObject.SetActive(false);
    }
}
