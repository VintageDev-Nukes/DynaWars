using UnityEngine;
using System;
using System.Collections;

public class SeedExtension {

	public SeedExtension(string strSeed) {
		seedKey = strSeed;
	}

	public string seedKey;

	public int ToInt() {
		int InternalSeed = 0;
		for(int x=0;x<seedKey.Length;x++) {
			if(!Char.IsDigit(seedKey[x])) {
				InternalSeed += Convert.ToInt32(seedKey[x])*(int)Math.Pow((double)x, 3);
			} else {
				InternalSeed += (int)Char.GetNumericValue(seedKey[x])*(int)Math.Pow((double)x, 3);
			}
		}
		return InternalSeed;
	}

}
