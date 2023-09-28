using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    [SerializeField] private FirebaseTokenManager tokenManager;

    private const string WEB_API_KEY = "AIzaSyCFi-0SrkVoZbzv3GzxacoJBtELJdeFTt8";

    private string userLocalId;
    private string userIdToken;
    private string projectLink = "https://pawsarena-b8133-default-rtdb.firebaseio.com/";
    private string userDataLink => $"{projectLink}/users/{userLocalId}/";
    private string gameDataLink => $"{projectLink}/gameData/";
    private string guildsLink => $"{projectLink}guilds/";

    public string PlayerId => userLocalId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TryLoginAndGetData(string _principalId, Action<bool> _callBack)
    {
        if (!string.IsNullOrEmpty(userLocalId))
        {
            _callBack?.Invoke(true);
            return;
        }

        _principalId = _principalId.Replace("-","");
        string _loginParms = "{\"email\":\"" + _principalId + "@paws.arena\",\"password\":\"" + GeneratePassword(_principalId) + "\",\"returnSecureToken\":true}";
        StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + WEB_API_KEY, _loginParms, (_result) =>
        {
            SignInResponse _signInResponse = JsonConvert.DeserializeObject<SignInResponse>(_result);
            userIdToken = _signInResponse.IdToken;
            userLocalId = _signInResponse.LocalId;
            tokenManager.Setup(WEB_API_KEY, _signInResponse.RefreshToken);
            CollectGameData(_callBack);
        }, (_result) =>
        {
            Register(_callBack,_loginParms);
        }, false));
    }

    private string GeneratePassword(string _principalId)
    {
        string _password = string.Empty;
        _password += _principalId[5];
        _password += _principalId[0];
        _password += _principalId[2];
        _password += _principalId[7];
        _password += _principalId[3];
        _password += _principalId[5];
        _password += _principalId[5];
        _password += _principalId[1];
        _password += _principalId[6];
        _password += _principalId[6];
        _password += _principalId[0];
        _password += _principalId[5];

        return _password;
    }

    private void Register(Action<bool> _callBack,string _parms)
    {
        StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + WEB_API_KEY, _parms, (_result) =>
        {
            RegisterResponse _registerResult = JsonConvert.DeserializeObject<RegisterResponse>(_result);
            userIdToken = _registerResult.IdToken;
            userLocalId = _registerResult.LocalId;
            tokenManager.Setup(WEB_API_KEY, _registerResult.RefreshToken);
            DataManager.Instance.CreatePlayerDataEmpty();
            SetStartingData(_callBack);
        }, (_result) =>
        {
            Debug.Log("Register failed");
            _callBack?.Invoke(false);
        }));
    }

    private void SetStartingData(Action<bool> _callBack)
    {
        string _data = JsonConvert.SerializeObject(DataManager.Instance.PlayerData);
        StartCoroutine(Put(userDataLink+"/.json", _data, (_result) =>
        {
            CollectGameData(_callBack);
        }, (_result) =>
        {
            _callBack?.Invoke(false);
        }));
    }

    private void CollectGameData(Action<bool> _callBack)
    {
        StartCoroutine(Get(gameDataLink + ".json", (_result) =>
        {
            DataManager.Instance.SetGameData(_result);
            CollectPlayerData(_callBack);
        }, (_result) =>
        {
            _callBack?.Invoke(false);
        }));
    }

    private void CollectPlayerData(Action<bool> _callBack)
    {
        StartCoroutine(Get(userDataLink + "/.json", (_result) =>
        {
            DataManager.Instance.SetPlayerData(_result);
            CollectGuildData(_callBack);
        }, (_result) =>
        {
            _callBack?.Invoke(false);
        }));
    }
    
    private void CollectGuildData(Action<bool> _callBack)
    {
        CollectGuilds(SetGuildData);

        void SetGuildData(Dictionary<string, GuildData> _guilds)
        {
            DataManager.Instance.SetGuildsData(_guilds);
            _callBack?.Invoke(true);
        }
    }

    public void ValidateGuildName(string _name, Action _onValid, Action _onInvalid)
    {
        CollectGuilds(ValidateName);

        void ValidateName(Dictionary<string, GuildData> _guilds)
        {
            DataManager.Instance.SetGuildsData(_guilds);
            if (_guilds==null)
            {
                _onValid?.Invoke();
                return;
            }
            
            foreach (var (_key,_guild) in _guilds)
            {
                if (_guild.Name==_name)
                {
                    _onInvalid?.Invoke();
                    return;
                }
            }
            
            _onValid?.Invoke();
        }
    }
    
    private void CollectGuilds(Action<Dictionary<string, GuildData>> _callBack)
    {
        StartCoroutine(Get(guildsLink + ".json", (_result) =>
        {
            if (string.IsNullOrEmpty(_result))
            {
                _callBack?.Invoke(null);
            }
            _callBack?.Invoke(JsonConvert.DeserializeObject<Dictionary<string, GuildData>>(_result));
        }, (_result) =>
        {
            throw new Exception("Failed to fetch guilds");
        }));
    }
    
    private void CollectGuildData(string _guildId,Action<GuildData> _callBack)
    {
        StartCoroutine(Get(guildsLink +_guildId +"/.json", (_result) =>
        {
            if (string.IsNullOrEmpty(_result))
            {
                _callBack?.Invoke(null);
            }
            _callBack?.Invoke(JsonConvert.DeserializeObject<GuildData>(_result));
        }, (_result) =>
        {
            throw new Exception("Failed to fetch guilds");
        }));
    }
    
    public void CreateGuild(GuildData _data)
    {
        string _jsonData = "{\""+_data.Id+"\":"+JsonConvert.SerializeObject(_data)+"}";
        StartCoroutine(Patch(guildsLink+"/.json", _jsonData, (_result) =>
        {

        }, (_result) =>
        {
            Debug.Log(_jsonData);
            Debug.Log("Failed to update data, please try again later");
            Debug.Log(_result);
        }));
    }

    public void SaveValue<T>(string _path, T _value)
    {
        string _valueString = "{\"" + _path + "\":" + _value + "}";
        StartCoroutine(Patch(userDataLink + ".json", _valueString, (_result) =>
        {

        }, (_result) =>
        {
            Debug.Log(_valueString);
            Debug.Log("Failed to update data, please try again later");
            Debug.Log(_result);
        }));
    }
    
    public void SaveString(string _path, string _value)
    {
        string _valueString = "{\"" + _path + "\":\"" + _value + "\"}";
        StartCoroutine(Patch(userDataLink + ".json", _valueString, (_result) =>
        {

        }, (_result) =>
        {
            Debug.Log("Failed to update data, please try again later");
            Debug.Log(_result);
        }));
    }

    public void CheckIfPlayerIsStillInGuild(Action _callBack)
    {
        if (string.IsNullOrEmpty(DataManager.Instance.PlayerData.GuildId))
        {
            _callBack?.Invoke();
            return;
        }
        CollectGuildData(DataManager.Instance.PlayerData.GuildId,Check);

        void Check(GuildData _guildData)
        {
            DataManager.Instance.GameData.Guilds[DataManager.Instance.PlayerData.GuildId] = _guildData;
            StartCoroutine(Get(userDataLink + "/.json", (_result) =>
            {
                PlayerData _playerData = JsonConvert.DeserializeObject<PlayerData>(_result);
                DataManager.Instance.PlayerData.GuildId = _playerData.GuildId;
                _callBack?.Invoke();
            }, (_result) =>
            {
                Debug.Log("Failed to check if player is still in guild, please try again later");
                Debug.Log(_result);
            }));
        }
    }

    public void RemovePlayerFromGuild(string _playerId, string _guildId)
    {
        GuildData _guild = DataManager.Instance.GameData.Guilds[_guildId];
        GuildPlayerData _player= _guild.GetPlayer(_playerId);

        if (_player==null)
        {
            return;
        }

        _guild.KickPlayer(_playerId);
        SaveGuild(_guildId,JsonConvert.SerializeObject(_guild),null);
    }
    
    public void JoinGuild(string _playerId, string _guildId, Action _callBack)
    {
        CollectGuildData(_guildId,Join);

        void Join(GuildData _guilds)
        {
            if (_guilds == null)
            {
                if (DataManager.Instance.GameData.Guilds.ContainsKey(_guildId))
                {
                    DataManager.Instance.GameData.Guilds.Remove(_guildId);
                }
                _callBack?.Invoke();
                return;
            }
            GuildPlayerData _myData = new GuildPlayerData()
            {
                IsLeader = false,
                Name = GameState.nickname,
                Points = DataManager.Instance.PlayerData.Points,
                Level = DataManager.Instance.PlayerData.Level,
                Id = _playerId
            };
            
            DataManager.Instance.GameData.Guilds[_guildId].Players.Add(_myData);
            DataManager.Instance.PlayerData.GuildId = _guildId;
            string _jsonData = JsonConvert.SerializeObject(DataManager.Instance.PlayerData.Guild);
            SaveGuild(_guildId, _jsonData, () =>
            {
                _callBack?.Invoke();
            });
        }
    }

    public void SaveGuild(string _guildId, string _guildData, Action _callBack)
    {
        StartCoroutine(Patch(guildsLink+_guildId+"/.json", _guildData,(_result) =>
        {
            _callBack?.Invoke();
        }, (_result) =>
        {
            Debug.Log(_result);
            _callBack?.Invoke();
        }));
    }

    public void SetNewGuildLeader(string _playerId,string _guildId, Action _callBack)
    {
        GuildData _guild = DataManager.Instance.GameData.Guilds[_guildId];
        GuildPlayerData _playerData = _guild.GetPlayer(_playerId);
        _playerData.IsLeader = true;
            
        SaveGuild(_guildId, JsonConvert.SerializeObject(_guild), () =>
        {
            _callBack?.Invoke();
        });
    }
    
    public void DeleteGuild()
    {
        StartCoroutine(Delete(guildsLink +DataManager.Instance.PlayerData.GuildId+ "/.json", (_result) =>
        {
        }, (_result) =>
        {
            Debug.Log("Failed to delete guild");
            Debug.Log(_result);
        }));
    }

    public void UpdateValue<T>(string _path, T _value)
    {
        string _valueString = "{\"" + _path + "\":" + _value + "}";

        StartCoroutine(Patch(userDataLink + ".json", _valueString, (_result) =>
        {

        }, (_result) =>
        {
            Debug.Log("Failed to update data, please try again later");
            Debug.Log(_result);
        }));
    }

    
    private IEnumerator Get(string uri, Action<string> onSuccess, Action<string> onError)
    {
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
                onError?.Invoke(webRequest.error);
            }

            webRequest.Dispose();
        }
    }

    private IEnumerator Post(string uri, string jsonData, Action<string> onSuccess, Action<string> onError, bool _includeHeader = true)
    {
        if (userIdToken != null)
        {
            if (_includeHeader)
            {
                uri = $"{uri}?auth={userIdToken}";
            }
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
                onError?.Invoke(webRequest.error);
            }

            webRequest.uploadHandler.Dispose();
            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }

    private IEnumerator Put(string uri, string jsonData, Action<string> onSuccess, Action<string> onError)
    {
        // If the userIdToken is available, append it to the URI
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
                onError?.Invoke(webRequest.error);
            }

            webRequest.uploadHandler.Dispose();
            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }

    private IEnumerator Patch(string uri, string jsonData, Action<string> onSuccess, Action<string> onError)
    {
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "PATCH"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();


            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(webRequest.error);
            }

            webRequest.uploadHandler.Dispose();
            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }
    
    private IEnumerator Delete(string uri, Action<string> onSuccess, Action<string> onError)
    {
        if (userIdToken != null)
        {
            uri = $"{uri}?auth={userIdToken}";
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Delete(uri))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(webRequest.error);
            }

            webRequest.downloadHandler.Dispose();
            webRequest.Dispose();
        }
    }

}
