using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
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
}
