using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class CGTCookieManager : MonoBehaviour {

    public static CGTCookieManager instance = null;

    [Header("Spawn")]
    public RectTransform spawnLine;

    [Header("Spawn Objects")]
    public GameObject[] spawnDuckObjects;
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
    public AudioClip[] gameBonusClickSound;
	public AudioClip powerUpSound;


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
	private ulong levelBonus = 1;
	private ulong levelBonusMulti = 1;

	private int clickMultiplier = 1;
    private int levelBonusQuacks = 1;
    private int levelBonusSub = 5;
	private ulong levelQps = 0;
	private ulong levelQpsShown = 0;
	private int MultiplierTime = 10;

    private float gameTimer = 0.0f;
	private ulong gameSeconds = 0;
	private int multiplierTimer = 0;
	private bool multiplierOn = false;

	private ulong maxQuacks = (ulong) Mathf.Pow (10, 13) - 1;
     
    private Animator animator;

    internal bool isGameOver = false;
    internal bool isGamePaused = false;

    internal Vector3 aimMousePosition;
    internal Vector3 aimTouchPosition;

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
				GetComponent<AudioSource>().PlayOneShot(powerUpSound);

			} else if (multiplierOn && multiplierTimer <= MultiplierTime) {
				
				multiplierTimer++;

			} else if (multiplierOn && multiplierTimer > MultiplierTime) {
				
				clickMultiplier = 1;
				multiplierTimer = 0;
				multiplierOn = false;
				multiplierText.text = "1x";
				rotatingBackground.GetComponent<SpriteRenderer> ().color = new Color(212,61,201);

			} else if (!multiplierOn && levelBonus > 100) {
				
				multiplierOn = Random.Range (0, 60) == 1;

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
			
			if (gameSeconds < (ulong)Mathf.Pow (10, 13) - 1) {

				gameSeconds++;

			}

			if (quackScore >= maxQuacks) {

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
                PlaySound(gameBonusClickSound[Random.Range(0, gameBonusClickSound.Length)]);
                UpdateScore(clickValue * clickMultiplier * levelBonusSub);
                Vector3 spawnBonusPos = raycastHit2D.collider.gameObject.transform.position;
                GameObject newBonusParticle = (GameObject)(Instantiate(bonusParticles, spawnBonusPos, Quaternion.identity));
                Destroy(newBonusParticle, 1.0f);
                Destroy(raycastHit2D.collider.gameObject);
                GameObject newBonusText = (GameObject)(Instantiate(scoreBonus, spawnBonusPos, Quaternion.identity));
                newBonusText.GetComponent<CGTScoreBonus>().SetScoreValue(clickValue * clickMultiplier * levelBonusSub);
            }
            else if (raycastHit2D.collider.tag == "Cookie")
            {
                PlaySound(gameClickSound[Random.Range(0, gameClickSound.Length)]);
                CreateSmallDuck();
                animator.SetBool("Tap", true);
                UpdateScore(clickValue * clickMultiplier);
            }
        }
    }

    public void UpdateScore(int scoreValue)
    {
		quackScore += (ulong) scoreValue;
        UpdateGameData();
        UpdateBonusData(scoreValue);
        SaveGameData();
        CreateTextScore(scoreValue);
    }

    void UpdateBonusData(int scoreValue)
    {
		currentBonus += (ulong) scoreValue;
       
        if (currentBonus >= levelBonus)
        {
            PlaySound(gameBonusSound);
            for (int i = 0; i < levelBonusQuacks; i++)
            {
                CreateBonusDuck();
            }
            currentBonus = currentBonus - levelBonus;
            SetBonusLevel();
            bonusBar.minValue = 0;
			bonusBar.maxValue = (float) levelBonus;  
        }
		bonusBar.value = (float) currentBonus;
    }

    void SetBonusLevel()
    {
		if ((levelBonus / levelBonusMulti) > 100)
        {
            levelBonusMulti *= 10;
        }
		levelBonusSub = (int) Mathf.Ceil(5 * levelBonusMulti); //bonus duck multiplier
		levelBonus += (ulong) (10 * levelBonusSub * clickValue * clickMultiplier) + (30 * baseQps); //10 bonus ducks + 30s of base QPS
		SetMaxBonusText();
    }

	public void SetMaxBonusText() 
	{
		gameMaxBonusText.text = number.FormatLargeNumber(number.RoundLargeNumber(levelBonus));
	}

    public void CreateTextScore(int scoreValue)
    {
        float spawnObjectXPos = Random.Range(-1.0f, 1.0f);
        float spawnObjectYPos = Random.Range(-1.0f, 1.0f);
        Vector3 spawnObjectPos = new Vector3(spawnObjectXPos,spawnObjectYPos, 0);
        GameObject newScoreText = (GameObject) (Instantiate(scoreEffect, spawnObjectPos, Quaternion.identity));
        newScoreText.GetComponent<CGTScoreEffect>().SetScoreValue(clickValue * clickMultiplier);
    }

    public void CreateSmallDuck()
    {
        float spawnObjectXPos = Random.Range(-4.0f, 4.0f);
        Vector3 spawnObjectPos = new Vector3(spawnObjectXPos, spawnLine.position.y, 0);
        spawnSmallDuck = spawnDuckObjects[Random.Range(0, spawnDuckObjects.Length)];
        GameObject newSmallDuck = (GameObject)(Instantiate(spawnSmallDuck, spawnObjectPos, Quaternion.identity));
		Destroy (newSmallDuck, 3.0f);
    }

    public void CreateBonusDuck()
    {
		for (int i = 0; i < Random.Range (1, 6); i++) {
			float spawnObjectXPos = Random.Range (-3.0f, 3.0f);
			Vector3 spawnObjectPos = new Vector3 (spawnObjectXPos, spawnLine.position.y, 0);
			spawnBonusDuck = spawnBonusObjects [Random.Range (0, spawnBonusObjects.Length)];
			GameObject newBonusDuck = (GameObject)(Instantiate (spawnBonusDuck, spawnObjectPos, Quaternion.identity));
			Destroy (newBonusDuck, 5.0f);
		}
    }

	public void CreateBubble()
	{
		float spawnObjectXPos = Random.Range(-4.0f, 4.0f);
		Vector3 spawnObjectPos = new Vector3(spawnObjectXPos, spawnLine.position.y, 0);
		spawnBubble = spawnBubbleObjects[Random.Range(0, spawnBubbleObjects.Length)];
		GameObject newBubble = (GameObject)(Instantiate(spawnBubble, spawnObjectPos, Quaternion.identity));
		Destroy (newBubble, 3.0f);	
	}

	public void PlaySound(AudioClip clip)
	{
		if (clip != null)
			CGTSoundManager.instance.PlaySound(clip);
	}

	public void ButtonSound()
	{
		if (buttonClickSound != null)
			CGTSoundManager.instance.PlaySound(buttonClickSound);
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
		#if UNITY_5_3_OR_NEWER
	        
			PlayerPrefs.DeleteAll(); // DELETE ALL GAME DATA !!!!!
			quackScore = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "QUACK_SCORE", "0"));
			levelBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "LEVEL_BONUS", "10"));
			levelBonusMulti = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "LEVEL_BONUSMULTI", "1"));
			currentBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "CURRENT_BONUS", "0"));
			clickValue = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "CLICK_VALUE", 1);
			baseQps = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "BASE_QPS", "0"));
			gameSeconds = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "GAME_SECONDS" ,"0"));

			GameObject itemManager = GameObject.Find("ItemManager");
			ItemManager itemManagerScript = itemManager.GetComponent<ItemManager>();
			itemManagerScript.LoadItemData();
	       
		#else
			// DELETE ALL GAME DATA !!!!! PlayerPrefs.DeleteAll();
			quackScore = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "QUACK_SCORE", "0"));
			levelBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "LEVEL_BONUS", "10"));
			levelBonusMulti = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "LEVEL_BONUSMULTI", "1"));
			currentBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "CURRENT_BONUS", "0"));
			clickValue = PlayerPrefs.GetInt(Application.loadedLevelName + "CLICK_VALUE", 1);
			baseQps = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "BASE_QPS", "0"));
	
		#endif
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
        }
        else
        {
           /* gameOverScoreText.text = "Score: " + lastGameScore.ToString();
            gameOverHighScoreText.text = "High Score: " + highGameScore.ToString(); */
        }
    }

    public void SaveGameData()
    {
		#if UNITY_5_3_OR_NEWER    
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "QUACK_SCORE", quackScore.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "LEVEL_BONUS", levelBonus.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "LEVEL_BONUSMULTI", levelBonusMulti.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "CURRENT_BONUS", currentBonus.ToString());
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "CLICK_VALUE", clickValue);
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "BASE_QPS", baseQps.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "GAME_SECONDS", gameSeconds.ToString());

			GameObject itemManager = GameObject.Find("ItemManager");
			ItemManager itemManagerScript = itemManager.GetComponent<ItemManager>();
			itemManagerScript.SaveItemData();

		#else
			PlayerPrefs.SetString(Application.loadedLevelName + "QUACK_SCORE", quackScore.ToString());
			PlayerPrefs.SetString(Application.loadedLevelName + "LEVEL_BONUS", levelBonus.ToString());
			PlayerPrefs.SetString(Application.loadedLevelName + "LEVEL_BONUSMULTI", levelBonusMulti.ToString());
			PlayerPrefs.SetString(Application.loadedLevelName + "CURRENT_BONUS", currentBonus.ToString());
			PlayerPrefs.SetInt(Application.loadedLevelName + "CLICK_VALUE", clickValue);
			PlayerPrefs.SetString(Application.loadedLevelName + "BASE_QPS", baseQps.ToString());
			
		#endif
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
		ButtonSound();
		gameTimeCompletedText.text = " Completed in : " + DisplayGameTime();
		ShowGameOverMenu();
	}

	public void GameRestart()
	{

		ButtonSound ();
		PlayerPrefs.DeleteAll ();
		LoadGameData ();
		SetGameData ();
		UpdateGameData ();
		ShowGamePlayMenu ();

	}

	#endregion
}
	