using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
		
	public int Level;
	public int LevelCount;
	public ulong Price;
	public int Qps;
	public string Name;

	public Item(int itemLevel, int itemLevelCount, ulong itemPrice, int itemQps, string itemName)
	{
		Level = itemLevel;
		LevelCount = itemLevelCount;
		Price = itemPrice;
		Qps = itemQps;
		Name = itemName;

	}

	// public int[] GenerateItemPrices(int powerBase, int scalingFactor, int levelCount) {

	// 	int[] prices = new int[levelCount];

	// 	for (int i = 0; i < levelCount; i++) {
	// 		prices [i] = (int) Mathf.Pow(powerBase, (1 + i/scalingFactor));
	// 	}

	// 	return prices;
	// }
	
}
