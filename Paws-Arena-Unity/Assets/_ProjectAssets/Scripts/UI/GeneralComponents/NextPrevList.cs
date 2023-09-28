using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPrevList : MonoBehaviour
{
    public int currentIdx = 0;

    public GameObject nextButton;
    public GameObject prevButton;

    private void OnEnable()
    {
        SetCurrentIdx(currentIdx);
    }

    public void Next()
    {
        if (currentIdx + 1 >= GetNumberOfElements()) return;
        SetCurrentIdx(currentIdx + 1);
    }

    public void Back()
    {
        if (currentIdx <= 0) return;
        SetCurrentIdx(currentIdx - 1);
    }

    protected virtual int GetNumberOfElements()
    {
        return 0;
    }

    protected virtual void SetCurrentIdx(int newIdx)
    {
        currentIdx = newIdx;

        prevButton.SetActive(currentIdx != 0);
        nextButton.SetActive(currentIdx < (GetNumberOfElements() - 1));
    }
}
