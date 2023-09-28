using UnityEngine;

[CreateAssetMenu(fileName = "NewChallenge", menuName = "ScriptableObjects/Challenge")]
public class ChallengeSO : ScriptableObject
{
    public int Id;
    public string Description;
    public int AmountNeeded;
    public Sprite RewardSprite;
    public int RewardAmount;
    public ChallengeRewardType RewardType;
    public ChallengeCategory Category;
}
