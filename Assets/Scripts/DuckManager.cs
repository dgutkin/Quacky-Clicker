﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;


public class DuckManager : MonoBehaviour {

    public static DuckManager instance = null;

    [Header("Spawn")]
    public RectTransform spawnLine;

    [Header("Spawn Objects")]
    public GameObject[] spawnDuckObjects;
    public int spawnDuckObjectsIndex = 0;
    private GameObject spawnSmallDuck;
    public GameObject[] spawnBonusObjects;
    private GameObject spawnBonusDuck;
	public GameObject[] spawnBubbleObjects;
	private GameObject spawnBubble;

    public GameObject scoreEffect;
    public GameObject scoreBonus;
    public GameObject bonusParticles;

    [Header("Sounds")]
    public AudioClip buttonClickSound;
    public AudioClip gameBonusSound;
    public AudioClip[] gameClickSound;
    public AudioClip gameBonusClickSound;
	public AudioClip powerUpSound;
	public AudioClip gameOverSound;


    [Header("Visuals")]
    public Text gameQuacksText;
    public Text gameQpsText;
    public Text gameMaxBonusText;
    public Text gameTimeText;
    public Text gameTimeCompletedText;

    public Slider bonusBar;

	public Text multiplierText;
	public GameObject rotatingBackground;

    [Header("Cursor")]
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    [Header("Menus")]
    public GameObject gameCanvas;
	public GameObject itemsCanvas;
    public GameObject gameOverCanvas;
    public GameObject gameInfoCanvas;

	[Header("Click")]
	public int clickValue = 1;
	public ulong baseQps = 0;
	public ulong quackScore = 0;

	public Number number = new Number();

	private ulong currentBonus;
	private ulong levelBonus = 10;
	private int bonusDuckMultiplier = 5;

	private int clickMultiplier = 1;
	private ulong levelQps = 0;
	private ulong levelQpsShown = 0;
	private int MultiplierTime = 10;

    private float gameTimer = 0.0f;
	private ulong gameSeconds = 0;
	private int multiplierTimer = 0;
	private bool multiplierOn = false;
	
	private ulong maxQuacks = 1000000000000;
     
    private Animator animator;

    private int itemAddCounter;

    internal bool isGameOver = false;
    internal bool isGamePaused = false;

    internal Vector3 aimMousePosition;
    internal Vector3 aimTouchPosition;

    string gameID = "3608416"; //iOS

	#region --------------- GAME MECHANICS ---------------

    void Awake()
    {
         if (instance == null)
             instance = this;
         else if (instance != this)
             Destroy(gameObject);  

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

		#if UNITY_WEBGL
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
		#else
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
		#endif

        LoadGameData();
        SetGameData();
        UpdateGameData();
        ShowGamePlayMenu();

        Advertisement.Initialize(gameID, true);

    }

    void Update()
    {
        gameTimer += Time.deltaTime;

		if (gameCanvas.activeSelf)
		{

        UpdateInput();

        if (gameTimer >= 1)
        { 
			
			if (multiplierOn && multiplierTimer == 0) {
				
				multiplierTimer++;
				clickMultiplier = 3;
				multiplierText.text = "3x";
				rotatingBackground.GetComponent<SpriteRenderer> ().color = Color.red;
				SoundManager.instance.PlaySoundPreserve(powerUpSound);

			} else if (multiplierOn && multiplierTimer <= MultiplierTime) {
				
				multiplierTimer++;

			} else if (multiplierOn && multiplierTimer > MultiplierTime) {
				
				clickMultiplier = 1;
				multiplierTimer = 0;
				multiplierOn = false;
				multiplierText.text = "1x";
				rotatingBackground.GetComponent<SpriteRenderer> ().color = new Color(212,61,201);

			} else if (!multiplierOn && levelBonus > 100) {
				
				int rand = Random.Range(0, 60);
				if (rand == 1)
				{
					multiplierOn = true;
				} else if (rand == 2 || rand == 3) {
					
					StartCoroutine(CreateBonusDuck(1, 1));

				}

			}

			int bubbleCount = (int)Mathf.Log10 (baseQps);
			for (int i = 0; i < bubbleCount; i++) {
				CreateBubble ();
			}

			quackScore = quackScore + baseQps;
			levelQpsShown = levelQps + baseQps;
			levelQps = 0;
            gameTimer = 0;
            UpdateGameData();
            UpdateBonusData();
			
			if (gameSeconds < 1000000000000) {

				gameSeconds++;

			}

			if (quackScore >= maxQuacks) {

				quackScore = maxQuacks;
				GameOver ();

			}

        }
    	}
	}

