using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipScreen : MonoBehaviour
{
    public LobbyUIManager lobbyUIManager;
    public Transform playerPlatformParent;
    public GameObject playerPlatformPrefab;

    public EquipmentsConfig equipmentsConfig;

    public Transform content;
    public GameObject nftPrefab;

    [Header("Btns")]
    public ButtonHoverable eyeBtn;
    public ButtonHoverable headBtn;
    public ButtonHoverable mouthBtn;
    public ButtonHoverable bodyBtn;
    public ButtonHoverable tailBtn;

    private GameObject playerPlatform;
    private ButtonHoverable selectedBtn;

    private List<NFTImageSprite> equipments;
    private PlayerCustomization playerCustomization;
    private EquipmentType currentType;
    private NFTImageSprite selectedEquipment;

    private void OnEnable()
    {
        playerPlatform = GameObject.Instantiate(playerPlatformPrefab, playerPlatformParent);
        playerPlatform.transform.localPosition = Vector3.zero;

        playerCustomization = playerPlatform.GetComponent<PlayerPlatformBehaviour>().playerCustomization;

        equipments = new List<NFTImageSprite>();
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        PopulateEyes();
    }

    private void OnDisable()
    {
        if (playerPlatform != null)
        {
            Destroy(playerPlatform);
            playerPlatform = null;
        }
        DePopulate();
    }

    public void PopulateEyes()
    {
        Populate(EquipmentType.EYEWEAR, equipmentsConfig.Eyes, eyeBtn);
    }
    public void PopulateHead()
    {
        Populate(EquipmentType.HAT, equipmentsConfig.Head, headBtn);
    }
    public void PopulateMouth()
    {
        Populate(EquipmentType.MOUTH, equipmentsConfig.Mouth, mouthBtn);
    }
    public void PopulateBody()
    {
        Populate(EquipmentType.BODY, equipmentsConfig.Body, bodyBtn);
    }
    public void PopulateTail()
    {
        Populate(EquipmentType.TAIL, equipmentsConfig.TailsOverlay, tailBtn);
        Populate(EquipmentType.TAIL, equipmentsConfig.TailsFloating, tailBtn, false);
        Populate(EquipmentType.TAIL, equipmentsConfig.TailsAnimated, tailBtn, false);
    }

    private void Populate(EquipmentType eqType, object elements, ButtonHoverable targetBtn, bool shouldClean = true)
    {
        currentType = eqType;

        selectedEquipment = null;

        selectedBtn?.Deselect();
        selectedBtn = targetBtn;
        selectedBtn.Select();

        if (shouldClean)
        {
            foreach (Transform t in content)
            {
                Destroy(t.gameObject);
                equipments.Clear();
            }
        }

        Equipment equippedItem = null;
        if (playerCustomization.playerEquipmentConfig.ContainsKey(eqType))
        {
            equippedItem = playerCustomization.playerEquipmentConfig[eqType];
        }

        if (elements is List<EquipmentData> spriteEls)
        {
            Populate(equippedItem, spriteEls);
        }
    }
    
    private void Populate(Equipment equippedItem, List<EquipmentData> elements)
    {
        foreach (EquipmentData el in elements)
        {
            if (!DataManager.Instance.PlayerData.OwnedEquiptables.Contains((Convert.ToInt32(el.Id))))
            {
                continue;
            }
            var go = GameObject.Instantiate(nftPrefab, content);
            var nftImageSprite = go.GetComponent<NFTImageSprite>();
            nftImageSprite.mainImage.sprite = el.Thumbnail;
            equipments.Add(nftImageSprite);

            if (equippedItem != null && equippedItem is SpriteEquipment spriteItem && el.Thumbnail == spriteItem.sprite)
            {
                Debug.Log("Found match for " + spriteItem.sprite.name);
                nftImageSprite.Select();
                selectedEquipment = nftImageSprite;
            }

            int idx = equipments.Count - 1;
            equipments[idx].onClick += () => OnEquipmentSelected(idx);
        }
    }

    private void OnEquipmentSelected(int idx, EquipmentData idPair = null)
    {
        selectedEquipment?.Deselect();
        selectedEquipment = equipments[idx];
        selectedEquipment.Select();

        if (equipments[idx].mainImage.sprite.name != "none")
        {
            if (idPair != null)
            {
                playerCustomization.SetTail(idPair.Id.ToString());
            }
            else
            {
                playerCustomization.SetEquipmentBySprite(currentType, equipments[idx].mainImage.sprite);
            }
        }
        else
        {
            playerCustomization.SetEquipmentBySprite(currentType, null);
        }
    }

    private void DePopulate()
    {
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
            equipments.Clear();
        }
    }

    public void SaveAndClose()
    {
        playerCustomization.SaveCustomConfig();
        lobbyUIManager.OpenGameMenu();
    }

    public void ResetConfig()
    {
        playerCustomization.ResetToOriginal();
    }
}
