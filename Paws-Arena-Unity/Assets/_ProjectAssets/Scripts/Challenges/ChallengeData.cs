using System;
using Newtonsoft.Json;

[Serializable]
public class ChallengeData
{
    public static Action<int> UpdatedProgress;
    public int Id;
    public int Value;
    public bool Completed;
    public bool Claimed;


    [JsonIgnore] public bool IsSubscribed;

    public void IncreaseAmount()
    {
        if (Completed)
        {
            return;
        }

        Value++;
        CheckIfCompleted();
    }

    public void IncreaseAmount(int _amount)
    {
        if (Completed)
        {
            return;
        }

        Value+=_amount;
        CheckIfCompleted();
    }
    
    private void CheckIfCompleted()
    {
        ChallengeSO _challenge = ChallengesManager.Instance.Get(Id);
        if (_challenge.AmountNeeded-Value<=0)
        {
            Completed = true;
        }
        else
        {
            UpdatedProgress?.Invoke(Id);
        }
    }

    public void Reset()
    {
        Value = 0;
        UpdatedProgress?.Invoke(Id);
    }
    
}