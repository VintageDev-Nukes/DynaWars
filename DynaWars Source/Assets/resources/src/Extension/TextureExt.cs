using UnityEngine;
using System.Collections;

public class TextureExt {

	public static Texture2D ReplaceColor(Texture2D mainTex, Color findColor, Color newColor) {

		Texture2D finalTex = new Texture2D(mainTex.width, mainTex.height, TextureFormat.ARGB32, false, false);

		for(int x = 0; x < mainTex.width; x++) {
			for(int y = 0; y < mainTex.height; y++) {
				if(mainTex.GetPixel(x, y) == findColor) {
					finalTex.SetPixel(x, y, newColor);
				} else {
					finalTex.SetPixel(x, y, mainTex.GetPixel(x, y));
				}
			}
		}

		finalTex.Apply();

		return finalTex;

	}

}
