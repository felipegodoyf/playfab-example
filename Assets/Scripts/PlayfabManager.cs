using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    #region SINGLETON

    public static PlayfabManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion
    #region LOGIN

    void Start()
    {
        Login();
    }

    public void Login()
    {
        Debug.Log("PlayFab: Login...");
        PlayFabClientAPI.LoginWithCustomID
        (
            new LoginWithCustomIDRequest()
            {
                CreateAccount = true,
                CustomId = SystemInfo.deviceUniqueIdentifier
            },
            response =>
            {
                Debug.Log("PlayFab: Logged in.");
            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                Login();
            }
        );
    }

    #endregion
    #region EVENTS

    public void WritePlayerEvent(string eventName, Dictionary<string, object> eventData)
    {
        Debug.Log($"PlayFab: WritePlayerEvent '{eventName}'...");
        PlayFabClientAPI.WritePlayerEvent
        (
            new WriteClientPlayerEventRequest()
            {
                EventName = eventName,
                Body = eventData
            },
            response =>
            {
                Debug.Log($"PlayFab: WritePlayerEvent '{eventName}' success");
            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
            }
        );
    }

    #endregion
}
