using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGuild", menuName = "ScriptableObjects/Guild")]
public class GuildSO : ScriptableObject
{
   [field: SerializeField] public int Id { get; private set; }
   [field: SerializeField] public Sprite Badge { get; private set; }
   [field: SerializeField] public Sprite Kitty { get; private set; }
   [field: SerializeField] public Sprite SelectedKitty { get; private set; }

   private static List<GuildSO> allGuilds;

   public static GuildSO Get(int _id)
   {
      LoadAll();

      return allGuilds.Find(_element => _element.Id == _id);
   }

   public static List<GuildSO> GetAll()
   {
      LoadAll();

      return allGuilds;
   }

   public static void LoadAll()
   {
      if (allGuilds == null)
      {
         allGuilds = Resources.LoadAll<GuildSO>("Guilds").ToList();
      }
   }
}
