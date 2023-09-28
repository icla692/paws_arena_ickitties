using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EquipmentType
{
    COLOR,
    EYES,
    BACK,
    BODY,
    HAT,
    EYEWEAR,
    MOUTH,
    TAIL,
    LEGS,
    GROUND_FRONT,
    GROUND_BACK,
    GROUND,
    NONE
}

[System.Serializable]
public class Equipment
{
    [SerializeField]
    public string id;
}

[System.Serializable]
public class SpriteEquipment : Equipment
{
    [SerializeField]
    public Sprite sprite;
}

[System.Serializable]
public class ColorEquipment : Equipment
{
    [SerializeField]
    public Color color;
}

[System.Serializable]
public class GameObjectEquipment : Equipment
{
    [SerializeField]
    public GameObject target;
}


public class PlayerCustomization : MonoBehaviour
{
    [SerializeField]
    private bool inGame = false;

    [SerializeField]
    public GameObject wrapper;

    [Space]

    [SerializeField]
    private GameObject backpack;
    [SerializeField]
    private GameObject rEar;
    [SerializeField]
    private GameObject lEar;

    [Header("KittyColor")]
    [SerializeField]
    private List<SpriteRenderer> colorMultiplyElements;

    [SerializeField]
    private List<ColorEquipment> kittyColorEquipment;

    [Header("Eyes")]
    [SerializeField]
    private List<GameObjectEquipment> eyesEquipment;

    [Header("Back")]
    [SerializeField]
    private GameObject backParent;
    [SerializeField]
    private SpriteRenderer backSpriteRenderer;
    [SerializeField]
    private List<SpriteEquipment> backEquipment;


    [Header("Body")]
    [SerializeField]
    private SpriteRenderer bodySpriteRenderer;

    [SerializeField]
    private List<SpriteEquipment> bodyEquipment;


    [Header("Hats")]
    [SerializeField]
    private SpriteRenderer hatsSpriteRenderer;
    [SerializeField]
    private SpriteRenderer hatsNoEarsRenderer;
    [SerializeField]
    private SpriteRenderer hatsBetweenEarsRenderer;

    [SerializeField]
    private List<SpriteEquipment> hatsEquipment;

    [SerializeField]
    private List<SpriteEquipment> hatsNoEarsEquipment;

    [SerializeField]
    private List<SpriteEquipment> hatsBetweenEarsEquipment;


    [Header("Earrings")]
    [SerializeField]
    private List<GameObject> earrings;

    [SerializeField]
    private List<Equipment> earringsEquipment;

    [SerializeField]
    private Sprite earringSprite;
    [SerializeField]
    private SpriteEquipment boneHatEquipment;

    [Header("Eyewear")]
    [SerializeField]
    private SpriteRenderer eyewearSpriteRenderer;
    [SerializeField]
    private List<SpriteEquipment> eyewearEquipment;

    [SerializeField]
    private SpriteRenderer closeableEyewearSpriteRenderer;

    [SerializeField]
    private List<SpriteEquipment> closeableEyewearEquipment;

    [Header("Mouth")]
    [SerializeField]
    private SpriteRenderer mouthSpriteRenderer;
    [SerializeField]
    private List<SpriteEquipment> mouthEquipment;

    [Header("Tail")]
    [SerializeField]
    private SpriteRenderer overlayTailRenderer;
    [SerializeField]
    private List<SpriteEquipment> overlayTailEquipment;

    [SerializeField]
    private List<GameObjectEquipment> tailAnimatedObjectsEquipment;

    [SerializeField]
    private SpriteRenderer staticTailObjectRenderer;
    [SerializeField]
    private List<SpriteEquipment> tailStaticObjectsEquipment;

    [Header("GroundFront")]
    [SerializeField]
    private Transform groundFrontTransform;

    [SerializeField]
    private SpriteRenderer groundFrontSpriteRenderer;

