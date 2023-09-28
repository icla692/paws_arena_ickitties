using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardUIManager : MonoBehaviour
{
    public LeaderboardData leaderboardData;
    [Header("UI Components")]
    public Transform leaderboardContent;
    public GameObject leaderboardLinePrefab;

    [Header("Places")]
    public TMPro.TextMeshProUGUI firstPlacePoints;
    public TMPro.TextMeshProUGUI secondPlacePoints;
    public TMPro.TextMeshProUGUI thirdPlacePoints;

    public PlayerPlatformBehaviour firstPlayer;
    public PlayerPlatformBehaviour secondPlayer;
    public PlayerPlatformBehaviour thirdPlayer;

    public List<Sprite> stars;

    // Start is called before the first frame update
    private void Start()
    {
        PopulateLeaderboard();
    }

    private async void PopulateLeaderboard()
    {
        LeaderboardGetResponseEntity data = await leaderboardData.GetLeaderboard();

        int idx = 0;
        foreach(PlayerStatsEntity playerStats in data.leaderboard)
        {
            GameObject go = Instantiate(leaderboardLinePrefab, leaderboardContent);
            go.GetComponent<LeaderboardLineBehaviour>().SetPrincipalId(playerStats.principalId, idx);
            go.transform.Find("HorizontalLayout/Points").GetComponent<TMPro.TextMeshProUGUI>().text = "" + playerStats.points;
            go.transform.Find("HorizontalLayout/Nickname").GetComponent<TMPro.TextMeshProUGUI>().text = playerStats.nickname;

            if (idx < stars.Count)
            {
                go.transform.Find("HorizontalLayout/Icon_Text").gameObject.SetActive(false);
                go.transform.Find("HorizontalLayout/Icon").GetComponent<Image>().sprite = stars[idx];
            }
            else
            {
                go.transform.Find("HorizontalLayout/Icon").gameObject.SetActive(false);
                go.transform.Find("HorizontalLayout/Icon_Text").GetComponent<TMPro.TextMeshProUGUI>().text ="" + (idx + 1);
                //go.transform.Find("HorizontalLayout/Icon").GetComponent<Image>().sprite = null;
                //go.transform.Find("HorizontalLayout/Icon").GetComponent<Image>().color = Color.clear;
            }

            idx++;
        }

        if (data.leaderboard.Count >= 1)
        {
            firstPlacePoints.text = "" + data.leaderboard[0]?.points;
            firstPlayer.SetCat(data.first);
        }

        if(data.leaderboard.Count >= 2)
        {
            secondPlacePoints.text = "" + data.leaderboard[1]?.points;
            secondPlayer.SetCat(data.second);
        }

        if (data.leaderboard.Count >= 3)
        {
            thirdPlacePoints.text = "" + data.leaderboard[2]?.points;
            thirdPlayer.SetCat(data.third);
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
}
