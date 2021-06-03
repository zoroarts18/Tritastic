using UnityEngine;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class PlayGames : MonoBehaviour
{
    string leaderboardID = "CgkIzIr9w8EaEAIQAA";

    static bool active = false;

    void Start()
    {

        if (!active)
        {
            DontDestroyOnLoad(this);
            active = true;
        }
        else
        {
            Destroy(this);
        }


        try
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) => { });
        }
        catch (Exception exception)
        {
            Debug.Log(exception);
        }
    }

    public void AddScoreToLeaderboard(int score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, leaderboardID, success => { });
        }
    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }
}