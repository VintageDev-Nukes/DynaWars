using UnityEngine;
using System.Collections;

public struct Range
{
	public int min;
	public int max;
	public int range {get {return max-min + 1;}}
	public Range(int aMin, int aMax)
	{
		min = aMin; max = aMax;
	}
}

public class Ranges {
	
	public static int RandomValueFromRanges(params Range[] ranges)
	{
		if (ranges.Length == 0)
			return 0;
		int count = 0;
		foreach(Range r in ranges)
			count += r.range;
		int sel = Random.Range(0,count);
		foreach(Range r in ranges)
		{
			if (sel < r.range)
			{
				return r.min + sel;
			}
			sel -= r.range;
		}
		throw new System.Exception("This should never happen");
	}
	
}

public class RandomExt {

	public static bool PRand(float probabilidad = 100) {
		System.Random random = new System.Random();
		return random.NextDouble() < (probabilidad/100);
	}

}