    [SerializeField]
    private List<SpriteEquipment> groundFrontEquipment;

    [Header("GroundBack")]
    [SerializeField]
    private Transform groundBackTransform;
    [SerializeField]
    private SpriteRenderer groundBackSpriteRenderer;
    [SerializeField]
    private List<SpriteEquipment> groundBackEquipment;

    [Header("Ground")]
    [SerializeField]
    private Transform groundTransform;
    [SerializeField]
    private SpriteRenderer groundSpriteRenderer;

    [SerializeField]
    private List<SpriteEquipment> groundEquipment;

    [HideInInspector]
    public Dictionary<EquipmentType, Equipment> playerEquipmentConfig;

    private string url;

    private void OnEnable()
    {
        if (!inGame)
        {
            backpack.SetActive(false);
        }
        else
        {
            backParent.SetActive(false);
            groundFrontTransform.gameObject.SetActive(false);
            groundBackTransform.gameObject.SetActive(false);
            groundTransform.gameObject.SetActive(false);
        }

    }

    public KittyCustomization SetCat(string url, List<string> ids)
    {
        this.url = url;

        var config = KittiesCustomizationService.GetCustomization(url);

        //Generate original config
        if (config == null)
        {
            playerEquipmentConfig = new Dictionary<EquipmentType, Equipment>();

            foreach (string id in ids)
            {
                EquipmentType equipmentType = GetEquipmentTypeById(id);

                if (equipmentType == EquipmentType.NONE) continue;

                playerEquipmentConfig.Add(equipmentType, new Equipment()
                {
                    id = id
                });
            }

            config = SaveOriginalConfig();
        }

        playerEquipmentConfig = new Dictionary<EquipmentType, Equipment>();
        if (config.playerEquipmentConfig != null && config.playerEquipmentConfig.Count > 0)
        {
            ApplyConfig(config.playerEquipmentConfig);
        }
        else
        {
            ApplyConfig(config.originalConfig);
        }

        return config;
    }

    public void SetTransientCat(string url, List<string> ids)
    {
        this.url = url;

        playerEquipmentConfig = new Dictionary<EquipmentType, Equipment>();
        var equipmentDefaultConfig = new Dictionary<EquipmentType, Equipment>();

        foreach (string id in ids)
        {
            EquipmentType equipmentType = GetEquipmentTypeById(id);

            if (equipmentType == EquipmentType.NONE) continue;

            equipmentDefaultConfig.Add(equipmentType, new Equipment()
            {
                id = id
            });
        }

        ApplyConfig(equipmentDefaultConfig);
    }

    public void SetTransientCat(string url, KittyCustomization customization)
    {
        this.url = url;

        playerEquipmentConfig = new Dictionary<EquipmentType, Equipment>();
        if (customization.playerEquipmentConfig != null && customization.playerEquipmentConfig.Count > 0)
        {
            ApplyConfig(customization.playerEquipmentConfig);
        }
        else
        {
            ApplyConfig(customization.originalConfig);
        }
    }


    public void ResetToOriginal()
    {
        KittiesCustomizationService.RemoveCustomizations(url);
        RemoveAllEquipment();
        var config = KittiesCustomizationService.GetCustomization(url);
        ApplyConfig(config.originalConfig);
    }

    private void ApplyConfig(Dictionary<EquipmentType, Equipment> playerEquipmentConfig)
    {
        foreach (KeyValuePair<EquipmentType, Equipment> pair in playerEquipmentConfig)
        {
            SetSpriteEquipment(pair.Key, pair.Value.id);
        }
    }

