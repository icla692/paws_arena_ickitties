using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagesBehaviour : MonoBehaviour
{
    public event Action<int> OnClick;
    public List<GameObject> buttons;

    public GameObject backButton;
    public GameObject nextButton;

    public Sprite selectedSprite;
    public Sprite defaultSprite;

    private int nrOfPages;
    private int offset;

    private int selectedNumber = 1;

    private void Start()
    {
        for(int i=0; i<buttons.Count; i++)
        {
            int idx = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClick?.Invoke(idx + offset);
                selectedNumber = idx + offset + 1;
                SetState();
            });
        }
    }
    public void SetNumberOfPages(int nrOfPages)
    {
        this.nrOfPages = nrOfPages;
        offset = 0;
        selectedNumber = 1;

        SetState();
    }

    private void SetState()
    {
        int count = buttons.Count;
        int from = offset + 1;

        backButton.SetActive(offset > 0);
        nextButton.SetActive(offset + count < nrOfPages);

        for(int i=0; i<count; i++)
        {
            int crtValue = from + i;

            if(crtValue == selectedNumber)
            {
                buttons[i].GetComponent<Image>().sprite = selectedSprite;
            }
            else
            {
                buttons[i].GetComponent<Image>().sprite = defaultSprite;
            }

            if (crtValue <= nrOfPages)
            {
                buttons[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "" + crtValue;
            }
            else
            {
                buttons[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
        }
    }

    public void OnNext()
    {
        if (offset + buttons.Count >= nrOfPages) return;
        offset++;
        SetState();
    }

    public void OnBack()
    {
        if (offset == 0) return;
        offset--;
        SetState();
    }
}
