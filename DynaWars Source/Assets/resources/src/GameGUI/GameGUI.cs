using CoherentNoise.Generation;
using CoherentNoise.Generation.Combination;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameGUI
{

	public class Draw {

		public static void Circle (Texture2D tex, float cx, float cy, float r, float ang, float cc, Color col, bool side, int texSize) {
			
			if (!side) {
				
				for (int y=0; y<texSize; y++)
				for (int x=0; x<texSize; x++) {
					
					float dx = x - cx;
					float dy = y - cy;
					float dist = Mathf.Sqrt (dx * dx + dy * dy);
					float angle = Mathf.Atan2 (dx, dy);
					float angulo = (ang - 180) * 0.0174532925f;
					Color colornulo = new Color (0, 0, 0, 0);
					
					if (angle <= angulo && dist < r) {
						tex.SetPixel (x, y, col);
					} else {
						tex.SetPixel (x, y, colornulo);
					}
					if (dist < cc)
						tex.SetPixel (x, y, colornulo);
					
				}
				
			} else 
			{
				
				for (int y=0; y<texSize; y++)
				for (int x=0; x<texSize; x++) {
					
					float dx = x - cx;
					float dy = y - cy;
					float dist = Mathf.Sqrt (dx * dx + dy * dy);
					float angle = -Mathf.Atan2 (dx, dy);
					float angulo = (ang - 180) * 0.0174532925f;
					Color colornulo = new Color (0, 0, 0, 0);
					
					if (angle <= angulo && dist < r) {
						tex.SetPixel (x, y, col);
					} else {
						tex.SetPixel (x, y, colornulo);
					}
					if (dist < cc)
						tex.SetPixel (x, y, colornulo);
					
				}
				
			}
			
			
		}

	}

	public class GUIOptions {

		static bool unlock = false;

		private static void ReadjustCursor() {
			Screen.lockCursor = true;
			Screen.lockCursor = true;
		}

		public static void SetSensitivity(bool InvertMouse, float Sensibility) {

			MouseLook mouseLookX = GameObject.Find("Player").GetComponent<MouseLook>();
			MouseLook mouseLookY = GameObject.Find("Player").transform.FindChild("mainCam").GetComponent<MouseLook>();

			//bool pauseMenu = Menus.pauseMenu;
			//bool waypointCreator = Menus.WaypointCreator;
			bool thirdView = Player.ThirdView;
			GUIPlaces currentPlace = GameObject.Find("GameScripts").GetComponent<GameGUIindex>().currentPlace;

			if(Input.GetKeyDown(mInput.GetKey("Unlock"))) {
				unlock = (unlock == false) ? true : false;
			}


			bool zeroSensitivity = currentPlace == GUIPlaces.optionsMenu || currentPlace == GUIPlaces.settingsMenu || currentPlace == GUIPlaces.waypointCreator || currentPlace == GUIPlaces.Inv || PlayerStats.Killed || unlock || currentPlace == GUIPlaces.shop || currentPlace == GUIPlaces.personal;

			if(zeroSensitivity) {
				mouseLookX.sensitivityX = 0;
				mouseLookX.sensitivityY = 0;
				mouseLookY.sensitivityX = 0;
				mouseLookY.sensitivityY = 0;
				Screen.lockCursor = false;
			} else {

				if (!thirdView) {
					
					if (!InvertMouse) {
						mouseLookX.sensitivityX = Sensibility;
						mouseLookX.sensitivityY = Sensibility;
						mouseLookY.sensitivityX = Sensibility;
						mouseLookY.sensitivityY = Sensibility;
					} else {
						mouseLookX.sensitivityX = -Sensibility;
						mouseLookX.sensitivityY = Sensibility;
						mouseLookY.sensitivityX = -Sensibility;
						mouseLookY.sensitivityY = Sensibility;
					}

				} else {
					
					if (!InvertMouse) {
						mouseLookX.sensitivityX = Sensibility;
						mouseLookX.sensitivityY = Sensibility;
						mouseLookY.sensitivityX = Sensibility;
						mouseLookY.sensitivityY = Sensibility;
					} else {
						mouseLookX.sensitivityX = -Sensibility;
						mouseLookX.sensitivityY = -Sensibility;
						mouseLookY.sensitivityX = -Sensibility;
						mouseLookY.sensitivityY = -Sensibility;
					}

					
				}

				ReadjustCursor();
			}

			//Debug.Log("Sensibilidad al final de la funcion"+mouseLookX.sensitivityX);

		}

		public static float GetSensitivity(bool fromYAxis = false) {
			try {
				if(!fromYAxis) {
					return GameObject.Find("Player").GetComponent<MouseLook>().sensitivityX;
				} else {
					return GameObject.Find("Player").transform.FindChild("mainCam").GetComponent<MouseLook>().sensitivityY;
				}
			} catch {
				return 666;
			}
		}

	}

	public class TextFormat {

		public static string SpecialFormat(float number) {

			string str = number.ToString("F1");

			if(number >= 1000000000000000000) {
				str = (number/1000000000000000000).ToString("F1")+"Q";
			} else if(number >= 1000000000000000) {
				str = (number/1000000000000000).ToString("F1")+"Ct";
			} else if(number >= 1000000000000) {
				str = (number/1000000000000).ToString("F1")+"T";
			} else if(number >= 1000000000) {
				str = (number/1000000000).ToString("F1")+"B";
			} else if(number >= 1000000) {
				str = (number/1000000).ToString("F1")+"M";
			} else if(number >= 1000) {
				str = (number/1000).ToString("F1")+"k";
			}
														
			return str;

		}

	}

	public class DebugScreen : MonoBehaviour {

		private static float _lastUpt = 0;
		
		public static float lastUpt {
			get {return _lastUpt;}
			set {_lastUpt = value;}
		}

		private static float _lastUptV = 0;
		
		public static float lastUptV {
			get {return _lastUptV;}
			set {_lastUptV = value;}
		}

		private static float _lastUptLite = 0;
		
		public static float lastUptLite {
			get {return _lastUptLite;}
			set {_lastUptLite = value;}
		}

		private static float _lastUptVLite = 0;
		
		public static float lastUptVLite {
			get {return _lastUptVLite;}
			set {_lastUptVLite = value;}
		}

		private static string lastRecord;
		private static string lastRecordV;
		private static string lastRecordLite;
		private static string lastRecordVLite;
		private static Vector3 playerPos;
		private static float UpdateTime = 0.25f;

	#if UNITY_EDITOR

		public static string DScreen() {
			
			playerPos = GameObject.Find("Player").transform.position;
			
			if(Time.time > lastUpt + UpdateTime) {
				
				lastRecord = String.Format("Ikilluneitor WIP ({0}, Tiempo de proc: {1} ms, Tiempo de rend: {2} ms) \n T: {3}, V: {4}\n\nx: {5}\ny: {6} (Aprox: {7})\nz: {8}", GameGUI.FPS.FPS.ShowFPS(), 0, (UnityEditor.UnityStats.renderTime*1000).ToString("F1"), GameGUI.TextFormat.SpecialFormat(UnityEditor.UnityStats.triangles), GameGUI.TextFormat.SpecialFormat(UnityEditor.UnityStats.vertices), playerPos.x.ToString("F2"), playerPos.y.ToString("F2"), GameObject.Find("GameScripts").GetComponent<TerrainGenerator>().GetHeights(playerPos.x, playerPos.z), playerPos.z.ToString("F2"));
				
				lastUpt = Time.time;
				
			}

			return lastRecord;
			
		}
		
		public static string DebugVScreen() {
			
			if(Time.time > lastUptV + UpdateTime) {
				
				lastRecordV = String.Format("Uso de VRam: {0}% ({1} MB) de {2} MB\nMemoria usada por la pantalla: {3} MB\nMemoria usada por texturas: {4} MB\nMemoria usada en renderizado: {5} MB\nMemoria total alojada: {6} MB", 
				                     (0/1048576*100/SystemInfo.graphicsMemorySize), 
				                     0, 
				                     SystemInfo.graphicsMemorySize,
				                     UnityEditor.UnityStats.screenBytes/1048576,
				                     UnityEditor.UnityStats.usedTextureMemorySize/1048576,
				                     UnityEditor.UnityStats.renderTextureBytes/1048576,
				                     Profiler.usedHeapSize/1048576);
				
				lastUptV = Time.time;
				
			}

			return lastRecordV;
			
		}

	#endif
		
		public static string DebugScreenLite() {
			
			playerPos = GameObject.Find("Player").transform.position;
			
			if(Time.time > lastUptLite + UpdateTime) {
				
				lastRecordLite = String.Format("Ikilluneitor WIP ({0}) \n\nx: {1}\ny: {2} (Aprox: {3})\nz: {4}", GameGUI.FPS.FPS.ShowFPS(), playerPos.x.ToString("F2"), playerPos.y.ToString("F2"), GameObject.Find("GameScripts").GetComponent<TerrainGenerator>().GetHeights(playerPos.x, playerPos.z), playerPos.z.ToString("F2"));
				
				lastUptLite = Time.time;
				
			}

			return lastRecordLite;
			
		}
		
		public static string DebugVScreenLite() {
			
			if(Time.time > lastUptVLite + UpdateTime) {
				
				lastRecordVLite = String.Format("Memoria total alojada: {0} MB", 
				                     Profiler.usedHeapSize/1048576);
				
				lastUptVLite = Time.time;
				
			}

			return lastRecordVLite;
			
		}

	}

	public class Keyboard {

		public static void DisableAllKeys() {

			if( !Event.current.isKey )
			{
				return;
			} 

			KeyCode[] keys = (KeyCode[])Enum.GetValues(typeof(KeyCode));
			
			foreach( KeyCode key in keys )
			{
				if( Event.current.keyCode == key )
				{
					Event.current.Use();
				}
			}

		}

		public static void DisableKeys( KeyCode[] keys )
		{
			if( !Event.current.isKey )
			{
				return;
			}
			
			foreach( KeyCode key in keys )
			{
				if( Event.current.keyCode == key )
				{
					Event.current.Use();
				}
			}
		}
		
		
		
		public static void DisableKey( KeyCode key )
		{
			DisableKeys( new KeyCode[]{ key } );
		}

	}

}