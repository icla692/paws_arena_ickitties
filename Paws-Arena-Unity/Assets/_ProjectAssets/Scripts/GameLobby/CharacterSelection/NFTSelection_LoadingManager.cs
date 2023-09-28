using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTSelection_LoadingManager : MonoBehaviour
{
    public LoadingScreen loadingScreen;
    public GameObject enterArenaButton;

    private List<string> loadingReasons;

    private void Start()
    {
        loadingReasons = new List<string>();
    }
    public void AddLoadingReason(string reason)
    {
        if (loadingReasons == null)
        {
            loadingReasons = new List<string>();
        }


        enterArenaButton.SetActive(false);
        loadingReasons.Add(reason);

        if (!loadingScreen.IsActivated())
        {
            loadingScreen.Activate(reason);
        }
    }

    public void StopLoadingReason(string reason)
    {
        loadingReasons.Remove(reason);

        if (loadingReasons.Count > 0)
        {
            loadingScreen.Activate(loadingReasons[0]);
        }
        else
        {
            enterArenaButton.SetActive(true);
            loadingScreen.Deactivate();
        }
    }
}
