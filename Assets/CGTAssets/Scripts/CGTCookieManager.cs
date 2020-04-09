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

	[Header("Click")]
	public int clickValue = 1;
	public ulong baseQps = 0;

	private ulong quackScore = 0;
	private ulong currentBonus;
	private ulong levelBonus = 1;
	private ulong levelBonusMulti = 1;

	private int clickMultiplier = 1;
    private int levelBonusQuacks = 1;
    private int levelBonusSub = 5;
	private ulong levelQps = 0;
	private ulong levelQpsShown = 0;

	[Header("Items")]
	public Item powerClick = new Item(0, new int[30], 1, "power click");
	public Item toothbrush = new Item(0, new int[100], 1, "toothbrush");
	public Item toothpaste = new Item (0, new int[100], 2, "toothpaste");
	public Item soap = new Item(0, new int[100], 5, "soap");
	public Item sponge = new Item (0, new int[100], 10, "sponge");
	public Item deodorant = new Item (0, new int[100], 20, "deodorant");
	public Item toiletPaper = new Item (0, new int[100], 50, "toilet paper");
	public Item hairbrush = new Item (0, new int[100], 100, "hair brush");
	public Item toiletBrush = new Item (0, new int[100], 500, "toilet brush");
	public Item plunger = new Item (0, new int[100], 1000, "plunger");
	public Item scale = new Item (0, new int[100], 5000, "scale");
	public Item ovalMirror = new Item (0, new int[100], 10000, "oval mirror");
	public Item towel = new Item (0, new int[100], 50000, "towel");
	public Item towelRack = new Item (0, new int[100], 100000, "towel rack");
	public Item hairDryer = new Item (0, new int[100], 500000, "hair dryer");
	public Item sink = new Item (0, new int[100], 10000000, "sink");
	public Item toilet = new Item (0, new int[100], 5000000, "toilet");
	public Item shower = new Item (0, new int[100], 10000000, "shower");
	public Item bathtub = new Item (0, new int[100], 50000000, "bathtub");
	public Item washingMachine = new Item (0, new int[100], 1000000000, "washing machine"); //Actual Qps: 100000000

	[Header("Item Level Texts")]
	public Text powerClickLevelText;
	public Text toothbrushLevelText;
	public Text toothpasteLevelText;
	public Text soapLevelText;
	public Text spongeLevelText;
	public Text deodorantLevelText;
	public Text toiletPaperLevelText;
	public Text hairbrushLevelText;
	public Text toiletBrushLevelText;
	public Text plungerLevelText;
	public Text scaleLevelText;
	public Text ovalMirrorLevelText;
	public Text towelLevelText;
	public Text towelRackLevelText;
	public Text hairDryerLevelText;
	public Text sinkLevelText;
	public Text toiletLevelText;
	public Text showerLevelText;
	public Text bathtubLevelText;
	public Text washingMachineLevelText;
	// could have a separate class called ItemManager
	// the class initializes all the level texts and contains all the upgrade methods
	// this class invokes the ItemManager to make updates to the Items however handling quacks and Qps updates here

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

			} else if (multiplierOn && multiplierTimer <= 5) {
				
				multiplierTimer++;

			} else if (multiplierOn && multiplierTimer > 5) {
				
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
		levelQps = levelQps + (ulong) (clickValue * clickMultiplier);

        
        RaycastHit2D raycastHit2D = Physics2D.Raycast(aimPosition, Vector2.zero);
        if (raycastHit2D.collider != null)
        {
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

	public void SetMaxBonusText() {

		int levelBonusSize = (int) Mathf.Floor(Mathf.Log10(levelBonus));
		if (levelBonusSize < 3) {
			gameMaxBonusText.text = levelBonus.ToString ("F1");
		} else if (levelBonusSize < 6) {
			gameMaxBonusText.text = (levelBonus / 1000f).ToString ("F1") + "k";
		} else if (levelBonusSize < 9) {
			gameMaxBonusText.text = (levelBonus / 1000000f).ToString ("F1") + "mn";
		} else if (levelBonusSize < 12) {
			gameMaxBonusText.text = (levelBonus / 1000000000f).ToString ("F1") + "bn";
		} else if (levelBonusSize < 15) {
			gameMaxBonusText.text = (levelBonus / 1000000000000f).ToString ("F1") + "tn";
		}

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

	void ButtonSound()
	{
		if (buttonClickSound != null)
			CGTSoundManager.instance.PlaySound(buttonClickSound); 
	}

	public void TapFalse()
	{
		animator.SetBool("Tap", false);
	}

	#endregion

	#region --------------- GAME DATA ---------------

    void LoadGameData()
    {
		#if UNITY_5_3_OR_NEWER
	        
			// also include in prefs baseQps
			PlayerPrefs.DeleteAll(); // DELETE ALL GAME DATA !!!!!
			quackScore = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "QUACK_SCORE", "0"));
			levelBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "LEVEL_BONUS", "10"));
			levelBonusMulti = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "LEVEL_BONUSMULTI", "1"));
			currentBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "CURRENT_BONUS", "0"));
			clickValue = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "CLICK_VALUE", 1);
			baseQps = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "BASE_QPS", "0"));
			powerClick.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "POWERCLICK_LEVEL", 0);
			toothbrush.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "TOOTHBRUSH_LEVEL", 0);
			toothpaste.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "TOOTHPASTE_LEVEL", 0);
			soap.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "SOAP_LEVEL", 0);
			sponge.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "SPONGE_LEVEL", 0);
			deodorant.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "DEODORANT_LEVEL", 0);
			toiletPaper.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "TOILETPAPER_LEVEL", 0);
			hairbrush.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "HAIRBRUSH_LEVEL", 0);
			toiletBrush.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "TOILETBRUSH_LEVEL", 0);
			plunger.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "PLUNGER_LEVEL", 0);
			scale.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "SCALE_LEVEL", 0);
			ovalMirror.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "OVALMIRROR_LEVEL", 0);
			towel.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "TOWEL_LEVEL", 0);
			towelRack.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "TOWELRACK_LEVEL", 0);
			hairDryer.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "HAIRDRYER_LEVEL", 0);
			sink.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "SINK_LEVEL", 0);
			toilet.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "TOILET_LEVEL", 0);
			shower.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "SHOWER_LEVEL", 0);
			bathtub.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "BATHTUB_LEVEL", 0);
			washingMachine.Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "WASHINGMACHINE_LEVEL", 0);
	       
		#else
			// DELETE ALL GAME DATA !!!!! PlayerPrefs.DeleteAll();
			quackScore = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "QUACK_SCORE", "0"));
			levelBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "LEVEL_BONUS", "10"));
			levelBonusMulti = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "LEVEL_BONUSMULTI", "1"));
			currentBonus = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "CURRENT_BONUS", "0"));
			clickValue = PlayerPrefs.GetInt(Application.loadedLevelName + "CLICK_VALUE", 1);
			baseQps = System.Convert.ToUInt64(PlayerPrefs.GetString(Application.loadedLevelName + "BASE_QPS", "0"));
			powerClick.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "POWERCLICK_LEVEL", 0);	
			toothbrush.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "TOOTHBRUSH_LEVEL", 0);
			toothpaste.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "TOOTHPASTE_LEVEL", 0);
			soap.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "SOAP_LEVEL", 0);
			sponge.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "SPONGE_LEVEL", 0);
			deodorant.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "DEODORANT_LEVEL", 0);
			toiletPaper.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "TOILETPAPER_LEVEL", 0);
			hairbrush.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "HAIRBRUSH_LEVEL", 0);
			toiletBrush.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "TOILETBRUSH_LEVEL", 0);
			plunger.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "PLUNGER_LEVEL", 0);
			scale.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "SCALE_LEVEL", 0);
			ovalMirror.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "OVALMIRROR_LEVEL", 0);
			towel.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "TOWEL_LEVEL", 0);
			towelRack.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "TOWELRACK_LEVEL", 0);
			hairDryer.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "HAIRDRYER_LEVEL", 0);
			sink.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "SINK_LEVEL", 0);
			toilet.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "TOILET_LEVEL", 0);
			shower.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "SHOWER_LEVEL", 0);
			bathtub.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "BATHTUB_LEVEL", 0);
			washingMachine.Level = PlayerPrefs.GetInt(Application.loadedLevelName + "WASHINGMACHINE_LEVEL", 0);
	
		#endif
    }

    void SetGameData()
    {
        bonusBar.minValue = 0;
		bonusBar.maxValue = (float) levelBonus;
		SetMaxBonusText ();
		bonusBar.value = (float) currentBonus;

		powerClick.QuackPrices = powerClick.GenerateItemPrices (1, 1, 30); //actual base is 10
		toothbrush.QuackPrices = toothbrush.GenerateItemPrices (1, 10, 100); //actual base is 10
		toothpaste.QuackPrices = toothpaste.GenerateItemPrices (1, 9, 100); // actual base is 50
		soap.QuackPrices = soap.GenerateItemPrices(1, 8, 100);
		sponge.QuackPrices = sponge.GenerateItemPrices (1, 7, 100);
		deodorant.QuackPrices = deodorant.GenerateItemPrices (1, 6, 100);
		toiletPaper.QuackPrices = toiletPaper.GenerateItemPrices (1, 5, 100);
		hairbrush.QuackPrices = hairbrush.GenerateItemPrices (1, 4, 100);
		toiletBrush.QuackPrices = toiletBrush.GenerateItemPrices (1, 3, 100);
		plunger.QuackPrices = plunger.GenerateItemPrices (1, 2, 100);
		scale.QuackPrices = scale.GenerateItemPrices (1, 1, 100);
		ovalMirror.QuackPrices = ovalMirror.GenerateItemPrices (1, 1, 100);
		towel.QuackPrices = towel.GenerateItemPrices (1, 1, 100);
		towelRack.QuackPrices = towelRack.GenerateItemPrices (1, 1, 100);
		hairDryer.QuackPrices = hairDryer.GenerateItemPrices (1, 1, 100);
		sink.QuackPrices = sink.GenerateItemPrices (1, 1, 100);
		toilet.QuackPrices = toilet.GenerateItemPrices (1, 1, 100);
		shower.QuackPrices = shower.GenerateItemPrices (1, 1, 100);
		bathtub.QuackPrices = bathtub.GenerateItemPrices (1, 1, 100);
		washingMachine.QuackPrices = washingMachine.GenerateItemPrices (1, 1, 100);

		powerClickLevelText.text = UpdateItemLevel (powerClick.Level);
		toothbrushLevelText.text = UpdateItemLevel (toothbrush.Level);
		toothpasteLevelText.text = UpdateItemLevel (toothpaste.Level);
		soapLevelText.text = UpdateItemLevel (soap.Level);
		spongeLevelText.text = UpdateItemLevel (sponge.Level);
		deodorantLevelText.text = UpdateItemLevel (deodorant.Level);
		toiletPaperLevelText.text = UpdateItemLevel (toiletPaper.Level);
		hairbrushLevelText.text = UpdateItemLevel (hairbrush.Level);
		toiletBrushLevelText.text = UpdateItemLevel (toiletBrush.Level);
		plungerLevelText.text = UpdateItemLevel (plunger.Level);
		scaleLevelText.text = UpdateItemLevel (scale.Level);
		ovalMirrorLevelText.text = UpdateItemLevel (ovalMirror.Level);
		towelLevelText.text = UpdateItemLevel (towel.Level);
		towelRackLevelText.text = UpdateItemLevel (towelRack.Level);
		hairDryerLevelText.text = UpdateItemLevel (hairDryer.Level);
		sinkLevelText.text = UpdateItemLevel (sink.Level);
		toiletLevelText.text = UpdateItemLevel (toilet.Level);
		showerLevelText.text = UpdateItemLevel (shower.Level);
		bathtubLevelText.text = UpdateItemLevel (bathtub.Level);
		washingMachineLevelText.text = UpdateItemLevel (washingMachine.Level);

    }

    void UpdateGameData()
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

    void SaveGameData()
    {
		#if UNITY_5_3_OR_NEWER    
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "QUACK_SCORE", quackScore.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "LEVEL_BONUS", levelBonus.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "LEVEL_BONUSMULTI", levelBonusMulti.ToString());
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "CURRENT_BONUS", currentBonus.ToString());
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "CLICK_VALUE", clickValue);
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "BASE_QPS", baseQps.ToString());
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "POWERCLICK", powerClick.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "TOOTHBRUSH_LEVEL", toothbrush.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "TOOTHPASTE_LEVEL", toothpaste.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "SOAP_LEVEL", soap.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "SPONGE_LEVEL", sponge.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "DEODORANT_LEVEL", deodorant.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "TOILETPAPER_LEVEL", toiletPaper.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "HAIRBRUSH_LEVEL", hairbrush.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "TOILETBRUSH_LEVEL", toiletBrush.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "PLUNGER_LEVEL", plunger.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "SCALE_LEVEL", scale.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "OVALMIRROR_LEVEL", ovalMirror.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "TOWEL_LEVEL", towel.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "TOWELRACK_LEVEL", towelRack.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "HAIRDRYER_LEVEL", hairDryer.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "SINK_LEVEL", sink.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "TOILET_LEVEL", toilet.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "SHOWER_LEVEL", shower.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "BATHTUB_LEVEL", bathtub.Level);
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "WASHINGMACHINE_LEVEL", washingMachine.Level);
		#else
			PlayerPrefs.SetString(Application.loadedLevelName + "QUACK_SCORE", quackScore.ToString());
			PlayerPrefs.SetString(Application.loadedLevelName + "LEVEL_BONUS", levelBonus.ToString());
			PlayerPrefs.SetString(Application.loadedLevelName + "LEVEL_BONUSMULTI", levelBonusMulti.ToString());
			PlayerPrefs.SetString(Application.loadedLevelName + "CURRENT_BONUS", currentBonus.ToString());
			PlayerPrefs.SetInt(Application.loadedLevelName + "CLICK_VALUE", clickValue);
			PlayerPrefs.SetString(Application.loadedLevelName + "BASE_QPS", baseQps.ToString());
			PlayerPrefs.SetInt(Application.loadedLevelName + "POWERCLICK_LEVEL", powerClick.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "TOOTHBRUSH_LEVEL", toothbrush.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "TOOTHPASTE_LEVEL", toothpaste.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "SOAP_LEVEL", soap.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "SPONGE_LEVEL", sponge.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "DEODORANT_LEVEL", deodorant.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "TOILETPAPER_LEVEL", toiletPaper.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "HAIRBRUSH_LEVEL", hairbrush.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "TOILETBRUSH_LEVEL", toiletBrush.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "PLUNGER_LEVEL", plunger.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "SCALE_LEVEL", scale.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "OVALMIRROR_LEVEL", ovalMirror.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "TOWEL_LEVEL", towel.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "TOWELRACK_LEVEL", towelRack.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "HAIRDRYER_LEVEL", hairDryer.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "SINK_LEVEL", sink.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "TOILET_LEVEL", toilet.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "SHOWER_LEVEL", shower.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "BATHTUB_LEVEL", bathtub.Level);
			PlayerPrefs.SetInt(Application.loadedLevelName + "WASHINGMACHINE_LEVEL", washingMachine.Level);
		#endif
    }

	#endregion

    #region --------------- MENUS AND GAME CONTROL ---------------

    public void ShowGamePlayMenu()
    {
        gameCanvas.SetActive(true);
		itemsCanvas.SetActive (false);
        gameOverCanvas.SetActive(false);
    }

    public void ShowItemsMenu()
    {
		itemsCanvas.SetActive (true);
		gameOverCanvas.SetActive(false);
		gameCanvas.SetActive(false);
    }

	public void ShowGameOverMenu()
	{
		gameOverCanvas.SetActive (true);
		gameCanvas.SetActive (false);
		itemsCanvas.SetActive (false);
	}

	public void GameItems()
	{
		ButtonSound ();
		ShowItemsMenu ();
	}

    public void GameReturn()
    {
        ButtonSound();
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#elif UNITY_WEBPLAYER
//            Application.OpenURL(gameOverURL);
//#else
//            Application.Quit();
//#endif   
		// instead of quitting the game go back to it
		ShowGamePlayMenu();

    }

	public void GameOver()
	{
		ButtonSound();
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

	#region ----------------- ITEMS --------------------

	public void PowerUp()
	{

		if (BuyQualify (powerClick.Level, powerClick.QuackPrices)) {

			BuyItem (powerClick.QuackPrices, powerClick.Level);
			powerClick.Level++;
			clickValue *= 2; // double the click value

			Text powerClickLevelText = GameObject.Find ("PowerClickLevelText").GetComponent<Text> ();
			powerClickLevelText.text = UpdateItemLevel (powerClick.Level);
			Text powerClickPriceText = GameObject.Find ("PowerClickPriceText").GetComponent<Text> ();
			powerClickPriceText.text = "Cost: " + UpdateItemPrice (powerClick.Level, powerClick.QuackPrices);

			UpdateGameData ();
			SaveGameData ();
		}
			
	}

	public bool BuyQualify(int itemLevel, int[] itemQuackPrices){

		if (itemLevel < itemQuackPrices.Length && quackScore >= (ulong) itemQuackPrices [itemLevel]) {
			
			return true;

		} else {
			
			ButtonSound ();
			return false;

		}

	}

	public void BuyItem(int[] itemQuackPrices, int itemLevel){

		PlaySound (gameBonusClickSound [Random.Range (0, gameBonusClickSound.Length)]);
		quackScore = quackScore - (ulong) itemQuackPrices [itemLevel];

	}

	public void UpdateQps(int itemQps){

		baseQps = baseQps + (ulong) itemQps;

	}

	public string UpdateItemLevel(int itemLevel){

		return itemLevel.ToString();

	}

	public string UpdateItemPrice(int itemLevel, int[] itemQuackPrices){

		if (itemLevel == itemQuackPrices.Length) {

			return "Maxed";

		} else {

			return itemQuackPrices [itemLevel].ToString () + " quacks";

		}

	}

	public void ToothbrushUpgrade()
	{

		if (BuyQualify (toothbrush.Level, toothbrush.QuackPrices)) {

			BuyItem (toothbrush.QuackPrices, toothbrush.Level);
			toothbrush.Level++;
			UpdateQps(toothbrush.Qps);

			Text toothbrushLevelText = GameObject.Find ("ToothbrushLevelText").GetComponent<Text> ();
			toothbrushLevelText.text = UpdateItemLevel (toothbrush.Level);
			Text toothbrushPriceText = GameObject.Find ("ToothbrushPriceText").GetComponent<Text> ();
			toothbrushPriceText.text = UpdateItemPrice (toothbrush.Level, toothbrush.QuackPrices);

			UpdateGameData ();
			SaveGameData ();
		}
	}

	public void ToothpasteUpgrade()
	{
		if (BuyQualify (toothpaste.Level, toothpaste.QuackPrices)) {

			BuyItem (toothpaste.QuackPrices, toothpaste.Level);
			toothpaste.Level++;
			UpdateQps (toothpaste.Qps);

			Text toothpasteLevelText = GameObject.Find ("ToothpasteLevelText").GetComponent<Text> ();
			toothpasteLevelText.text = UpdateItemLevel (toothpaste.Level);
			Text toothpastePriceText = GameObject.Find ("ToothpastePriceText").GetComponent<Text> ();
			toothpastePriceText.text = UpdateItemPrice (toothpaste.Level, toothpaste.QuackPrices);

			UpdateGameData();
			SaveGameData ();

		}
	}

	public void SoapUpgrade()
	{
		if (BuyQualify (soap.Level, soap.QuackPrices)) {

			BuyItem (soap.QuackPrices, soap.Level);
			soap.Level++;
			UpdateQps (soap.Qps);

			Text soapLevelText = GameObject.Find ("SoapLevelText").GetComponent<Text> ();
			soapLevelText.text = UpdateItemLevel (soap.Level);
			Text soapPriceText = GameObject.Find ("SoapPriceText").GetComponent<Text> ();
			soapPriceText.text = UpdateItemPrice (soap.Level, soap.QuackPrices);

			UpdateGameData();
			SaveGameData ();

		}
	}

	public void SpongeUpgrade()
	{
		if (BuyQualify (sponge.Level, sponge.QuackPrices)) {

			BuyItem (sponge.QuackPrices, sponge.Level);
			sponge.Level++;
			UpdateQps (sponge.Qps);

			Text spongeLevelText = GameObject.Find ("SpongeLevelText").GetComponent<Text> ();
			spongeLevelText.text = UpdateItemLevel (sponge.Level);
			Text spongePriceText = GameObject.Find ("SpongePriceText").GetComponent<Text> ();
			spongePriceText.text = UpdateItemPrice (sponge.Level, sponge.QuackPrices);

			UpdateGameData();
			SaveGameData ();

		}
	}

	public void DeodorantUpgrade()
	{
		if (BuyQualify (deodorant.Level, deodorant.QuackPrices)) {

			BuyItem (deodorant.QuackPrices, deodorant.Level);
			deodorant.Level++;
			UpdateQps (deodorant.Qps);

			Text deodorantLevelText = GameObject.Find ("DeodorantLevelText").GetComponent<Text> ();
			deodorantLevelText.text = UpdateItemLevel (deodorant.Level);
			Text deodorantPriceText = GameObject.Find ("DeodorantPriceText").GetComponent<Text> ();
			deodorantPriceText.text = UpdateItemPrice (deodorant.Level, deodorant.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void ToiletPaperUpgrade()
	{
		if (BuyQualify (toiletPaper.Level, toiletPaper.QuackPrices)) {

			BuyItem (toiletPaper.QuackPrices, toiletPaper.Level);
			toiletPaper.Level++;
			UpdateQps (toiletPaper.Qps);

			Text toiletPaperLevelText = GameObject.Find ("ToiletPaperLevelText").GetComponent<Text> ();
			toiletPaperLevelText.text = UpdateItemLevel (toiletPaper.Level);
			Text toiletPaperPriceText = GameObject.Find ("ToiletPaperPriceText").GetComponent<Text> ();
			toiletPaperPriceText.text = UpdateItemPrice (toiletPaper.Level, toiletPaper.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void HairbrushUpgrade()
	{
		if (BuyQualify (hairbrush.Level, hairbrush.QuackPrices)) {

			BuyItem (hairbrush.QuackPrices, hairbrush.Level);
			hairbrush.Level++;
			UpdateQps (hairbrush.Qps);

			Text hairbrushLevelText = GameObject.Find ("HairbrushLevelText").GetComponent<Text> ();
			hairbrushLevelText.text = UpdateItemLevel (hairbrush.Level);
			Text hairbrushPriceText = GameObject.Find ("HairbrushPriceText").GetComponent<Text> ();
			hairbrushPriceText.text = UpdateItemPrice (hairbrush.Level, hairbrush.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void ToiletBrushUpgrade()
	{
		if (BuyQualify (toiletBrush.Level, toiletBrush.QuackPrices)) {

			BuyItem (toiletBrush.QuackPrices, toiletBrush.Level);
			toiletBrush.Level++;
			UpdateQps (toiletBrush.Qps);

			Text toiletBrushLevelText = GameObject.Find ("ToiletBrushLevelText").GetComponent<Text> ();
			toiletBrushLevelText.text = UpdateItemLevel (toiletBrush.Level);
			Text toiletBrushPriceText = GameObject.Find ("ToiletBrushPriceText").GetComponent<Text> ();
			toiletBrushPriceText.text = UpdateItemPrice (toiletBrush.Level, toiletBrush.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void PlungerUpgrade()
	{
		if (BuyQualify (plunger.Level, plunger.QuackPrices)) {

			BuyItem (plunger.QuackPrices, plunger.Level);
			plunger.Level++;
			UpdateQps (plunger.Qps);

			Text plungerLevelText = GameObject.Find ("PlungerLevelText").GetComponent<Text> ();
			plungerLevelText.text = UpdateItemLevel (plunger.Level);
			Text plungerPriceText = GameObject.Find ("PlungerPriceText").GetComponent<Text> ();
			plungerPriceText.text = UpdateItemPrice (plunger.Level, plunger.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void ScaleUpgrade()
	{
		if (BuyQualify (scale.Level, scale.QuackPrices)) {

			BuyItem (scale.QuackPrices, scale.Level);
			scale.Level++;
			UpdateQps (scale.Qps);

			Text scaleLevelText = GameObject.Find ("ScaleLevelText").GetComponent<Text> ();
			scaleLevelText.text = UpdateItemLevel (scale.Level);
			Text scalePriceText = GameObject.Find ("ScalePriceText").GetComponent<Text> ();
			scalePriceText.text = UpdateItemPrice (scale.Level, scale.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void OvalMirrorUpgrade()
	{
		if (BuyQualify (ovalMirror.Level, ovalMirror.QuackPrices)) {

			BuyItem (ovalMirror.QuackPrices, ovalMirror.Level);
			ovalMirror.Level++;
			UpdateQps (ovalMirror.Qps);

			Text ovalMirrorLevelText = GameObject.Find ("OvalMirrorLevelText").GetComponent<Text> ();
			ovalMirrorLevelText.text = UpdateItemLevel (ovalMirror.Level);
			Text ovalMirrorPriceText = GameObject.Find ("OvalMirrorPriceText").GetComponent<Text> ();
			ovalMirrorPriceText.text = UpdateItemPrice (ovalMirror.Level, ovalMirror.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void TowelUpgrade()
	{
		if (BuyQualify (towel.Level, towel.QuackPrices)) {

			BuyItem (towel.QuackPrices, towel.Level);
			towel.Level++;
			UpdateQps (towel.Qps);

			Text towelLevelText = GameObject.Find ("TowelLevelText").GetComponent<Text> ();
			towelLevelText.text = UpdateItemLevel (towel.Level);
			Text towelPriceText = GameObject.Find ("TowelPriceText").GetComponent<Text> ();
			towelPriceText.text = UpdateItemPrice (towel.Level, towel.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void TowelRackUpgrade()
	{
		if (BuyQualify (towelRack.Level, towelRack.QuackPrices)) {

			BuyItem (towelRack.QuackPrices, towelRack.Level);
			towelRack.Level++;
			UpdateQps (towelRack.Qps);

			Text towelRackLevelText = GameObject.Find ("TowelRackLevelText").GetComponent<Text> ();
			towelRackLevelText.text = UpdateItemLevel (towelRack.Level);
			Text towelRackPriceText = GameObject.Find ("TowelRackPriceText").GetComponent<Text> ();
			towelRackPriceText.text = UpdateItemPrice (towelRack.Level, towelRack.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void HairDryerUpgrade()
	{
		if (BuyQualify (hairDryer.Level, hairDryer.QuackPrices)) {

			BuyItem (hairDryer.QuackPrices, hairDryer.Level);
			hairDryer.Level++;
			UpdateQps (hairDryer.Qps);

			Text hairDryerLevelText = GameObject.Find ("HairDryerLevelText").GetComponent<Text> ();
			hairDryerLevelText.text = UpdateItemLevel (hairDryer.Level);
			Text hairDryerPriceText = GameObject.Find ("HairDryerPriceText").GetComponent<Text> ();
			hairDryerPriceText.text = UpdateItemPrice (hairDryer.Level, hairDryer.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void SinkUpgrade()
	{
		if (BuyQualify (sink.Level, sink.QuackPrices)) {

			BuyItem (sink.QuackPrices, sink.Level);
			sink.Level++;
			UpdateQps (sink.Qps);

			Text sinkLevelText = GameObject.Find ("SinkLevelText").GetComponent<Text> ();
			sinkLevelText.text = UpdateItemLevel (sink.Level);
			Text sinkPriceText = GameObject.Find ("SinkPriceText").GetComponent<Text> ();
			sinkPriceText.text = UpdateItemPrice (sink.Level, sink.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void ToiletUpgrade()
	{
		if (BuyQualify (toilet.Level, toilet.QuackPrices)) {

			BuyItem (toilet.QuackPrices, toilet.Level);
			toilet.Level++;
			UpdateQps (toilet.Qps);

			Text toiletLevelText = GameObject.Find ("ToiletLevelText").GetComponent<Text> ();
			toiletLevelText.text = UpdateItemLevel (toilet.Level);
			Text toiletPriceText = GameObject.Find ("ToiletPriceText").GetComponent<Text> ();
			toiletPriceText.text = UpdateItemPrice (toilet.Level, toilet.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void ShowerUpgrade()
	{
		if (BuyQualify (shower.Level, shower.QuackPrices)) {

			BuyItem (shower.QuackPrices, shower.Level);
			shower.Level++;
			UpdateQps (shower.Qps);

			Text showerLevelText = GameObject.Find ("ShowerLevelText").GetComponent<Text> ();
			showerLevelText.text = UpdateItemLevel (shower.Level);
			Text showerPriceText = GameObject.Find ("ShowerPriceText").GetComponent<Text> ();
			showerPriceText.text = UpdateItemPrice (shower.Level, shower.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void BathtubUpgrade()
	{
		if (BuyQualify (bathtub.Level, bathtub.QuackPrices)) {

			BuyItem (bathtub.QuackPrices, bathtub.Level);
			bathtub.Level++;
			UpdateQps (bathtub.Qps);

			Text bathtubLevelText = GameObject.Find ("BathtubLevelText").GetComponent<Text> ();
			bathtubLevelText.text = UpdateItemLevel (bathtub.Level);
			Text bathtubPriceText = GameObject.Find ("BathtubPriceText").GetComponent<Text> ();
			bathtubPriceText.text = UpdateItemPrice (bathtub.Level, bathtub.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

	public void WashingMachineUpgrade()
	{
		if (BuyQualify (washingMachine.Level, washingMachine.QuackPrices)) {

			BuyItem (washingMachine.QuackPrices, washingMachine.Level);
			washingMachine.Level++;
			UpdateQps (washingMachine.Qps);

			Text washingMachineLevelText = GameObject.Find ("WashingMachineLevelText").GetComponent<Text> ();
			washingMachineLevelText.text = UpdateItemLevel (washingMachine.Level);
			Text washingMachinePriceText = GameObject.Find ("WashingMachinePriceText").GetComponent<Text> ();
			washingMachinePriceText.text = UpdateItemPrice (washingMachine.Level, washingMachine.QuackPrices);

			UpdateGameData ();
			SaveGameData ();

		}
	}

    #endregion
}
	