using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPrevGameObjectList : NextPrevList
{
    public List<GameObject> list;


    protected override int GetNumberOfElements()
    {
        return list.Count;
    }

    protected override void SetCurrentIdx(int newIdx)
    {
        base.SetCurrentIdx(newIdx);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(i == currentIdx);
        }
    }
}
