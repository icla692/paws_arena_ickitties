using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEnvironment
{
    DEV,
    STAGING,
    PROD
}

[CreateAssetMenu(fileName = "Config", menuName = "Configurations/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public bool enableDevLogs = false;

    public GameEnvironment env;
    public string devUrl = "https://localhost:7226";
    public string stagingUrl = "https://localhost:7226";
    public string prodUrl = "https://localhost:7226";

    public string devPhotonKey = "ea89fb64-713d-4979-b25e-f61a960b4bde";
    public string prodPhotonKey = "3a4a871a-3614-4f4e-8c0e-43ed06d3d6ed";
}
