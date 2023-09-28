using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardPanel : MonoBehaviour
{
    public static Action OnClosePressed;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image iconDisplay;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private GameObject holder;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
        LevelRewardDisplay.OnClaimed += Setup;
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        LevelRewardDisplay.OnClaimed -= Setup;
    }


    public void Setup(LevelReward _reward, Sprite _sprite)
    {
        nameDisplay.text = _reward.Name;
        iconDisplay.sprite = _sprite;
        iconDisplay.SetNativeSize();
        holder.SetActive(true);
    }

    private void Close()
    {
        OnClosePressed?.Invoke();
        holder.SetActive(false);
    }
}
