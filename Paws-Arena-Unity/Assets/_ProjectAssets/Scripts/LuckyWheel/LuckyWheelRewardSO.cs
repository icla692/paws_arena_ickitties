using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWheelReward", menuName = "ScriptableObjects/WheelReward")]
public class LuckyWheelRewardSO : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public float Amount { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int Chances { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float MinRotation { get; private set; }
    [field: SerializeField] public float MaxRotation { get; private set; }

    private static List<LuckyWheelRewardSO> allRewards;

    public static List<LuckyWheelRewardSO> GetAll()
    {
        LoadAllRewards();
        return allRewards.ToList();
    }

    public static LuckyWheelRewardSO GetReward()
    {
        LoadAllRewards();

        List<int> _listOfRewardIds = new List<int>();
        foreach (var _reward in allRewards)
        {
            for (int i = 0; i < _reward.Chances; i++)
            {
                _listOfRewardIds.Add(_reward.Id);
            }
        }
        _listOfRewardIds = _listOfRewardIds.OrderBy(element => System.Guid.NewGuid()).ToList();
        int _randomId = _listOfRewardIds[Random.Range(0, _listOfRewardIds.Count)];
        return Get(_randomId);
    }

    public static LuckyWheelRewardSO Get(int _id)
    {
        LoadAllRewards();
        return allRewards.First(element => element.Id == _id);
    }

    private static void LoadAllRewards()
    {
        if (allRewards != null)
        {
            return;
        }

        allRewards = Resources.LoadAll<LuckyWheelRewardSO>("WheelRewards/").ToList();
    }
}
