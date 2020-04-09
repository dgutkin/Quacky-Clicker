using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
		
	public int Level;
	public int[] QuackPrices;
	public int Qps;
	public string Name;

	public Item(int itemLevel, int[] itemQuackPrices, int itemQps, string itemName)
	{
		Level = itemLevel;
		QuackPrices = itemQuackPrices;
		Qps = itemQps;
		Name = itemName;

	}

	public int[] GenerateItemPrices(int powerBase, int scalingFactor, int levelCount) {

		int[] prices = new int[levelCount];

		for (int i = 0; i < levelCount; i++) {
			prices [i] = (int) Mathf.Pow(powerBase, (1 + i/scalingFactor));
		}

		return prices;
	}
	
}