    private void SetSpriteEquipment(EquipmentType equipmentType, string id)
    {
        switch (equipmentType)
        {
            case EquipmentType.COLOR:
                {
                    SetKittyColor(id);
                    break;
                }
            case EquipmentType.EYES:
                {
                    SetEyes(id);
                    break;
                }
            case EquipmentType.BACK:
                {
                    SetBack(id);
                    break;
                }
            case EquipmentType.BODY:
                {
                    SetBody(id);
                    break;
                }
            case EquipmentType.HAT:
                {
                    SetHat(id);
                    break;
                }
            case EquipmentType.EYEWEAR:
                {
                    SetEyewear(id);
                    break;
                }
            case EquipmentType.MOUTH:
                {
                    SetMouth(id);
                    break;
                }
            case EquipmentType.GROUND:
                {
                    SetGround(id);
                    break;
                }
            case EquipmentType.GROUND_BACK:
                {
                    SetGroundBack(id);
                    break;
                }
            case EquipmentType.GROUND_FRONT:
                {
                    SetGroundFront(id);
                    break;
                }
            case EquipmentType.TAIL:
                {
                    SetTail(id);
                    break;
                }
            case EquipmentType.LEGS:
                {
                    break;
                }
        }
    }

    public EquipmentType GetEquipmentTypeById(string id)
    {
        if (kittyColorEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.COLOR;
        }
        if (eyesEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.EYES;
        }
        if (backEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.BACK;
        }
        if (bodyEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.BODY;
        }
        if (earringsEquipment.FindIndex(eq => eq.id == id) >= 0 || boneHatEquipment.id == id ||
            hatsBetweenEarsEquipment.FindIndex(eq => eq.id == id) >= 0 ||
            hatsEquipment.FindIndex(eq => eq.id == id) >= 0 ||
            hatsNoEarsEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.HAT;
        }

        if (eyewearEquipment.FindIndex(eq => eq.id == id) >= 0 || closeableEyewearEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.EYEWEAR;
        }

        if (mouthEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.MOUTH;
        }

        if(tailAnimatedObjectsEquipment.FindIndex(eq => eq.id == id) >= 0 ||
            overlayTailEquipment.FindIndex(eq => eq.id == id) >= 0 ||
            tailStaticObjectsEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.TAIL;
        }

        if (groundBackEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.GROUND_BACK;
        }

        if (groundEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.GROUND;
        }

        if (groundFrontEquipment.FindIndex(eq => eq.id == id) >= 0)
        {
            return EquipmentType.GROUND_FRONT;
        }

        return EquipmentType.NONE;
    }

    public void RemoveAllEquipment()
    {
        List<EquipmentType> types = new List<EquipmentType>();
        types.Add(EquipmentType.EYEWEAR);
        types.Add(EquipmentType.HAT);
        types.Add(EquipmentType.MOUTH);
        types.Add(EquipmentType.BODY);

        foreach (EquipmentType type in types)
        {
            SetEquipmentBySprite(type, null);
        }
    }

    public void SetEquipmentBySprite(EquipmentType equipmentType, Sprite equipmentSprite)
    {
        switch (equipmentType)
        {
            case EquipmentType.EYEWEAR:
                {
                    //Empty case
                    if(equipmentSprite == null)
                    {
                        SetEyewear("none");
                        break;
                    }

                    bool found = FindBySprite(equipmentSprite, eyewearEquipment, id => SetEyewear(id));

                    if (found) { break; }

                    FindBySprite(equipmentSprite, closeableEyewearEquipment, id => SetEyewear(id));
                    break;
                }
            case EquipmentType.HAT:
                {
                    //Empty case
                    if (equipmentSprite == null)
                    {
                        SetHat("none");
                        break;
                    }

                    bool found = FindBySprite(equipmentSprite, hatsEquipment, id => SetHat(id));
                    if (found) { break; }

                    found = FindBySprite(equipmentSprite, hatsNoEarsEquipment, id => SetHat(id));
                    if (found) { break; }

                    FindBySprite(equipmentSprite, hatsBetweenEarsEquipment, id => SetHat(id));
                    break;
                }
            case EquipmentType.MOUTH:
                {
                    //Empty case
                    if (equipmentSprite == null)
                    {
                        SetMouth("none");
                        break;
                    }
                    FindBySprite(equipmentSprite, mouthEquipment, id => SetMouth(id));
                    break;
                }
            case EquipmentType.BODY:
                {
                    //Empty case
                    if (equipmentSprite == null)
                    {
                        SetBody("none");
                        break;
                    }
                    FindBySprite(equipmentSprite, bodyEquipment, id => SetBody(id));
                    break;
                }
            case EquipmentType.TAIL:
                {
                    if (equipmentSprite == null)
                    {
                        SetTail("none");
                        break;
                    }
                    
                    FindBySprite(equipmentSprite, overlayTailEquipment, id => SetTail(id));
                    FindBySprite(equipmentSprite, tailStaticObjectsEquipment, id => SetTail(id));
                    break;
                }
            case EquipmentType.LEGS:
                {
                    break;
                }
        }
    }

