using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class Number
 {

 	public Number()
 	{
 		//constructor
 	}

    public ulong RoundLargeNumber(ulong Number)
	{
		int numberSize = (int) Mathf.Floor(Mathf.Log10(Number));	

		if (numberSize < 3) {
			Number = Number / 1 * 1;
		} else if (numberSize < 6) {
			Number = (Number / 1000) * 1000;
		} else if (numberSize < 9) {
			Number = (Number / 1000000) * 1000000;
		} else if (numberSize < 12) {
			Number = (Number / 1000000000) * 1000000000;
		} else if (numberSize < 15) {
			Number = (Number / 1000000000000) * 1000000000000;
		}

		return Number;
	}

	public string FormatLargeNumber(ulong Number)
	{
		int numberSize = (int) Mathf.Floor(Mathf.Log10(Number));
		string formattedNumber = "";

		if (numberSize < 3) {
			formattedNumber = Number.ToString ("F0");
		} else if (numberSize < 6) {
			formattedNumber = (Number / 1000).ToString ("F0") + "k";
		} else if (numberSize < 9) {
			formattedNumber = (Number / 1000000).ToString ("F0") + "mn";
		} else if (numberSize < 12) {
			formattedNumber = (Number / 1000000000).ToString ("F0") + "bn";
		} else if (numberSize < 15) {
			formattedNumber = (Number / 1000000000000).ToString ("F0") + "tn";
		}

		return formattedNumber;
	}

}
