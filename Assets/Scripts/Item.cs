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
	
}
