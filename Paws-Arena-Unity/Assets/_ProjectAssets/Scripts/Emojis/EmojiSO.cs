using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEmoji", menuName = "ScriptableObjects/Emoji")]
public class EmojiSO : ScriptableObject
{
    [field: SerializeField] public int Id;
    [field: SerializeField] public string Name;
    [field: SerializeField] public Sprite Sprite;

    private static List<EmojiSO> allEmojis;

    public static EmojiSO Get(int _id)
    {
        if (allEmojis==null)
        {
            LoadAllEmojis();
        }

        return allEmojis?.Find(_element => _element.Id == _id);
        
    }

    private static void LoadAllEmojis()
    {
        allEmojis = Resources.LoadAll<EmojiSO>("Emojis/").ToList();
    }
}
