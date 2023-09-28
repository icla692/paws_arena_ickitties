using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewRecepie", menuName = "ScriptableObjects/CraftingRecepie")]

public class CraftingRecepieSO : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public ItemType Inggrdiant { get; private set; }
    [field: SerializeField] public int AmountNeeded { get; private set; }
    [field: SerializeField] public string IngridiantColor { get; private set; }
    [field: SerializeField] public Sprite IngridiantSprite { get; private set; }
    [field: SerializeField] public ItemType EndProduct { get; private set; }
    [field: SerializeField] public string EndProductColor { get; private set; }
    [field: SerializeField] public float FusionTime { get; private set; }
    [field: SerializeField] public Sprite EndProductSprite { get; private set; }
    [field: SerializeField] public Sprite TopOfferBackground { get; private set; }
    [field: SerializeField] public Sprite BottomOfferBackground { get; private set; }
    [field: SerializeField] public int BotAmountNeeded { get; private set; }

    private static List<CraftingRecepieSO> allRecepies;

    private static void LoadAll()
    {
        if (allRecepies != null)
        {
            return;
        }

        allRecepies = Resources.LoadAll<CraftingRecepieSO>("CraftingRecepies").ToList();
    }

    public static CraftingRecepieSO Get(ItemType _ingridiant)
    {
        LoadAll();
        return allRecepies.Find(element => element.Inggrdiant == _ingridiant);
    }
    
}
