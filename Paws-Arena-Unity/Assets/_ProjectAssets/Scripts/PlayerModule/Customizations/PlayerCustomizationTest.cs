using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCustomization))]
public class PlayerCustomizationTest : MonoBehaviour
{

    private PlayerCustomization playerCustomization;

    private void Awake()
    {
        playerCustomization = GetComponent<PlayerCustomization>();
    }

    //********//

    public string colorId = "kittyColor1";

    [Button(text:"Set Color", enabledMode: EButtonEnableMode.Playmode)]
    private void SetColor()
    {
        playerCustomization.SetKittyColor(colorId);
    }

    public string eyesId = "eyes1";

    [Button(text: "Set Eyes", enabledMode: EButtonEnableMode.Playmode)]
    private void SetEyes()
    {
        playerCustomization.SetEyes(eyesId);
    }

    public string backId = "back1";

    [Button(text: "Set Back", enabledMode: EButtonEnableMode.Playmode)]
    private void SetBack()
    {
        playerCustomization.SetBack(backId);
    }

    public string bodyId = "body1";

    [Button(text: "Set Body", enabledMode: EButtonEnableMode.Playmode)]
    private void SetBody()
    {
        playerCustomization.SetBody(bodyId);
    }


    public string hatsId = "hats1";

    [Button(text: "Set Hats", enabledMode: EButtonEnableMode.Playmode)]
    private void SetHats()
    {
        playerCustomization.SetHat(hatsId);
    }


    public string eyewearId = "eyewear1";

    [Button(text: "Set Eyewear", enabledMode: EButtonEnableMode.Playmode)]
    private void SetEyewear()
    {
        playerCustomization.SetEyewear(eyewearId);
    }


    public string mouthId = "mouth1";

    [Button(text: "Set Mouth", enabledMode: EButtonEnableMode.Playmode)]
    private void SetMouth()
    {
        playerCustomization.SetMouth(mouthId);
    }


    public string groundFrontId = "groundfront1";

    [Button(text: "Set GroundFront", enabledMode: EButtonEnableMode.Playmode)]
    private void SetGroundFront()
    {
        playerCustomization.SetGroundFront(groundFrontId);
    }


    public string groundBackId = "groundback1";

    [Button(text: "Set GroundBack", enabledMode: EButtonEnableMode.Playmode)]
    private void SetGroundBack()
    {
        playerCustomization.SetGroundBack(groundBackId);
    }


    public string groundId = "ground1";

    [Button(text: "Set Ground", enabledMode: EButtonEnableMode.Playmode)]
    private void SetGround()
    {
        playerCustomization.SetGround(groundId);
    }

}