    [ContextMenu("Save")]
    public void SaveCustomConfig()
    {
        KittiesCustomizationService.SaveCustomConfig(url, playerEquipmentConfig);
    }
    public KittyCustomization SaveOriginalConfig()
    {
        return KittiesCustomizationService.SaveOriginalConfig(url, playerEquipmentConfig);
    }

    private bool FindBySprite(Sprite sprite, List<SpriteEquipment> equipment, Action<string> callback)
    {
        int idx = equipment.FindIndex(el => el.sprite == sprite);
        if (idx != -1)
        {
            callback?.Invoke(equipment[idx].id);
            return true;
        }

        return false;
    }

    public void SetKittyColor(string colorId, bool updateConfig = true)
    {
        if(colorId == "kittycolor8")
        {
            GetComponent<PlayerBodyCustomization>().SetRainbowBody();
        }
        else
        {
            GetComponent<PlayerBodyCustomization>().SetDefaultBody();
        }

        ColorEquipment equipment = SetColor(colorId, kittyColorEquipment, colorMultiplyElements);
        if (updateConfig && equipment != null)
        {
            AddOrUpdateEquipment(EquipmentType.COLOR, equipment);
        }
    }

    public void SetEyes(string eyesId, bool updateConfig = true)
    {
        GameObjectEquipment eq = SetSingleActiveElement(eyesId, eyesEquipment);

        if (updateConfig && eq != null)
        {
            AddOrUpdateEquipment(EquipmentType.EYES, eq);
        }
    }

    public void SetBack(string backId, bool updateConfig = true)
    {
        if (inGame) return;
        SpriteEquipment eq = SetSingleSpriteElement(backId, backEquipment, backSpriteRenderer);

        if (updateConfig && eq != null)
        {
            AddOrUpdateEquipment(EquipmentType.BACK, eq);
        }
    }

    public void SetBody(string bodyId, bool updateConfig = true)
    {
        SpriteEquipment eq = SetSingleSpriteElement(bodyId, bodyEquipment, bodySpriteRenderer);

        if (updateConfig && eq != null)
        {
            AddOrUpdateEquipment(EquipmentType.BODY, eq);
        }
    }

    public void SetTail(string id, bool updateConfig = true)
    {
        int idx = overlayTailEquipment.FindIndex(el => el.id == id);
        overlayTailRenderer.sprite = null;
        if (idx >= 0)
        {
            SpriteEquipment eq = SetSingleSpriteElement(id, overlayTailEquipment, overlayTailRenderer);

            if (updateConfig && eq != null)
            {
                AddOrUpdateEquipment(EquipmentType.TAIL, eq);
            }
        }


        for (int i = 0; i < tailAnimatedObjectsEquipment.Count; i++)
        {
            tailAnimatedObjectsEquipment[i].target.SetActive(false);
        }

        idx = tailAnimatedObjectsEquipment.FindIndex(el => el.id == id);
        if(idx >= 0)
        {
            GameObjectEquipment eq = SetSingleActiveElement(id, tailAnimatedObjectsEquipment);

            if (updateConfig && eq != null)
            {
                AddOrUpdateEquipment(EquipmentType.TAIL, eq);
            }
        }

        idx = tailStaticObjectsEquipment.FindIndex(el => el.id == id);
        staticTailObjectRenderer.sprite = null;
        if (idx >= 0)
        {
            SpriteEquipment eq = SetSingleSpriteElement(id, tailStaticObjectsEquipment, staticTailObjectRenderer);

            if (updateConfig && eq != null)
            {
                AddOrUpdateEquipment(EquipmentType.TAIL, eq);
            }
        }
    }

