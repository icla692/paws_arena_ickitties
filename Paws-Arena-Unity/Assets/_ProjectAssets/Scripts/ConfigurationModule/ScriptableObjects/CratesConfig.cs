using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrateConfig
{
    [SerializeField]
    public GameObject prefab;
}

[System.Serializable]
public class HealthCrate : CrateConfig
{
    [SerializeField]
    public int minHP = 10;
    [SerializeField]
    public int maxHP = 50;
}

[CreateAssetMenu(fileName = "CratesConfig", menuName = "Configurations/CratesConfig", order = 3)]
public class CratesConfig : ScriptableObject
{
    [SerializeReference]
    private List<CrateConfig> crates;

    private void Awake()
    {
        if (crates == null)
        {
            crates = new List<CrateConfig>();
            crates.Add(new HealthCrate());
        }
    }

    public CrateConfig GetCrate()
    {
        return crates[0];
    }

    public T GetCrate<T>() where T : CrateConfig
    {
        foreach(CrateConfig crate in crates)
        {
            if (crate is T) return (T)crate;
        }

        return null;
    }
}
