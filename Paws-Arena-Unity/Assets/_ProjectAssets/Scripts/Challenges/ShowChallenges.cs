using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowChallenges : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private ChallengesPanel challengesPanel;
    private void OnEnable()
    {
        button.onClick.AddListener(Show);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Show);
    }

    private void Show()
    {
        challengesPanel.Setup();
    }
}
