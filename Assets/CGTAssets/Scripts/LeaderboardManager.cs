using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LeaderboardManager : MonoBehaviour
{

	ILeaderboard highQuacksLeaderboard;
	ILeaderboard bestTimesLeaderboard;

    // Start is called before the first frame update
    void Start()
    {
        if (!Social.localUser.authenticated)
            Social.localUser.Authenticate (success => {
                //do something on success if you like
            });

		highQuacksLeaderboard = Social.CreateLeaderboard();
        highQuacksLeaderboard.id = "HighQuacks";
        highQuacksLeaderboard.LoadScores(result =>
        {
            Debug.Log("Received " + highQuacksLeaderboard.scores.Length + " scores");
            foreach (IScore score in highQuacksLeaderboard.scores)
                Debug.Log(score);
        });

        bestTimesLeaderboard = Social.CreateLeaderboard();
        bestTimesLeaderboard.id = "BestTimes";
        bestTimesLeaderboard.LoadScores(result =>
        {
            Debug.Log("Received " + bestTimesLeaderboard.scores.Length + " scores");
            foreach (IScore score in bestTimesLeaderboard.scores)
                Debug.Log(score);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ShowLeaderboard ()
    {
        if (Social.localUser.authenticated) {
            Social.ShowLeaderboardUI ();
            
            GameObject duckManager = GameObject.Find("DuckManager");
			CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
			ReportScore((long) duckManagerScript.quackScore, highQuacksLeaderboard.id);

        }
    }

    public void ReportScore(long score, string leaderboardID)
    {
        Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
        Social.ReportScore(score, leaderboardID, success => {
            Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
    }

}
