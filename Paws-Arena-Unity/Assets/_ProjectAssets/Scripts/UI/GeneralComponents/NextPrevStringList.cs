using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPrevStringList : NextPrevList
{
    [SerializeField]
    private GameObject value;
    public List<string> list;

    protected override int GetNumberOfElements()
    {
        return list.Count;
    }

    protected override void SetCurrentIdx(int newIdx)
    {
        base.SetCurrentIdx(newIdx);
        value.GetComponent<TMPro.TextMeshProUGUI>().text = list[newIdx];
    }

    public void SetValue(string v)
    {
        int idx = 0;
        foreach(string val in list)
        {
            if(val == v)
            {
                SetCurrentIdx(idx);
                return;
            }
            idx++;
        }
    }

    public string GetValue()
    {
        return list[currentIdx];
    }
}
