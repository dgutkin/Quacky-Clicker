using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{

	[Header("Items")]

	static int itemCount = 20;
	public Item[] items = new Item[itemCount];

	[Header("Item Texts")]

	public Text quackCountText;

	public Text[] itemLevelTexts;

	public Text[] itemPriceTexts;

	[Header("Sounds")]

	public AudioClip[] gameBonusClickSound;

	[Header("Background Toggles")]

	public SpriteToggle[] backgroundToggles;
	
	public GameObject levelUpEffect;

	public Number number = new Number();



    #region --------------- ITEM DATA ---------------

    public void LoadItemData()
    {

    	items[0] = new Item(0, 25, 1, 0, "POWERCLICK");
		items[1] = new Item(0, 10, 1, 1, "TOOTHBRUSH");
		items[2] = new Item (0, 10, 1, 2, "TOOTHPASTE");
		items[3] = new Item(0, 10, 1, 5, "SOAP");
		items[4] = new Item (0, 10, 1, 10, "SPONGE");
		items[5] = new Item (0, 10, 1, 20, "DEODORANT");
		items[6] = new Item (0, 10, 1, 50, "TOILETPAPER");
		items[7] = new Item (0, 10, 1, 100, "HAIRBRUSH");
		items[8] = new Item (0, 10, 1, 500, "TOILETBRUSH");
		items[9] = new Item (0, 10, 1, 1000, "PLUNGER");
		items[10] = new Item (0, 10, 1, 5000, "SCALE");
		items[11] = new Item (0, 10, 1, 10000, "OVALMIRROR");
		items[12] = new Item (0, 10, 1, 50000, "TOWEL");
		items[13] = new Item (0, 10, 1, 100000, "TOWELRACK");
		items[14] = new Item (0, 10, 1, 500000, "HAIRDRYER");
		items[15] = new Item (0, 10, 1, 1000000, "SINK");
		items[16] = new Item (0, 10, 1, 5000000, "TOILET");
		items[17] = new Item (0, 10, 1, 10000000, "SHOWER");
		items[18] = new Item (0, 10, 1, 50000000, "BATHTUB");
		items[19] = new Item (0, 10, 1, 100000000, "WASHINGMACHINE");

		GameObject duckManager = GameObject.Find("DuckManager");
		DuckManager duckManagerScript = duckManager.GetComponent<DuckManager>();
    	ulong baseQps = duckManagerScript.baseQps;
    	int clickValue = duckManagerScript.clickValue;

  
		for (int i = 0; i < itemCount; i++) 
		{
			items[i].Level = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + items[i].Name + "_LEVEL", 0);
			if (i == 0) {
				items[i].Price = CalculatePowerClickPrice(items[i].Level);
			} else {
				items[i].Price = CalculateItemPrice(baseQps, clickValue, items[i].Qps);
				backgroundToggles[i-1].ToggleState = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + items[i].Name + "_TOGGLESTATE", -1);
				if (backgroundToggles[i-1].ToggleState == 1)
					duckManagerScript.spawnDuckObjectsIndex = i;
			}
			items[i].Price = System.Convert.ToUInt64(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + items[i].Name + "_PRICE", items[i].Price.ToString()));
		}
    }

    public void SetItemData()
    {
		for (int i = 0; i < itemCount; i++)
		{
			itemLevelTexts[i].text = UpdateItemLevel(items[i].Level);
			if (i == 0){
				itemPriceTexts[i].text = "Cost: " + UpdateItemPrice(items[i].Level, items[i].LevelCount, items[i].Price);
			} else {
				itemPriceTexts[i].text = UpdateItemPrice(items[i].Level, items[i].LevelCount, items[i].Price);		
			}
		}
    }

    public void SaveItemData()
    {
		for (int i = 0; i < itemCount; i++) 
		{
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + items[i].Name + "_LEVEL", items[i].Level);
			PlayerPrefs.SetString(SceneManager.GetActiveScene().name + items[i].Name + "_PRICE", items[i].Price.ToString());
			if (i > 0)
				PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + items[i].Name + "_TOGGLESTATE", backgroundToggles[i-1].ToggleState);
		}
    }

    #endregion

    #region --------------- ITEM UPGRADES ---------------

    public void UpdateQuacksText(ulong quackScore)
	{
		quackCountText.text = quackScore.ToString("N0") + " Quacks";
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

	public string UpdateItemLevel(int itemLevel)
	{
		return itemLevel.ToString();
	}

	public string UpdateItemPrice(int itemLevel, int itemLevelCount, ulong itemPrice)
	{
		if (itemLevel == itemLevelCount) {

			return "Maxed";

		} else {

			return number.FormatLargeNumber(itemPrice) + " quacks";
		}
	}

	public bool BuyQualify(int itemLevel, int itemLevelCount, ulong itemPrice, ulong quackScore)
	{
		if ((itemLevel < itemLevelCount) && (quackScore >= itemPrice)) {
			
			return true;

		} else {
			
			GameObject duckManager = GameObject.Find("DuckManager");
			DuckManager duckManagerScript = duckManager.GetComponent<DuckManager>();	
			duckManagerScript.ButtonSound ();
			return false;

		}
	}

	public void BuyItem(ulong itemPrice, int itemLevel)
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		DuckManager duckManagerScript = duckManager.GetComponent<DuckManager>();
		duckManagerScript.PlaySound (gameBonusClickSound [Random.Range (0, gameBonusClickSound.Length)]);
		duckManagerScript.quackScore = duckManagerScript.quackScore - itemPrice;
		UpdateQuacksText(duckManagerScript.quackScore);
	}

	public void PowerClickUpgrade()
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		DuckManager duckManagerScript = duckManager.GetComponent<DuckManager>();

		if (BuyQualify (items[0].Level, items[0].LevelCount, items[0].Price, duckManagerScript.quackScore)) {

			Vector2 spawnLevelUpPos = itemLevelTexts[0].GetComponent<RectTransform>().transform.position;
            GameObject newLevelUpEffect = (GameObject)(Instantiate(levelUpEffect, spawnLevelUpPos, Quaternion.identity));
            newLevelUpEffect.GetComponent<ScoreEffect>().SetScoreValue(1);
            
			BuyItem (items[0].Price, items[0].Level);
			items[0].Level++;
			items[0].Price = CalculatePowerClickPrice(items[0].Level);
			duckManagerScript.clickValue *= 2; // double the click value

			itemLevelTexts[0].text = UpdateItemLevel (items[0].Level);
			itemPriceTexts[0].text = "Cost: " + UpdateItemPrice (items[0].Level, items[0].LevelCount, items[0].Price);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}

	}

	public void ItemUpgrade(int itemIndex)
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		DuckManager duckManagerScript = duckManager.GetComponent<DuckManager>();
		
		if (BuyQualify (items[itemIndex].Level, items[itemIndex].LevelCount, items[itemIndex].Price, duckManagerScript.quackScore)) {

			Vector2 spawnLevelUpPos = itemLevelTexts[itemIndex].GetComponent<RectTransform>().transform.position;
            GameObject newLevelUpEffect = (GameObject)(Instantiate(levelUpEffect, spawnLevelUpPos, Quaternion.identity));
            newLevelUpEffect.GetComponent<ScoreEffect>().SetScoreValue(1);

			BuyItem (items[itemIndex].Price, items[itemIndex].Level);
			items[itemIndex].Level++;
			duckManagerScript.baseQps += (ulong) items[itemIndex].Qps;
			ulong newPrice = CalculateItemPrice(duckManagerScript.baseQps, duckManagerScript.clickValue, items[itemIndex].Qps);
			if (newPrice <= items[itemIndex].Price)
				newPrice = number.IncrementLargeNumber(items[itemIndex].Price, 1);
			items[itemIndex].Price = newPrice;

			itemLevelTexts[itemIndex].text = UpdateItemLevel (items[itemIndex].Level);
			itemPriceTexts[itemIndex].text = UpdateItemPrice (items[itemIndex].Level, items[itemIndex].LevelCount, items[itemIndex].Price);

			SaveItemData();
			duckManagerScript.UpdateGameData ();
			duckManagerScript.SaveGameData ();
		}
	}

	#endregion

	#region --------------- ITEM TOGGLES FOR GAME BACKGROUND ---------------

	public void ResetItemBackgrounds() 
	{
		foreach (SpriteToggle backgroundToggle in backgroundToggles)
			if (backgroundToggle.ToggleState == 1)
				backgroundToggle.SetToggleValue(0);

		GameObject duckManager = GameObject.Find("DuckManager");
		DuckManager duckManagerScript = duckManager.GetComponent<DuckManager>();
		duckManagerScript.spawnDuckObjectsIndex = 0;
	}

	public void ItemBackground(int itemIndex)
	{
		GameObject duckManager = GameObject.Find("DuckManager");
		DuckManager duckManagerScript = duckManager.GetComponent<DuckManager>();
		
		if (itemPriceTexts[itemIndex].text == "Maxed" && backgroundToggles[itemIndex - 1].ToggleState == 0) {
			
			ResetItemBackgrounds();
			backgroundToggles[itemIndex - 1].SetToggleValue(1);
			
			duckManagerScript.spawnDuckObjectsIndex = itemIndex;

		} else if (itemPriceTexts[itemIndex].text == "Maxed" && backgroundToggles[itemIndex - 1].ToggleState == 1) {

			backgroundToggles[itemIndex - 1].SetToggleValue(0);

			duckManagerScript.spawnDuckObjectsIndex = 0;

		} else if (itemPriceTexts[itemIndex].text == "Maxed" && backgroundToggles[itemIndex - 1].ToggleState == -1) {

			backgroundToggles[itemIndex - 1].SetToggleValue(0);

		}
	}

	#endregion
}