    public void SetHat(string hatId, bool updateConfig = true)
    {
        int idx = earringsEquipment.FindIndex(el => el.id == hatId);
        if (idx >= 0)
        {
            if (updateConfig)
            {
                AddOrUpdateEquipment(EquipmentType.HAT, earringsEquipment[idx]);
            }
            if (idx == 0) //right
            {
                earrings[1].GetComponent<SpriteRenderer>().sprite = earringSprite;
            }
            else if (idx == 1) //left
            {
                earrings[0].GetComponent<SpriteRenderer>().sprite = earringSprite;
            }
            else //dual
            {
                earrings[0].GetComponent<SpriteRenderer>().sprite = earrings[1].GetComponent<SpriteRenderer>().sprite = earringSprite;
            }
            return;
        }
        else if (boneHatEquipment.id == hatId)
        {
            if (updateConfig)
            {
                AddOrUpdateEquipment(EquipmentType.HAT, boneHatEquipment);
            }
            earrings[1].GetComponent<SpriteRenderer>().sprite = boneHatEquipment.sprite;
            return;
        }


        idx = hatsBetweenEarsEquipment.FindIndex(el => el.id == hatId);
        if (idx >= 0)
        {
            if (updateConfig)
            {
                AddOrUpdateEquipment(EquipmentType.HAT, hatsBetweenEarsEquipment[idx]);
            }

            lEar.SetActive(true);
            rEar.SetActive(true);

            hatsBetweenEarsRenderer.gameObject.SetActive(true);
            hatsNoEarsRenderer.gameObject.SetActive(false);
            hatsSpriteRenderer.gameObject.SetActive(false);

            SetSingleSpriteElement(hatId, hatsBetweenEarsEquipment, hatsBetweenEarsRenderer);
            return;
        }

        idx = hatsEquipment.FindIndex(el => el.id == hatId);

        if (idx >= 0)
        {
            if (updateConfig)
            {
                AddOrUpdateEquipment(EquipmentType.HAT, hatsEquipment[idx]);
            }

            lEar.SetActive(true);
            rEar.SetActive(true);

            hatsBetweenEarsRenderer.gameObject.SetActive(false);
            hatsNoEarsRenderer.gameObject.SetActive(false);
            hatsSpriteRenderer.gameObject.SetActive(true);

            SetSingleSpriteElement(hatId, hatsEquipment, hatsSpriteRenderer);
            return;
        }

        idx = hatsNoEarsEquipment.FindIndex(el => el.id == hatId);
        if (idx >= 0)
        {
            if (updateConfig)
            {
                AddOrUpdateEquipment(EquipmentType.HAT, hatsNoEarsEquipment[idx]);
            }

            lEar.SetActive(false);
            rEar.SetActive(false);

            hatsBetweenEarsRenderer.gameObject.SetActive(false);
            hatsNoEarsRenderer.gameObject.SetActive(true);
            hatsSpriteRenderer.gameObject.SetActive(false);

            SetSingleSpriteElement(hatId, hatsNoEarsEquipment, hatsNoEarsRenderer);
            return;
        }
    }

