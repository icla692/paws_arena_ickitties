using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using UnityEngine;

public class PlayerNicknameButton : MonoBehaviour
{
    public event Action<string> OnPlayerNameUpdated;

    [SerializeField]
    private TMPro.TextMeshProUGUI nicknameText;
    [SerializeField]
    private InputModal inputModal;
    [SerializeField]
    private NFTSelection_LoadingManager screenLoadingManager;


    private async void Start()
    {
        //screenLoadingManager.AddLoadingReason("Grabbing account information...");
        string nickname = await TryGetNickname(() => { });

        nicknameText.text = "";
        if (!string.IsNullOrEmpty(nickname.Trim()))
        {
            SetPlayerName(nickname);
        }
        else
        {
            EnableEdit(false);
        }

        //screenLoadingManager.StopLoadingReason("Grabbing account information...");
        OnPlayerNameUpdated?.Invoke(nicknameText.text);
    }

    public void EnableEdit(bool isCancelable)
    {
        inputModal.Show("Nickname", "Nickname", isCancelable, (nickname) =>
        {
            SendNewNicknameToServer(nickname);
        });
    }

    private async UniTask<string> TryGetNickname(Action onError)
    {
        try
        {
            string resp = await NetworkManager.GETRequestCoroutine("/user/nickname",
            (code, err) =>
            {
                Debug.LogWarning($"Failed getting nickname {err} : {code}");
                onError?.Invoke();
            }, true);

            return resp;
        }
        catch (UnityWebRequestException ex)
        {
            Debug.LogWarning($"Failed getting nickname form server {ex.ResponseCode} : {ex.Text}");
            return "";
        }
    }

    private async void SendNewNicknameToServer(string nickname)
    {
        try
        {
            await NetworkManager.POSTRequest("/user/nickname", $"\"{nickname}\"", (resp) =>
            {
                Debug.Log($"Saved nickname {nickname}");
                SetPlayerName(nickname);
                inputModal.Hide();
            }, (code, err) =>
            {
                Debug.LogWarning($"Failed saving nickname {err} : {code}");
            }, true);
        }
        catch (UnityWebRequestException err)
        {
            Debug.LogWarning($"Problem setting nickname {err.ResponseCode} : {err.Text}");
            if (err.ResponseCode == 0)
            {
                inputModal.SetError("We cannot reach the server :(");
            }
            else
            {
                inputModal.SetError(err.Text);
            }
        }
    }

    public void SetPlayerName(string value)
    {
        GameState.nickname = PhotonNetwork.NickName = nicknameText.text = value;
        OnPlayerNameUpdated?.Invoke(value);
    }
}
