using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseTokenManager : MonoBehaviour
{
    private string apiKey;
    private string refreshToken;
    private const float checkInterval = 60f; // Check interval in seconds

    private Coroutine tokenCheckCoroutine;

    public void Setup(string _apiKey, string _refreshToken)
    {
        apiKey = _apiKey;
        refreshToken = _refreshToken;
        if (tokenCheckCoroutine != null)
        {
            StopCoroutine(tokenCheckCoroutine);
        }
        tokenCheckCoroutine = StartCoroutine(TokenCheckCoroutine());
    }

    private void OnDestroy()
    {
        if (tokenCheckCoroutine != null)
        {
            StopCoroutine(tokenCheckCoroutine);
        }
    }

    private IEnumerator TokenCheckCoroutine()
    {
        while (true)
        {
            yield return CheckAndRenewToken();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private IEnumerator CheckAndRenewToken()
    {
        var _requestForm = new WWWForm();
        _requestForm.AddField("grant_type", "refresh_token");
        _requestForm.AddField("refresh_token", refreshToken);

        UnityWebRequest _request = UnityWebRequest.Post(
            $"https://securetoken.googleapis.com/v1/token?key={apiKey}",
            _requestForm);

        yield return _request.SendWebRequest();

        if (_request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error checking token: " + _request.error);
            yield break;
        }

        var _responseJson = _request.downloadHandler.text;
        var _responseData = JsonUtility.FromJson<TokenResponse>(_responseJson);

        _request.Dispose();
    }


    [System.Serializable]
    private class TokenResponse
    {
        public int expires_in;
        // Add any additional fields from the response if needed
    }
}