    public void SetEyewear(string eyewearId, bool updateConfig = true)
    {
        SetSingleSpriteElement("none", eyewearEquipment, eyewearSpriteRenderer);
        SetSingleSpriteElement("none", closeableEyewearEquipment, closeableEyewearSpriteRenderer);

        if (playerEquipmentConfig.ContainsKey(EquipmentType.EYES))
        {
            SetEyes(playerEquipmentConfig[EquipmentType.EYES].id, false);
        }

        SpriteEquipment eq = SetSingleSpriteElement(eyewearId, eyewearEquipment, eyewearSpriteRenderer);
        if (eq == null)
        {
            eq = SetSingleSpriteElement(eyewearId, closeableEyewearEquipment, closeableEyewearSpriteRenderer);
            if(eq != null && eyewearId != "none")
            {
                //Closeable eyes (zombie, snoop) only work with normal eyes
                SetEyes("eyes1", false);
            }
        }

        if (updateConfig && eq != null)
        {
            AddOrUpdateEquipment(EquipmentType.EYEWEAR, eq);
        }
    }

    public void SetMouth(string mouthId, bool updateConfig = true)
    {
        SpriteEquipment eq = SetSingleSpriteElement(mouthId, mouthEquipment, mouthSpriteRenderer);
        if (updateConfig && eq != null)
        {
            AddOrUpdateEquipment(EquipmentType.MOUTH, eq);
        }
    }

    public void SetGroundFront(string groundId, bool updateConfig = true)
    {
        if (inGame) return;

        SpriteEquipment eq = SetSingleSpriteElement(groundId, groundFrontEquipment, groundFrontSpriteRenderer);
        if (updateConfig && eq != null)
        {
            AddOrUpdateEquipment(EquipmentType.GROUND_FRONT, eq);
        }
    }

    public void SetGroundBack(string groundId, bool updateConfig = true)
    {
        if (inGame) return;

        SpriteEquipment eq = SetSingleSpriteElement(groundId, groundBackEquipment, groundBackSpriteRenderer);
        if (updateConfig && eq != null)
        {
            AddOrUpdateEquipment(EquipmentType.GROUND_BACK, eq);
        }
    }

    public void SetGround(string groundId, bool updateConfig = true)
    {
        if (inGame) return;

        SpriteEquipment eq = SetSingleSpriteElement(groundId, groundEquipment, groundSpriteRenderer);
        if (updateConfig && eq != null)
        {
            AddOrUpdateEquipment(EquipmentType.GROUND, eq);
        }
    }

    private SpriteEquipment SetSingleSpriteElement(string key, List<SpriteEquipment> equipment, SpriteRenderer spriteRenderer)
    {
        int idx = equipment.FindIndex(eq => eq.id == key);
        if (idx < 0)
        {
            return null;
        }

        spriteRenderer.sprite = equipment[idx].sprite;
        return equipment[idx];
    }

    private ColorEquipment SetColor(string key, List<ColorEquipment> elements, List<SpriteRenderer> targets)
    {
        int idx = elements.FindIndex(el => el.id == key);
        if (idx < 0)
        {
            return null;
        }

        Color col = elements[idx].color;
        foreach (SpriteRenderer sprite in targets)
        {
            sprite.color = col;
        }
        return elements[idx];
    }
    private GameObjectEquipment SetSingleActiveElement(string key, List<GameObjectEquipment> equipment)
    {
        int idx = equipment.FindIndex(el => el.id == key);
        if (idx < 0)
        {
            return null;
        }

        for (int i = 0; i < equipment.Count; i++)
        {
            equipment[i].target.SetActive(i == idx);
        }

        return equipment[idx];
    }

    private void AddOrUpdateEquipment(EquipmentType equipmentType, Equipment equipment)
    {
        if (playerEquipmentConfig.ContainsKey(equipmentType))
        {
            playerEquipmentConfig[equipmentType] = equipment;
        }
        else
        {
            playerEquipmentConfig.Add(equipmentType, equipment);
        }
    }
}
