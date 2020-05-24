using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LeaderboardManager : MonoBehaviour
{

	ILeaderboard highQuacksLeaderboard;
	ILeaderboard bestTimesLeaderboard;

    void Start()
    {
        if (!Social.localUser.authenticated)
            Social.localUser.Authenticate (success => {
                //do something on success
            });

		highQuacksLeaderboard = Social.CreateLeaderboard();
        highQuacksLeaderboard.id = "HighQuacks";
        highQuacksLeaderboard.LoadScores(result =>
        {
            // Debug.Log("Received " + highQuacksLeaderboard.scores.Length + " scores");
            // foreach (IScore score in highQuacksLeaderboard.scores)
            //     Debug.Log(score);
        });

        bestTimesLeaderboard = Social.CreateLeaderboard();
        bestTimesLeaderboard.id = "BestTimes";
        bestTimesLeaderboard.LoadScores(result =>
        {
            // log scores
        });
    }

    void Update()
    {
        
    }

	public void ShowLeaderboard ()
    {
        GameObject duckManager = GameObject.Find("DuckManager");
        DuckManager duckManagerScript = duckManager.GetComponent<DuckManager>();
        duckManagerScript.ButtonSound();

        if (Social.localUser.authenticated) {
            
            Social.ShowLeaderboardUI ();
            
			ReportScore((long) duckManagerScript.quackScore, highQuacksLeaderboard.id);

        }
    }

    public void ReportScore(long score, string leaderboardID)
    {
        //Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
        Social.ReportScore(score, leaderboardID, success => {
            //Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
    }

}
