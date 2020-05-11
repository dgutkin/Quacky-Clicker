using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LeaderboardManager : MonoBehaviour
{

	ILeaderboard leaderboard;

    // Start is called before the first frame update
    void Start()
    {
        if (!Social.localUser.authenticated)
            Social.localUser.Authenticate (success => {
                //do something on success if you like
            });

		leaderboard = Social.CreateLeaderboard();
        leaderboard.id = "HighQuacks";
        leaderboard.LoadScores(result =>
        {
            Debug.Log("Received " + leaderboard.scores.Length + " scores");
            foreach (IScore score in leaderboard.scores)
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
            // get the quackScore from the CGTCookieManager and report to the leaderboard
            GameObject duckManager = GameObject.Find("DuckManager");
			CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
			ReportScore((long) duckManagerScript.quackScore, leaderboard.id);
        }
    }

    void ReportScore(long score, string leaderboardID)
    {
        Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
        Social.ReportScore(score, leaderboardID, success => {
            Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
    }

}
