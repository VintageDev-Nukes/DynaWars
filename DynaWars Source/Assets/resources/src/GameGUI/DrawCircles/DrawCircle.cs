using UnityEngine;
using System.Collections;

public class DrawCircle {

	/*public void DrawCircle(float Radio, Color color) 
		: this(Radio, 360, 0, color)
	{
	}

	public void DrawCircle(float Radio, float Angulo, Color color) 
		: this(Radio, Angulo, 0, color)
	{
	}*/

	/*public void Draw(Texture2D circle, float Radio, float Angulo, float RadiusPadding, Color color) 
	{

		circle = new Texture2D(Radio*2, Radio*2, TextureFormat.ARGB32, false);

		//circle = new Texture2D(Radio*2, Radio*2, TextureFormat.ARGB32, false);

		for(float x = 0; x < Angulo; x++) {



			//GUIUtility.RotateAroundPivot(Angulo, new Vector2(0, 0));
			//GUI.DrawTexture(new Rect(0, 0, Radio, 1), texture);

		}

		circle.Apply();

	}*/

	int texSize = 256;

	public void Circle (Texture2D tex, int cx, int cy, int r, Color col) {

		//Circle = new Texture2D(texSize, texSize);

		tex = new Texture2D(texSize, texSize);

		int y = r;
		float d = 1/4 - r;
		float end = Mathf.Ceil(r/Mathf.Sqrt(2));
		
		for (int x = 0; x <= end; x++) {
			tex.SetPixel(cx+x, cy+y, col);
			tex.SetPixel(cx+x, cy-y, col);
			tex.SetPixel(cx-x, cy+y, col);
			tex.SetPixel(cx-x, cy-y, col);
			tex.SetPixel(cx+y, cy+x, col);
			tex.SetPixel(cx-y, cy+x, col);
			tex.SetPixel(cx+y, cy-x, col);
			tex.SetPixel(cx-y, cy-x, col);
			
			d += 2*x+1;
			if (d > 0) {
				d += 2 - 2*y--;
			}
		}

		tex.Apply();

	}

}
