using UnityEngine;
using System.Collections;

public class ColorExt {

	public static Color InvertColor (Color color) {
		return new Color (1-color.r, 1-color.g, 1-color.b);
	}

}
