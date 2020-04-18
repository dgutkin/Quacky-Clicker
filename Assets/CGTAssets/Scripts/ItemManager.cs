using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{

	[Header("Items")]

	public Item powerClick = new Item(0, 30, 1, 0, "PowerClick");
	public Item toothbrush = new Item(0, 100, 1, 1, "Toothbrush");
	public Item toothpaste = new Item (0, 100, 1, 2, "Toothpaste");
	public Item soap = new Item(0, 100, 1, 5, "Soap");
	public Item sponge = new Item (0, 100, 1, 10, "Sponge");
	public Item deodorant = new Item (0, 100, 1, 20, "Deodorant");
	public Item toiletPaper = new Item (0, 100, 1, 50, "ToiletPaper");
	public Item hairbrush = new Item (0, 100, 1, 100, "HairBrush");
	public Item toiletBrush = new Item (0, 100, 1, 500, "ToiletBrush");
	public Item plunger = new Item (0, 100, 1, 1000, "Plunger");
	public Item scale = new Item (0, 100, 1, 5000, "Scale");
	public Item ovalMirror = new Item (0, 100, 1, 10000, "OvalMirror");
	public Item towel = new Item (0, 100, 1, 50000, "Towel");
	public Item towelRack = new Item (0, 100, 1, 100000, "TowelRack");
	public Item hairDryer = new Item (0, 100, 1, 500000, "HairDryer");
	public Item sink = new Item (0, 100, 1, 10000000, "Sink");
	public Item toilet = new Item (0, 100, 1, 5000000, "Toilet");
	public Item shower = new Item (0, 100, 1, 10000000, "Shower");
	public Item bathtub = new Item (0, 100, 1, 50000000, "Bathtub");
	public Item washingMachine = new Item (0, 100, 1, 100000000, "WashingMachine");

	[Header("Item Texts")]

	public Text quackCountText;

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

	public Text powerClickPriceText;
	public Text toothbrushPriceText;
	public Text toothpastePriceText;
	public Text soapPriceText;
	public Text spongePriceText;
	public Text deodorantPriceText;
	public Text toiletPaperPriceText;
	public Text hairbrushPriceText;
	public Text toiletBrushPriceText;
	public Text plungerPriceText;
	public Text scalePriceText;
	public Text ovalMirrorPriceText;
	public Text towelPriceText;
	public Text towelRackPriceText;
	public Text hairDryerPriceText;
	public Text sinkPriceText;
	public Text toiletPriceText;
	public Text showerPriceText;
	public Text bathtubPriceText;
	public Text washingMachinePriceText;

	[Header("Sounds")]
	public AudioClip[] gameBonusClickSound;

	public Number number = new Number();
	
    // Start is called before the first frame update
    void Start()
    {
    	
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region --------------- ITEM DATA ---------------

    public void LoadItemData()
    {
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
    }

    public void SetItemData()
    {
    	GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
    	ulong baseQps = duckManagerScript.baseQps;
    	int clickValue = duckManagerScript.clickValue;

    	powerClick.Price = CalculatePowerClickPrice(powerClick.Level);
		toothbrush.Price = CalculateItemPrice(baseQps, clickValue, toothbrush.Qps);
		toothpaste.Price = CalculateItemPrice(baseQps, clickValue, toothpaste.Qps);
		soap.Price = CalculateItemPrice(baseQps, clickValue, soap.Qps);
		sponge.Price = CalculateItemPrice(baseQps, clickValue, sponge.Qps);
		deodorant.Price = CalculateItemPrice(baseQps, clickValue, deodorant.Qps);
		toiletPaper.Price = CalculateItemPrice(baseQps, clickValue, toiletPaper.Qps);
		hairbrush.Price = CalculateItemPrice(baseQps, clickValue, hairbrush.Qps);
		toiletBrush.Price = CalculateItemPrice(baseQps, clickValue, toiletBrush.Qps);
		plunger.Price = CalculateItemPrice(baseQps, clickValue, plunger.Qps);
		scale.Price = CalculateItemPrice(baseQps, clickValue, scale.Qps);
		ovalMirror.Price = CalculateItemPrice(baseQps, clickValue, ovalMirror.Qps);
		towel.Price = CalculateItemPrice(baseQps, clickValue, towel.Qps);
		towelRack.Price = CalculateItemPrice(baseQps, clickValue, towelRack.Qps);
		hairDryer.Price = CalculateItemPrice(baseQps, clickValue, hairDryer.Qps);
		sink.Price = CalculateItemPrice(baseQps, clickValue, sink.Qps);
		toilet.Price = CalculateItemPrice(baseQps, clickValue, toilet.Qps);
		shower.Price = CalculateItemPrice(baseQps, clickValue, shower.Qps);
		bathtub.Price = CalculateItemPrice(baseQps, clickValue, bathtub.Qps);
		washingMachine.Price = CalculateItemPrice(baseQps, clickValue, washingMachine.Qps);

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

		powerClickPriceText.text = "Cost: " + UpdateItemPrice(powerClick.Level, powerClick.LevelCount, powerClick.Qps, baseQps, clickValue);
		toothbrushPriceText.text = UpdateItemPrice(toothbrush.Level, toothbrush.LevelCount, toothbrush.Qps, baseQps, clickValue);
		toothpastePriceText.text = UpdateItemPrice(toothpaste.Level, toothpaste.LevelCount, toothpaste.Qps, baseQps, clickValue);
		soapPriceText.text = UpdateItemPrice(soap.Level, soap.LevelCount, soap.Qps, baseQps, clickValue);
		spongePriceText.text = UpdateItemPrice(sponge.Level, sponge.LevelCount, sponge.Qps, baseQps, clickValue);
		deodorantPriceText.text = UpdateItemPrice(deodorant.Level, deodorant.LevelCount, deodorant.Qps, baseQps, clickValue);
		toiletPaperPriceText.text = UpdateItemPrice(toiletPaper.Level, toiletPaper.LevelCount, toiletPaper.Qps, baseQps, clickValue);
		hairbrushPriceText.text = UpdateItemPrice(hairbrush.Level, hairbrush.LevelCount, hairbrush.Qps, baseQps, clickValue);
		toiletBrushPriceText.text = UpdateItemPrice(toiletBrush.Level, toiletBrush.LevelCount, toiletBrush.Qps, baseQps, clickValue);
		plungerPriceText.text = UpdateItemPrice(plunger.Level, plunger.LevelCount, plunger.Qps, baseQps, clickValue);
		scalePriceText.text = UpdateItemPrice(scale.Level, scale.LevelCount, scale.Qps, baseQps, clickValue);
		ovalMirrorPriceText.text = UpdateItemPrice(ovalMirror.Level, ovalMirror.LevelCount, ovalMirror.Qps, baseQps, clickValue);
		towelPriceText.text = UpdateItemPrice(towel.Level, towel.LevelCount, towel.Qps, baseQps, clickValue);
		towelRackPriceText.text = UpdateItemPrice(towelRack.Level, towelRack.LevelCount, towelRack.Qps, baseQps, clickValue);
		hairDryerPriceText.text = UpdateItemPrice(hairDryer.Level, hairDryer.LevelCount, hairDryer.Qps, baseQps, clickValue);
		sinkPriceText.text = UpdateItemPrice(sink.Level, sink.LevelCount, sink.Qps, baseQps, clickValue);
		toiletPriceText.text = UpdateItemPrice(toilet.Level, toilet.LevelCount, toilet.Qps, baseQps, clickValue);
		showerPriceText.text = UpdateItemPrice(shower.Level, shower.LevelCount, shower.Qps, baseQps, clickValue);
		bathtubPriceText.text = UpdateItemPrice(bathtub.Level, bathtub.LevelCount, bathtub.Qps, baseQps, clickValue);
		washingMachinePriceText.text = UpdateItemPrice(washingMachine.Level, washingMachine.LevelCount, washingMachine.Qps, baseQps, clickValue);
    }

    public void SaveItemData()
    {
    	PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "POWERCLICK_LEVEL", powerClick.Level);
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
    }

    #endregion

    public void UpdateQuacksText(ulong quackScore)
	{
		quackCountText.text = quackScore.ToString() + " Quacks";
	}

	public ulong CalculatePowerClickPrice(int itemLevel)
	{
		ulong itemPrice = (ulong) Mathf.Pow(10, 1 + (itemLevel / 2.5f));
		
		return number.RoundLargeNumber(itemPrice);
	}

	public ulong CalculateItemPrice(ulong baseQps, int clickValue, int itemQps)
	{
		ulong itemPrice =  (baseQps + (ulong) clickValue  + (ulong) itemQps) * 60;
	
		return number.RoundLargeNumber(itemPrice);
	}

	public string UpdateItemLevel(int itemLevel){

		return itemLevel.ToString();

	}

	public string UpdateItemPrice(int itemLevel, int itemLevelCount, int itemQps, ulong baseQps, int clickValue){

		if (itemLevel == itemLevelCount) {

			return "Maxed";

		} else {

			ulong itemPrice = 0;

			if (itemQps == 0)
			{
				itemPrice = CalculatePowerClickPrice(itemLevel);
			} else {
				itemPrice = CalculateItemPrice(baseQps, clickValue, itemQps);	
			}

			return number.FormatLargeNumber(itemPrice) + " quacks";
		}

	}

	public bool BuyQualify(int itemLevel, int itemLevelCount, ulong itemPrice, ulong quackScore){

		if ((itemLevel < itemLevelCount) && (quackScore >= itemPrice)) {
			
			return true;

		} else {
			
			GameObject duckManager = GameObject.Find("DuckManager");
			CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();	
			duckManagerScript.ButtonSound ();
			return false;

		}

	}

	public void BuyItem(ulong itemPrice, int itemLevel){

		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		duckManagerScript.PlaySound (gameBonusClickSound [Random.Range (0, gameBonusClickSound.Length)]);
		duckManagerScript.quackScore = duckManagerScript.quackScore - itemPrice;
		UpdateQuacksText(duckManagerScript.quackScore);

	}

	public void PowerClickUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();

		if (BuyQualify (powerClick.Level, powerClick.LevelCount, powerClick.Price, duckManagerScript.quackScore)) {

			BuyItem (powerClick.Price, powerClick.Level);
			powerClick.Level++;
			powerClick.Price = CalculatePowerClickPrice(powerClick.Level);
			duckManagerScript.clickValue *= 2; // double the click value

			powerClickLevelText.text = UpdateItemLevel (powerClick.Level);
			powerClickPriceText.text = "Cost: " + UpdateItemPrice (powerClick.Level, powerClick.LevelCount, powerClick.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}

	}

	public void ToothbrushUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (toothbrush.Level, toothbrush.LevelCount, toothbrush.Price, duckManagerScript.quackScore)) {

			BuyItem (toothbrush.Price, toothbrush.Level);
			toothbrush.Level++;
			duckManagerScript.baseQps += (ulong) toothbrush.Qps;
			toothbrush.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, toothbrush.Qps);

			toothbrushLevelText.text = UpdateItemLevel (toothbrush.Level);
			toothbrushPriceText.text = UpdateItemPrice (toothbrush.Level, toothbrush.LevelCount, toothbrush.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void ToothpasteUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (toothpaste.Level, toothpaste.LevelCount, toothpaste.Price, duckManagerScript.quackScore)) {

			BuyItem (toothpaste.Price, toothpaste.Level);
			toothpaste.Level++;
			duckManagerScript.baseQps += (ulong) toothpaste.Qps;
			toothpaste.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, toothpaste.Qps);

			toothpasteLevelText.text = UpdateItemLevel (toothpaste.Level);
			toothpastePriceText.text = UpdateItemPrice (toothpaste.Level, toothpaste.LevelCount, toothpaste.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void SoapUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (soap.Level, soap.LevelCount, soap.Price, duckManagerScript.quackScore)) {

			BuyItem (soap.Price, soap.Level);
			soap.Level++;
			duckManagerScript.baseQps += (ulong) soap.Qps;
			soap.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, soap.Qps);

			soapLevelText.text = UpdateItemLevel (soap.Level);
			soapPriceText.text = UpdateItemPrice (soap.Level, soap.LevelCount, soap.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void SpongeUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (sponge.Level, sponge.LevelCount, sponge.Price, duckManagerScript.quackScore)) {

			BuyItem (sponge.Price, sponge.Level);
			sponge.Level++;
			duckManagerScript.baseQps += (ulong) sponge.Qps;
			sponge.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, sponge.Qps);

			spongeLevelText.text = UpdateItemLevel (sponge.Level);
			spongePriceText.text = UpdateItemPrice (sponge.Level, sponge.LevelCount, sponge.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void DeodorantUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (deodorant.Level, deodorant.LevelCount, deodorant.Price, duckManagerScript.quackScore)) {

			BuyItem (deodorant.Price, deodorant.Level);
			deodorant.Level++;
			duckManagerScript.baseQps += (ulong) deodorant.Qps;
			deodorant.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, deodorant.Qps);

			deodorantLevelText.text = UpdateItemLevel (deodorant.Level);
			deodorantPriceText.text = UpdateItemPrice (deodorant.Level, deodorant.LevelCount, deodorant.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void ToiletPaperUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (toiletPaper.Level, toiletPaper.LevelCount, toiletPaper.Price, duckManagerScript.quackScore)) {

			BuyItem (toiletPaper.Price, toiletPaper.Level);
			toiletPaper.Level++;
			duckManagerScript.baseQps += (ulong) toiletPaper.Qps;
			toiletPaper.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, toiletPaper.Qps);

			toiletPaperLevelText.text = UpdateItemLevel (toiletPaper.Level);
			toiletPaperPriceText.text = UpdateItemPrice (toiletPaper.Level, toiletPaper.LevelCount, toiletPaper.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void HairbrushUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (hairbrush.Level, hairbrush.LevelCount, hairbrush.Price, duckManagerScript.quackScore)) {

			BuyItem (hairbrush.Price, hairbrush.Level);
			hairbrush.Level++;
			duckManagerScript.baseQps += (ulong) hairbrush.Qps;
			hairbrush.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, hairbrush.Qps);

			hairbrushLevelText.text = UpdateItemLevel (hairbrush.Level);
			hairbrushPriceText.text = UpdateItemPrice (hairbrush.Level, hairbrush.LevelCount, hairbrush.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void ToiletBrushUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (toiletBrush.Level, toiletBrush.LevelCount, toiletBrush.Price, duckManagerScript.quackScore)) {

			BuyItem (toiletBrush.Price, toiletBrush.Level);
			toiletBrush.Level++;
			duckManagerScript.baseQps += (ulong) toiletBrush.Qps;
			toiletBrush.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, toiletBrush.Qps);

			toiletBrushLevelText.text = UpdateItemLevel (toiletBrush.Level);
			toiletBrushPriceText.text = UpdateItemPrice (toiletBrush.Level, toiletBrush.LevelCount, toiletBrush.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void PlungerUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (plunger.Level, plunger.LevelCount, plunger.Price, duckManagerScript.quackScore)) {

			BuyItem (plunger.Price, plunger.Level);
			plunger.Level++;
			duckManagerScript.baseQps += (ulong) plunger.Qps;
			plunger.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, plunger.Qps);

			plungerLevelText.text = UpdateItemLevel (plunger.Level);
			plungerPriceText.text = UpdateItemPrice (plunger.Level, plunger.LevelCount, plunger.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void ScaleUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (scale.Level, scale.LevelCount, scale.Price, duckManagerScript.quackScore)) {

			BuyItem (scale.Price, scale.Level);
			scale.Level++;
			duckManagerScript.baseQps += (ulong) scale.Qps;
			scale.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, scale.Qps);

			scaleLevelText.text = UpdateItemLevel (scale.Level);
			scalePriceText.text = UpdateItemPrice (scale.Level, scale.LevelCount, scale.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void OvalMirrorUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (ovalMirror.Level, ovalMirror.LevelCount, ovalMirror.Price, duckManagerScript.quackScore)) {

			BuyItem (ovalMirror.Price, ovalMirror.Level);
			ovalMirror.Level++;
			duckManagerScript.baseQps += (ulong) ovalMirror.Qps;
			ovalMirror.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, ovalMirror.Qps);

			ovalMirrorLevelText.text = UpdateItemLevel (ovalMirror.Level);
			ovalMirrorPriceText.text = UpdateItemPrice (ovalMirror.Level, ovalMirror.LevelCount, ovalMirror.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void TowelUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (towel.Level, towel.LevelCount, towel.Price, duckManagerScript.quackScore)) {

			BuyItem (towel.Price, towel.Level);
			towel.Level++;
			duckManagerScript.baseQps += (ulong) towel.Qps;
			towel.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, towel.Qps);

			towelLevelText.text = UpdateItemLevel (towel.Level);
			towelPriceText.text = UpdateItemPrice (towel.Level, towel.LevelCount, towel.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void TowelRackUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (towelRack.Level, towelRack.LevelCount, towelRack.Price, duckManagerScript.quackScore)) {

			BuyItem (towelRack.Price, towelRack.Level);
			towelRack.Level++;
			duckManagerScript.baseQps += (ulong) towelRack.Qps;
			towelRack.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, towelRack.Qps);

			towelRackLevelText.text = UpdateItemLevel (towelRack.Level);
			towelRackPriceText.text = UpdateItemPrice (towelRack.Level, towelRack.LevelCount, towelRack.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void HairDryerUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (hairDryer.Level, hairDryer.LevelCount, hairDryer.Price, duckManagerScript.quackScore)) {

			BuyItem (hairDryer.Price, hairDryer.Level);
			hairDryer.Level++;
			duckManagerScript.baseQps += (ulong) hairDryer.Qps;
			hairDryer.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, hairDryer.Qps);

			hairDryerLevelText.text = UpdateItemLevel (hairDryer.Level);
			hairDryerPriceText.text = UpdateItemPrice (hairDryer.Level, hairDryer.LevelCount, hairDryer.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void SinkUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (sink.Level, sink.LevelCount, sink.Price, duckManagerScript.quackScore)) {

			BuyItem (sink.Price, sink.Level);
			sink.Level++;
			duckManagerScript.baseQps += (ulong) sink.Qps;
			sink.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, sink.Qps);

			sinkLevelText.text = UpdateItemLevel (sink.Level);
			sinkPriceText.text = UpdateItemPrice (sink.Level, sink.LevelCount, sink.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void ToiletUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (toilet.Level, toilet.LevelCount, toilet.Price, duckManagerScript.quackScore)) {

			BuyItem (toilet.Price, toilet.Level);
			toilet.Level++;
			duckManagerScript.baseQps += (ulong) toilet.Qps;
			toilet.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, toilet.Qps);

			toiletLevelText.text = UpdateItemLevel (toilet.Level);
			toiletPriceText.text = UpdateItemPrice (toilet.Level, toilet.LevelCount, toilet.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void ShowerUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (shower.Level, shower.LevelCount, shower.Price, duckManagerScript.quackScore)) {

			BuyItem (shower.Price, shower.Level);
			shower.Level++;
			duckManagerScript.baseQps += (ulong) shower.Qps;
			shower.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, shower.Qps);

			showerLevelText.text = UpdateItemLevel (shower.Level);
			showerPriceText.text = UpdateItemPrice (shower.Level, shower.LevelCount, shower.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void BathtubUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (bathtub.Level, bathtub.LevelCount, bathtub.Price, duckManagerScript.quackScore)) {

			BuyItem (bathtub.Price, bathtub.Level);
			bathtub.Level++;
			duckManagerScript.baseQps += (ulong) bathtub.Qps;
			bathtub.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, bathtub.Qps);

			bathtubLevelText.text = UpdateItemLevel (bathtub.Level);
			bathtubPriceText.text = UpdateItemPrice (bathtub.Level, bathtub.LevelCount, bathtub.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	public void WashingMachineUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		CGTCookieManager duckManagerScript = duckManager.GetComponent<CGTCookieManager>();
		
		if (BuyQualify (washingMachine.Level, washingMachine.LevelCount, washingMachine.Price, duckManagerScript.quackScore)) {

			BuyItem (washingMachine.Price, washingMachine.Level);
			washingMachine.Level++;
			duckManagerScript.baseQps += (ulong) washingMachine.Qps;
			washingMachine.Price = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, washingMachine.Qps);

			washingMachineLevelText.text = UpdateItemLevel (washingMachine.Level);
			washingMachinePriceText.text = UpdateItemPrice (washingMachine.Level, washingMachine.LevelCount, washingMachine.Qps, duckManagerScript.baseQps, duckManagerScript.clickValue);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}
}
