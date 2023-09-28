using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpritesEquipment : Equipment
{
    [SerializeField]
    public List<Sprite> sprites;
}

public class PlayerBodyCustomization : MonoBehaviour
{
    [SerializeField]
    public List<SpriteRenderer> targets;
    [SerializeField]
    public List<SpritesEquipment> spritesConfig;

    public void SetDefaultBody()
    {
        SetBody(spritesConfig[0]);
    }

    public void SetRainbowBody()
    {
        SetBody(spritesConfig[1]);
    }

    private void SetBody(SpritesEquipment equipment)
    {
        for(int i=0; i<equipment.sprites.Count; i++) {
            targets[i].sprite = equipment.sprites[i];
        }
    }
}
