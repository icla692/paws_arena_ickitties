using System;
using Newtonsoft.Json;
using UnityEngine;

[SerializeField]
public class CrystalsData
{
    private float commonCrystal = 0;
    private float uncommonCrystal = 0;
    private float rareCrystal = 0;
    private float epicCrystal = 0;
    private float legendaryCristal = 0;
    
    [JsonIgnore] public Action UpdatedCommonCrystal;
    [JsonIgnore] public Action UpdatedUncommonCrystal;
    [JsonIgnore] public Action UpdatedRareCrystal;
    [JsonIgnore] public Action UpdatedEpicCrystal;
    [JsonIgnore] public Action UpdatedLegendaryCrystal;
    
    public float CommonCrystal
    {
        get
        {
            return commonCrystal;
        }
        set
        {
            commonCrystal = value;
            UpdatedCommonCrystal?.Invoke();
        }
    }

    public float UncommonCrystal
    {
        get
        {
            return uncommonCrystal;
        }
        set
        {
            uncommonCrystal = value;
            UpdatedUncommonCrystal?.Invoke();
        }
    }

    public float RareCrystal
    {
        get
        {
            return rareCrystal;
        }
        set
        {
            rareCrystal = value;
            UpdatedRareCrystal?.Invoke();
        }
    }

    public float EpicCrystal
    {
        get
        {
            return epicCrystal;
        }

        set
        {
            epicCrystal = value;
            UpdatedEpicCrystal?.Invoke();
        }
    }

    public float LegendaryCrystal
    {
        get
        {
            return legendaryCristal;
        }
        set
        {
            legendaryCristal = value;
            UpdatedLegendaryCrystal?.Invoke();
        }
    }
    
    [JsonIgnore]
    public float TotalCrystalsAmount => commonCrystal + uncommonCrystal + rareCrystal + epicCrystal + legendaryCristal;

}
