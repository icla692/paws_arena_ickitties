using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChallengesManager : MonoBehaviour
{
    public static ChallengesManager Instance;

    private readonly int amountOfChallenges=3;

    private bool isInit;
    private bool isSubscribed;
    private List<ChallengeSO> allChallenges = new List<ChallengeSO>();

    private void OnEnable()
    {
        ChallengeDisplay.OnClaimPressed += ClaimedChallenge;
    }

    private void OnDisable()
    {
        ChallengeDisplay.OnClaimPressed -= ClaimedChallenge;
    }
    
    public void ClaimedChallenge(ChallengeData _challengeData)
    {
        ChallengeSO _challengeSO = allChallenges.Find(_element => _element.Id == _challengeData.Id);
        _challengeData.Claimed = true;
        switch (_challengeSO.RewardType)
        {
            case ChallengeRewardType.SeasonExperience:
                if (DataManager.Instance.GameData.SeasonEnds<DateTime.Now)
                {
                    return;
                }
                DataManager.Instance.PlayerData.Experience += _challengeSO.RewardAmount;
                break;
            case ChallengeRewardType.JugOfMilk:
                DataManager.Instance.PlayerData.JugOfMilk += _challengeSO.RewardAmount;
                break;
            case ChallengeRewardType.Snacks:
                DataManager.Instance.PlayerData.Snacks += _challengeSO.RewardAmount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        DataManager.Instance.SaveChallenges();
    }


    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            allChallenges = Resources.LoadAll<ChallengeSO>("Challenges").ToList();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        if (isInit)
        {
            return;
        }

        isInit = true;
        StartCoroutine(CheckForReset());
    }

    private IEnumerator CheckForReset()
    {
        if (DateTime.UtcNow>DataManager.Instance.PlayerData.Challenges.NextReset)
        {
            GenerateNewChallenges();
        }
        else
        {
            SubscribeEvents();
        }

        yield return new WaitForSeconds(1);
    }

    private void SubscribeEvents()
    {
        if (isSubscribed)
        {
            return;
        }

        isSubscribed = true;
        foreach (var _challengeDataDB in DataManager.Instance.PlayerData.Challenges.ChallengesData)
        {
            if (_challengeDataDB.Completed)
            {
                continue;
            }

            ChallengeSO _challengeData = allChallenges.Find(_element => _element.Id == _challengeDataDB.Id);
            if (_challengeData.Category == ChallengeCategory.WinGame)
            {
                EventsManager.OnWonGame += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.CraftItem)
            {
                EventsManager.OnCraftedItem += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.CraftShard)
            {
                EventsManager.OnCraftedCrystal += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.GainExperience)
            {
                EventsManager.OnGotExperience += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.GainLeaderboardPoints)
            {
                EventsManager.OnWonLeaderboardPoints += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.WinWithFullHp)
            {
                EventsManager.OnWonGameWithFullHp += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.LoseMatch)
            {
                EventsManager.OnLostGame += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.DealDamage)
            {
                EventsManager.OnDealtDamageToOpponent += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.UseMilkBottle)
            {
                EventsManager.OnUsedMilkBottle += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.HealYourKitty)
            {
                EventsManager.OnHealedKitty += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.PlayMatch)
            {
                EventsManager.OnPlayedMatch += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootRocket)
            {
                EventsManager.OnUsedRocket += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootCannon)
            {
                EventsManager.OnUsedCannon += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootTripleRocket)
            {
                EventsManager.OnUsedTripleRocket += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootPlane)
            {
                EventsManager.OnUsedAirplane += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootMouse)
            {
                EventsManager.OnUsedMouse += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootArrow)
            {
                EventsManager.OnUsedArrow += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.WinMatchesInARow)
            {
                EventsManager.OnWonGame += _challengeDataDB.IncreaseAmount;
                EventsManager.OnLostGame += _challengeDataDB.Reset;
            }
            else if (_challengeData.Category == ChallengeCategory.WinMatchWithLessThan10Hp)
            {
                EventsManager.OnWonWithHpLessThan10 += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.WinMatchWithLessThan20Hp)
            {
                EventsManager.OnWonWithHpLessThan20 += _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.WinMatchWithLessThan30Hp)
            {
                EventsManager.OnWonWithHpLessThan30 += _challengeDataDB.IncreaseAmount;
            }

            _challengeDataDB.IsSubscribed = true;
        }
    }

    private void UnsubscribeEvents()
    {
        if (!isSubscribed)
        {
            return;
        }

        isSubscribed = false;
        foreach (var _challengeDataDB in DataManager.Instance.PlayerData.Challenges.ChallengesData)
        {
            if (_challengeDataDB.Completed)
            {
                continue;
            }
            ChallengeSO _challengeData = allChallenges.Find(_element => _element.Id == _challengeDataDB.Id);
            if (_challengeData.Category == ChallengeCategory.WinGame)
            {
                EventsManager.OnWonGame -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.CraftItem)
            {
                EventsManager.OnCraftedItem -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.CraftShard)
            {
                EventsManager.OnCraftedCrystal -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.GainExperience)
            {
                EventsManager.OnGotExperience -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.GainLeaderboardPoints)
            {
                EventsManager.OnWonLeaderboardPoints -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.WinWithFullHp)
            {
                EventsManager.OnWonGameWithFullHp -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.LoseMatch)
            {
                EventsManager.OnLostGame -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.DealDamage)
            {
                EventsManager.OnDealtDamageToOpponent -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.UseMilkBottle)
            {
                EventsManager.OnUsedMilkBottle -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.HealYourKitty)
            {
                EventsManager.OnHealedKitty -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.PlayMatch)
            {
                EventsManager.OnPlayedMatch -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootRocket)
            {
                EventsManager.OnUsedRocket -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootCannon)
            {
                EventsManager.OnUsedCannon -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootTripleRocket)
            {
                EventsManager.OnUsedTripleRocket -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootPlane)
            {
                EventsManager.OnUsedAirplane -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootMouse)
            {
                EventsManager.OnUsedMouse -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.ShootArrow)
            {
                EventsManager.OnUsedArrow -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.WinMatchesInARow)
            {
                EventsManager.OnWonGame -= _challengeDataDB.IncreaseAmount;
                EventsManager.OnLostGame -= _challengeDataDB.Reset;
            }
            else if (_challengeData.Category == ChallengeCategory.WinMatchWithLessThan10Hp)
            {
                EventsManager.OnWonWithHpLessThan10 -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.WinMatchWithLessThan20Hp)
            {
                EventsManager.OnWonWithHpLessThan20 -= _challengeDataDB.IncreaseAmount;
            }
            else if (_challengeData.Category == ChallengeCategory.WinMatchWithLessThan30Hp)
            {
                EventsManager.OnWonWithHpLessThan30 -= _challengeDataDB.IncreaseAmount;
            }

            _challengeDataDB.IsSubscribed = false;
        }
    }

    private void GenerateNewChallenges()
    {
        UnsubscribeEvents();
        List<ChallengeSO> _allChallenges = allChallenges.ToList().OrderBy(_element => Guid.NewGuid()).ToList();
        DataManager.Instance.PlayerData.Challenges.ChallengesData = new List<ChallengeData>();

        int _counter = -1;
        while (DataManager.Instance.PlayerData.Challenges.ChallengesData.Count<amountOfChallenges)
        {
            bool _skip = false;
            _counter++;

            ChallengeSO _challenge = _allChallenges[_counter];

            foreach (var _chData in DataManager.Instance.PlayerData.Challenges.ChallengesData)
            {
                ChallengeSO _challengeSO = _allChallenges.Find(_element => _element.Id == _chData.Id);
                if (_challenge.Category==_challengeSO.Category)
                {
                    _skip = true;
                    break;
                }
            }

            if (_skip)
            {
                continue;
            }

            ChallengeData _challengeData = new ChallengeData()
            {
                Id = _challenge.Id,
                Completed = false,
                Claimed = false,
                Value = 0
            };
            DataManager.Instance.PlayerData.Challenges.ChallengesData.Add(_challengeData);    
        }
        

        DateTime _nextReset = DateTime.UtcNow.AddDays(1);

        DataManager.Instance.PlayerData.Challenges.ClaimedLuckySpin = false;
        DataManager.Instance.PlayerData.Challenges.NextReset =
            new DateTime(_nextReset.Year, _nextReset.Month, _nextReset.Day, 0, 0, 0);
        DataManager.Instance.SaveChallenges();
        SubscribeEvents();
    }


    public ChallengeSO Get(int _id)
    {
        return allChallenges.Find(_element => _element.Id == _id);
    }
}
