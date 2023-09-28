using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationModal : MonoBehaviour
{
    [SerializeField]
    private GameObject modalWrapper;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private TMPro.TextMeshProUGUI descriptionText;

    private void Start()
    {
        cancelButton.onClick.AddListener(HideModal);
    }

    private void HideModal()
    {
        modalWrapper.SetActive(false);
    }

    public void ShowModal(string description, Action onConfirm)
    {
        modalWrapper.SetActive(true);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => HideModal());
        confirmButton.onClick.AddListener(() => onConfirm?.Invoke());

        descriptionText.text = description;
    }
}
