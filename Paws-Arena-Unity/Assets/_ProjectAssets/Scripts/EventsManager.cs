using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static Action OnWonGame;
    public static Action OnWonGameWithFullHp;
    public static Action OnLostGame;
    public static Action OnCraftedItem;
    public static Action OnCraftedCrystal;
    public static Action OnUsedMilkBottle;
    public static Action OnHealedKitty;
    public static Action OnPlayedMatch;
    public static Action OnUsedRocket;
    public static Action OnUsedCannon;
    public static Action OnUsedTripleRocket;
    public static Action OnUsedAirplane;
    public static Action OnUsedMouse;
    public static Action OnUsedArrow;
    public static Action OnWonWithHpLessThan10;
    public static Action OnWonWithHpLessThan20;
    public static Action OnWonWithHpLessThan30;
    public static Action<int> OnDealtDamageToOpponent;
    public static Action<int> OnGotExperience;
    public static Action<int> OnWonLeaderboardPoints;
}