    public void UpdateInput()
    {
        if (!Application.isMobilePlatform)
        {

            if (Input.GetButtonDown("Fire1"))
            {
                aimMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if (!gameOverCanvas.activeSelf) 
				{
					Tap (aimMousePosition);
				}
            }
        }

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                for (int t = 0; t < Input.touchCount; t++)
                {
                    Touch touch = Input.GetTouch(t);
                    TouchPhase phase = touch.phase;

                    if (phase == TouchPhase.Began)
                    {
                        aimTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        Tap(aimTouchPosition);
                    }
                }
            }
        }
    }

    public void Tap(Vector3 aimPosition)
    {   
        RaycastHit2D raycastHit2D = Physics2D.Raycast(aimPosition, Vector2.zero);
        if (raycastHit2D.collider != null)
        {
        	levelQps = levelQps + (ulong) (clickValue * clickMultiplier);

            if (raycastHit2D.collider.tag == "Bonus")
            {
                SoundManager.instance.PlaySoundPreserve(gameBonusClickSound);
                UpdateScore(clickValue * clickMultiplier * bonusDuckMultiplier);
                Vector3 spawnBonusPos = raycastHit2D.collider.gameObject.transform.position;
                GameObject newBonusParticle = (GameObject)(Instantiate(bonusParticles, spawnBonusPos, Quaternion.identity));
                Destroy(newBonusParticle, 1.0f);
                Destroy(raycastHit2D.collider.gameObject);
                GameObject newBonusText = (GameObject)(Instantiate(scoreBonus, spawnBonusPos, Quaternion.identity));
                newBonusText.GetComponent<ScoreBonus>().SetScoreValue(clickValue * clickMultiplier * bonusDuckMultiplier);
            }
            else if (raycastHit2D.collider.tag == "Cookie")
            {
                PlaySound(gameClickSound[Random.Range(0, gameClickSound.Length)]);
                CreateSmallDuck();
                animator.SetBool("Tap", true);
                UpdateScore(clickValue * clickMultiplier);
                CreateTextScore(clickValue * clickMultiplier);
            }
        }
    }

    public void UpdateScore(int scoreValue)
    {
		quackScore += (ulong) scoreValue;
        UpdateGameData();
        UpdateBonusData();
        SaveGameData();
    }

    void UpdateBonusData()
    {
		currentBonus = quackScore;
       
        if (currentBonus >= levelBonus && currentBonus < maxQuacks)
        {
            SoundManager.instance.PlaySoundPreserve(gameBonusSound);
            
            StartCoroutine(CreateBonusDuck(2, 6));
            
            SetBonusLevel();

            SetMaxBonusText();
            bonusBar.minValue = 0;
			bonusBar.maxValue = (float) levelBonus; 
        }
		bonusBar.value = (float) currentBonus;
    }

    void SetBonusLevel()
    {
		//best bonus ducks, add buffer and 30s of bonus clicks
		ulong levelBonusNew = levelBonus + (ulong) ((3 + (6 * 3)) * bonusDuckMultiplier * clickValue) + 30 * (baseQps + (ulong) clickValue);
		levelBonusNew = number.RoundLargeNumber(levelBonusNew);
		if (levelBonusNew == levelBonus)
			levelBonusNew = number.IncrementLargeNumber(levelBonus, 1);
		if (levelBonusNew > maxQuacks)
			levelBonusNew = maxQuacks;
			
		levelBonus = levelBonusNew;
    }

	public void SetMaxBonusText() 
	{
		gameMaxBonusText.text = number.FormatLargeNumber(levelBonus);
	}

    public void CreateTextScore(int scoreValue)
    {
        float spawnObjectXPos = Random.Range(-1.0f, 1.0f);
        float spawnObjectYPos = Random.Range(-1.0f, 1.0f);
        Vector3 spawnObjectPos = new Vector3(spawnObjectXPos,spawnObjectYPos, 0);
        GameObject newScoreText = (GameObject) (Instantiate(scoreEffect, spawnObjectPos, Quaternion.identity));
        newScoreText.GetComponent<ScoreEffect>().SetScoreValue(clickValue * clickMultiplier);
    }

    public void CreateSmallDuck()
    {
        float spawnObjectXPos = Random.Range(-2.0f, 2.0f);
        Vector3 spawnObjectPos = new Vector3(spawnObjectXPos, spawnLine.position.y, 0);
        spawnSmallDuck = spawnDuckObjects[spawnDuckObjectsIndex];
        GameObject newSmallDuck = (GameObject)(Instantiate(spawnSmallDuck, spawnObjectPos, Quaternion.identity));
		Destroy (newSmallDuck, 3.0f);
    }

    public IEnumerator CreateBonusDuck(int minDucks, int maxDucks)
    {
		for (int i = 0; i < Random.Range (minDucks, maxDucks); i++) {
			float spawnObjectXPos = Random.Range (-1.5f, 1.5f);
			Vector3 spawnObjectPos = new Vector3 (spawnObjectXPos, spawnLine.position.y, 0);
			spawnBonusDuck = spawnBonusObjects [Random.Range (0, spawnBonusObjects.Length)];
			GameObject newBonusDuck = (GameObject)(Instantiate (spawnBonusDuck, spawnObjectPos, Quaternion.identity));
			Destroy (newBonusDuck, 5.0f);
			yield return new WaitForSeconds(1.0f);
		}
    }

	public void CreateBubble()
	{
		float spawnObjectXPos = Random.Range(-2.0f, 2.0f);
		Vector3 spawnObjectPos = new Vector3(spawnObjectXPos, spawnLine.position.y, 0);
		spawnBubble = spawnBubbleObjects[Random.Range(0, spawnBubbleObjects.Length)];
		GameObject newBubble = (GameObject)(Instantiate(spawnBubble, spawnObjectPos, Quaternion.identity));
		Destroy (newBubble, 3.0f);	
	}

	public void PlaySound(AudioClip clip)
	{
		if (clip != null)
			SoundManager.instance.PlaySound(clip);
	}

	public void ButtonSound()
	{
		if (buttonClickSound != null)
			SoundManager.instance.PlaySound(buttonClickSound);
	}

	public void TapFalse()
	{
		animator.SetBool("Tap", false);
	}

	public string DisplayGameTime()
	{
		ulong gameHours = 0;
		ulong gameMinutes = 0;
		ulong gameSecondsRemainder = 0;
		ulong secondsInHour = 60 * 60;
		ulong secondsInMinute = 60;

		if (gameSeconds > (secondsInHour))
		{
			gameHours = gameSeconds / (secondsInHour);
		}

		gameMinutes = (gameSeconds - (gameHours * secondsInHour)) / secondsInMinute;

		gameSecondsRemainder = gameSeconds - (gameHours * secondsInHour) - (gameMinutes * secondsInMinute);
		
		return gameHours.ToString() + "h " + gameMinutes.ToString() + "m " + gameSecondsRemainder.ToString() + "s";

	}

	#endregion

	#region --------------- GAME DATA ---------------

    void LoadGameData()
    {       
			//PlayerPrefs.DeleteAll(); // DELETE ALL GAME DATA !!!!!
			quackScore = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "QUACK_SCORE", "0"));
			levelBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "LEVEL_BONUS", "10"));
			currentBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "CURRENT_BONUS", "0"));
			clickValue = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "CLICK_VALUE", 1);
			baseQps = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "BASE_QPS", "0"));
			gameSeconds = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "GAME_SECONDS" ,"0"));

			GameObject itemManager = GameObject.Find("ItemManager");
			ItemManager itemManagerScript = itemManager.GetComponent<ItemManager>();
			itemManagerScript.LoadItemData();
    }

    void SetGameData()
    {
        bonusBar.minValue = 0;
		bonusBar.maxValue = (float) levelBonus;
		SetMaxBonusText ();
		bonusBar.value = (float) currentBonus;
		
		GameObject itemManager = GameObject.Find("ItemManager");
		ItemManager itemManagerScript = itemManager.GetComponent<ItemManager>();
		itemManagerScript.SetItemData();

    }

    public void UpdateGameData()
    {
        if (!isGameOver)
        {
			gameQuacksText.text = quackScore.ToString("N0") + " Quacks!";
            gameQpsText.text = levelQpsShown.ToString("N1") + " per second";
            bonusBar.value = quackScore;
        }
    }

    public void SaveGameData()
    {  
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "QUACK_SCORE", quackScore.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "LEVEL_BONUS", levelBonus.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "CURRENT_BONUS", currentBonus.ToString());
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "CLICK_VALUE", clickValue);
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "BASE_QPS", baseQps.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "GAME_SECONDS", gameSeconds.ToString());

			GameObject itemManager = GameObject.Find("ItemManager");
			ItemManager itemManagerScript = itemManager.GetComponent<ItemManager>();
			itemManagerScript.SaveItemData();
    }

	#endregion

    #region --------------- MENUS AND GAME CONTROL ---------------

    public void ShowGamePlayMenu()
    {
        gameCanvas.SetActive(true);
		itemsCanvas.SetActive (false);
        gameOverCanvas.SetActive(false);
        gameInfoCanvas.SetActive(false);
    }

    public void ShowItemsMenu()
    {
		itemsCanvas.SetActive (true);
		gameOverCanvas.SetActive(false);
		gameCanvas.SetActive(false);
		gameInfoCanvas.SetActive(false);
    }

	public void ShowGameOverMenu()
	{
		gameOverCanvas.SetActive (true);
		gameCanvas.SetActive (false);
		itemsCanvas.SetActive (false);
		gameInfoCanvas.SetActive(false);
	}

	public void ShowGameInfoMenu()
	{
		gameInfoCanvas.SetActive(true);
		gameOverCanvas.SetActive (false);
		gameCanvas.SetActive (false);
		itemsCanvas.SetActive (false);
	}

	public void GameItems()
	{
		ButtonSound ();
		GameObject itemManager = GameObject.Find("ItemManager");
		ItemManager itemManagerScript = itemManager.GetComponent<ItemManager>();
		itemManagerScript.UpdateQuacksText(quackScore);

		GameObject leaderboardManager = GameObject.Find("LeaderboardManager");
		LeaderboardManager leaderboardManagerScript = leaderboardManager.GetComponent<LeaderboardManager>();
		leaderboardManagerScript.ReportScore((long) quackScore, "HighQuacks");

		if (itemAddCounter == 4) {
			Advertisement.Show();
			itemAddCounter = 0;
		} else {
			itemAddCounter++;
		}
		ShowItemsMenu ();
	}

    public void GameReturn()
    {
        ButtonSound();
		ShowGamePlayMenu();
    }

    public void GameInfo()
    {
    	ButtonSound();
    	gameTimeText.text = "Time played : " + DisplayGameTime();
    	ShowGameInfoMenu();
    }

	public void GameOver()
	{
		PlaySound(gameOverSound);
		gameTimeCompletedText.text = " Completed in : " + DisplayGameTime();

		GameObject leaderboardManager = GameObject.Find("LeaderboardManager");
		LeaderboardManager leaderboardManagerScript = leaderboardManager.GetComponent<LeaderboardManager>();
		leaderboardManagerScript.ReportScore((long) quackScore, "HighQuacks");
		leaderboardManagerScript.ReportScore((long) gameSeconds, "BestTimes");

		ShowGameOverMenu();
	}

	public void GameRestart()
	{

		ButtonSound ();

		if (quackScore < maxQuacks)
		{
			GameObject leaderboardManager = GameObject.Find("LeaderboardManager");
			LeaderboardManager leaderboardManagerScript = leaderboardManager.GetComponent<LeaderboardManager>();
			leaderboardManagerScript.ReportScore((long) quackScore, "HighQuacks");
		}

		PlayerPrefs.DeleteAll ();
		spawnDuckObjectsIndex = 0;
		LoadGameData ();
		SetGameData ();
		UpdateGameData ();
		ShowGamePlayMenu ();

	}

	#endregion
}
	