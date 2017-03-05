using UnityEngine;
using System.Collections;

namespace GameGUI.FPS
{

	public class FPS
	{
	
		// Attach this to a GUIText to make a frames/second indicator.
		//
		// It calculates frames/second over each updateInterval,
		// so the display does not keep changing wildly.
		//
		// It is also fairly accurate at very low FPS counts (<10).
		// We do this not by simply counting frames per interval, but
		// by accumulating FPS for each frame. This way we end up with
		// correct overall FPS even if the interval renders something like
		// 5.5 frames.

		private static float accum   = 0; // FPS accumulated over the interval
		private static int   frames  = 0; // Frames drawn over the interval
		private static float timeleft; // Left time for current interval

		//private static float Green = 0;
		//private static float Red = 0;
		private	static float maxfps;

		private static string lastRecord;
		private static float timeScale;
		private static float timeDelta;
		private static float lastTimeD;

		/*public void SetValues(float updateInterval2, GUIText guiText2) 
		{
			updateInterval = updateInterval2;
			guiText = guiText2;
		}*/
	
		/*void Start()
		{
		if( !guiText )
		{
			Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
			enabled = false;
			return;
		}
		timeleft = updateInterval;  
		}*/

		private float Avg(float maxnum, float maxconv, float current) 
		{
		return current * maxconv / maxnum;
		}
	
		public static string ShowFPS()
		{

			timeScale = Time.timeScale;
			timeDelta = Time.deltaTime;

			if (timeScale == 0) {
				timeScale = 1;			
			}

			if (timeDelta == 0) {
				timeDelta = lastTimeD;		
			}

			timeleft -= timeDelta;
			accum += timeScale / timeDelta;
			++frames;

			string format = lastRecord;
		
			// Interval ended - update GUI text and start new interval
			//if (timeleft <= 0.0) {

				// display two fractional digits (f2 format)
				float fps = accum / frames;
				format = fps.ToString("F2")+" fps";
				lastRecord = format;

				//timeleft = updateInterval;
				accum = 0.0F;
				frames = 0;

			//}

			if(timeDelta != 0) {
				lastTimeD = timeDelta;
			}

			return format;

		}
	}

}