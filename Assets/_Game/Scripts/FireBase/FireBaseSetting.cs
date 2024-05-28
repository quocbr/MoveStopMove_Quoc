using Firebase;
using Firebase.Extensions;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class FireBaseSetting : Singleton<FireBaseSetting>
{
    [SerializeField] private string fieBaseLink = "https://alalys-default-rtdb.asia-southeast1.firebasedatabase.app/";
    private FirebaseApp app;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        try
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    public void PutToDatabase(UserData userData)
    {
        string json = JsonUtility.ToJson(userData);
        RestClient.Put(fieBaseLink + userData.userID + ".json", json).Then(response =>
        {
            Debug.Log("Data posted successfully: " + response.Text);
        }).Catch(error =>
        {
            Debug.LogError("Error posting data: " + error.Message);
        });

    }
    public void GetToDatabase(string gamingName)
    {
        StartCoroutine(FetchUserDataCoroutine(gamingName));
        
        //RestClient.Get<UserData>(fieBaseLink + gamingName + ".json").Then(callback =>
        //    {
        //        //SaveLoadManager.Ins.LoadToFile(callback);
        //        //Debug.Log(callback.userName + ": " + callback.Coin);
        //        SaveLoadManager.Ins.LoadToFile(callback);
        //    }).Catch(error =>
        //    {
        //        Debug.LogError("Error get data: " + error.Message);
        //    });
    }

    public Task GetUserDataAsync(string userId)
    {
        TaskCompletionSource<UserData> tcs = new TaskCompletionSource<UserData>();
        string url = $"{fieBaseLink}{userId}.json";

        RestClient.Get<UserData>(url).Then(response =>
        {
            tcs.SetResult(response);
            SaveLoadManager.Ins.LoadToFile(response);
        }).Catch(error =>
        {
            tcs.SetException(error);
        });

        return tcs.Task;
    }

    public IEnumerator FetchUserDataCoroutine(string userId)
    {
        Task getUserDataTask = GetUserDataAsync(userId);
        yield return new WaitUntil(() => getUserDataTask.IsCompleted);

        if (getUserDataTask.Exception != null)
        {
            Debug.LogError("Error getting data: " + getUserDataTask.Exception);
        }
        else
        {
            //UserData userData = getUserDataTask.Result;
            //SaveLoadManager.Ins.LoadToFile(userData);
            //LevelManager.Ins.Init();
            
        }
    }

}
