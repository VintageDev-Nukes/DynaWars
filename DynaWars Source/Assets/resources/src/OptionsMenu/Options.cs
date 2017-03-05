using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum OptionsMenu {StartMenu, ControlsMenu, VideoMenu, DriverMenu, Minimap, WaypointEditor, ColorSelector};

public class Options {

	//Menus
	private static OptionsMenu currentMenu = OptionsMenu.StartMenu;
		//bool MenuStart = true, ControlsMenu, VideoMenu, DriverMenu, Minimap;
	
	//Shared Settings Variables
	private static int _qualityLvl, _vSync, selectedIndex;
	private static float _fov, _Sensitivity, _brightness, _renderDistance, _maxFPS, _minimapZoom, tempQualityLvl, cScroll, wpRed, wpGreen, wpBlue;
	private static bool _invertMouse, setNewKey, wpColorPalette, activedPalette;
	private static Texture2D wpColorPreview;
	private static Color finalColor;
	
	public static GUIStyle warn;

	public static int QualityLvl {
		get {return _qualityLvl;}
		set {_qualityLvl = value;}
	}
	
	public static float FOV {
		get {return _fov;}
		set {_fov = value;}
	}
	
	public static float Sensitivity {
		get {return _Sensitivity;}
		set {_Sensitivity = value;}
	}
	
	public static float Brightness {
		get {return _brightness;}
		set {_brightness = value;}
	}
	
	public static float RenderDistance {
		get {return _renderDistance;}
		set {_renderDistance = value;}
	}
	
	public static float MaxFPS {
		get {return _maxFPS;}
		set {_maxFPS = value;}
	}
	
	public static bool InvertMouse {
		get {return _invertMouse;}
		set {_invertMouse = value;}
	}
	
	public static int VSync {
		get {return _vSync;}
		set {_vSync = value;}
	}

	public static float MinimapZoom {
		get {return _minimapZoom;}
		set {_minimapZoom = value;}
	}

	private static Camera Camara;

	//Textures
	private static Texture2D preTab;

	private static Dictionary<string, KeyCode> tempInputMap;

	/*private static string DataConfig() {
		Debug.Log ("Data config function was setted.");
		return "FOV=60" + Environment.NewLine + "Sensitivity=15" + Environment.NewLine + "Brightness=0.3" + Environment.NewLine + "RenderDistance=1500" + Environment.NewLine + "MaxFPS=-1" + Environment.NewLine + "InvertMouse=0" + Environment.NewLine + "VSync=1" + Environment.NewLine + "MinimapZoom=100";
	}

	public static void CreateConfig() {

		if (!System.IO.File.Exists (Application.dataPath + "/appConfig.cfg")) {
			using (System.IO.FileStream fs = System.IO.File.Create(Application.dataPath + "/appConfig.cfg")) {
				Byte[] info = new System.Text.UTF8Encoding (true).GetBytes (DataConfig());
				// Add some information to the file.
				fs.Write (info, 0, info.Length);
				fs.Close();
			}
		}
		
	}*/

	private static void ApplyChanges() {

		//Var != 0 is for avoid bugs

		Profile.currentProfile.QualityLvl = QualityLvl;

		if (FOV != 0) {
			Profile.currentProfile.FOV = FOV;
		}

		if (Sensitivity != 0) {
			Profile.currentProfile.Sensitivity = Sensitivity;
		}

		if(Brightness != 0) {
			Profile.currentProfile.Brightness = Brightness;
		}

		if(RenderDistance != 0) {
			Profile.currentProfile.RenderDistance = RenderDistance;
		}

		if (MaxFPS != 0) {
			Profile.currentProfile.MaxFPS = MaxFPS;
		} else if (MaxFPS < 10) {
			Profile.currentProfile.MaxFPS = -1;	
		}

		Profile.currentProfile.InvertMouse = InvertMouse;

		if (MinimapZoom != 0) {
			Profile.currentProfile.MinimapZoom = MinimapZoom;	
		}

		SetValues();
		
	}

	public static void SetValues(bool fromProfile = false) {

		preTab = (Texture2D)Resources.Load("images/preTab2");

		if(fromProfile) {
			QualityLvl = Profile.currentProfile.QualityLvl;
			FOV = Profile.currentProfile.FOV;
			Sensitivity = Profile.currentProfile.Sensitivity;
			RenderDistance = Profile.currentProfile.RenderDistance;
			MaxFPS = Profile.currentProfile.MaxFPS;
			VSync = Profile.currentProfile.VSync;
			InvertMouse = Profile.currentProfile.InvertMouse;
			Brightness = Profile.currentProfile.Brightness;
			MinimapZoom = Profile.currentProfile.MinimapZoom;
		}

		//That is for set default values if the app is bugged

		#pragma warning disable

		if(QualityLvl == null) {
			QualityLvl = 3;
		}

		if (FOV == 0 || FOV == null) {
			FOV = 60;		
		}

		if (Sensitivity == 0 || Sensitivity == null) {
			Sensitivity = 15;		
		}

		if (RenderDistance == 0 || RenderDistance == null) {
			RenderDistance = 1000;		
		}

		if (Brightness == null) {
			Brightness = 0.3f;		
		}

		if (MaxFPS == null) {
			MaxFPS = 60;		
		}

		if (VSync == null) {
			VSync = 1;		
		}

		if (InvertMouse == null) {
			InvertMouse = false;		
		}

		if (MinimapZoom == 0 || MinimapZoom == null) {
			MinimapZoom = 100;
		}
		#pragma warning restore

	}

	public static bool Draw(string style) {

		warn = new GUIStyle();

		warn.alignment = TextAnchor.MiddleCenter;
		warn.fontSize = 10;
		warn.fontStyle = FontStyle.Bold;
		warn.normal.textColor = Color.white;

		float MarginH = Screen.height*0.45f;
		float MarginW = Screen.width*0.4f;

		if(style != "menu") {
			GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);
			MarginH = Screen.height/2;
			MarginW = Screen.width/2;
		}

		switch(currentMenu) {

		case OptionsMenu.StartMenu:

			if(GUI.Button(new Rect(MarginW-300, MarginH-287+75+515-165, 300, 50), "Ajustes de video")) {
				currentMenu = OptionsMenu.VideoMenu;
			}
		
			if(GUI.Button(new Rect(MarginW+15, MarginH-287+75+515-165, 300, 50), "Controles")) {
				tempInputMap = mInput.InputMap;
				currentMenu = OptionsMenu.ControlsMenu;
			}
		
			if(GUI.Button(new Rect(MarginW-300, MarginH-287+75+515-230, 300, 50), "Ajustes de controladores")) {
				currentMenu = OptionsMenu.DriverMenu;
			}

			if(GUI.Button(new Rect(MarginW+15, MarginH-287+75+515-230, 300, 50), "Ajustes del Minimapa")) {
				currentMenu = OptionsMenu.Minimap;
			}
		
			if(GUI.Button(new Rect(MarginW-100, MarginH-287+75+515-95, 200, 50), "Hecho")) {
				return false;
			}
			break;

		case OptionsMenu.VideoMenu:

			if(style != "menu") {
				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);
			}

			string strqualityLvl = "";
			
			if(QualityLvl == 5) {
				strqualityLvl = "Fantásticos";
			} else if(QualityLvl >= 4 && QualityLvl < 5) {
				strqualityLvl = "Muy buenos";
			} else if(QualityLvl >= 3 && QualityLvl < 4) {
				strqualityLvl = "Buenos";
			} else if(QualityLvl >= 2 && QualityLvl < 3) {
				strqualityLvl = "Intermedios";
			} else if(QualityLvl >= 1 && QualityLvl < 2) {
				strqualityLvl = "Malos";
			} else if(QualityLvl >= 0 && QualityLvl < 1) {
				strqualityLvl = "Muy malos";
			}
			
			GUI.Label(new Rect(MarginW-300, MarginH-287+50, 300, 30), "Gráficos: " + strqualityLvl);
			
			tempQualityLvl = GUI.HorizontalSlider(new Rect(MarginW-300, MarginH-287+75, 300, 20), tempQualityLvl, 5, 0);

			QualityLvl = Mathf.RoundToInt(tempQualityLvl);

			string FOVresult = "";
			
			if(FOV == 120) {
				FOVresult = "Quake Pro";
			} else if(FOV == 60) {
				FOVresult = "Normal";
			} else {
				FOVresult = Mathf.FloorToInt(FOV).ToString();
			}
			
			GUI.Label(new Rect(MarginW+15, MarginH-287+50, 300, 30), "FOV: " + FOVresult);
			
			FOV = GUI.HorizontalSlider(new Rect(MarginW+15, MarginH-287+75, 300, 20), FOV, 60, 120);
			
			string Brightresult = "";
			
			if(Brightness <= 0.01f) {
				Brightresult = "Oscuro";
			} else if(Brightness == 0.3f) {
				Brightresult = "Claro";
			} else {
				Brightresult = Mathf.FloorToInt(((Brightness - 0.01f) / (0.3f - 0.01f)) * 100).ToString();
			}
			
			GUI.Label(new Rect(MarginW-300, MarginH-287+100, 300, 30), "Brillo: " + Brightresult);
			
			Brightness = GUI.HorizontalSlider(new Rect(MarginW-300, MarginH-287+125, 300, 20), Brightness, 0.01f, 0.3f);
			
			GUI.Label(new Rect(MarginW+15, MarginH-287+100, 300, 30), "Distancia de renderizado: " + (RenderDistance/1000).ToString("F1") + " Km");
			
			RenderDistance = GUI.HorizontalSlider(new Rect(MarginW+15, MarginH-287+125, 300, 20), RenderDistance, 100, 8000);

			string FPString = "";

			if(MaxFPS <= 10) {
				VSync = 1;
				FPString = "VSync";
			} else if(MaxFPS > 190) {
				VSync = 0;
				FPString = "FPS Máximos";
			} else {
				VSync = 0;
				FPString = Mathf.FloorToInt(MaxFPS).ToString();
			}

			GUI.Label(new Rect(MarginW-300, MarginH-287+150, 300, 30), "FPS Maximos: " + FPString+"*");
			
			MaxFPS = GUI.HorizontalSlider(new Rect(MarginW-300, MarginH-287+175, 300, 20), MaxFPS, 9, 200);

			GUI.Label (new Rect(MarginW-250, MarginH-287+75+515-95-25, 500, 20), "*: Para que estos cambian surtan efecto, el juego tiene que ser reiniciado.", warn);
			
			if(GUI.Button(new Rect(MarginW-100, MarginH-287+75+515-95, 200, 50), "Hecho")) {
				ApplyChanges();
				currentMenu = OptionsMenu.StartMenu;
			}

			break;
			
		case OptionsMenu.ControlsMenu:
		
			//Controles

			if (style != "menu") {
				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);
			}

			int ControlsQnt = mInput.InputMap.Count;

			cScroll = GUI.VerticalSlider(new Rect(Screen.width/2-350+20+700-40+2, Screen.height/2-252+20, 10, 525-100), cScroll, 0, 50*ControlsQnt);

			GUI.BeginGroup(new Rect(Screen.width/2-350+20, Screen.height/2-252+20, 700-20, 525-20));

			GUI.Box(new Rect(0, 0, 700-40, 525-100), "");

			GUI.BeginGroup(new Rect(680/2-250, 0, 500, 425));

			GUI.BeginGroup(new Rect(0, -cScroll, 500, 50*ControlsQnt));

			GUIStyle labelCentered = new GUIStyle() {alignment = TextAnchor.MiddleLeft, normal = new GUIStyleState() {textColor = Color.white}, fontSize = 14, fontStyle = FontStyle.Bold};
		
			cScroll -= Input.GetAxis("Mouse ScrollWheel")*25;
			
			if(cScroll < 0) {
				cScroll += 25;
			} else if(cScroll > ControlsQnt * 50) {
				cScroll -= 25;
			}

			float i = 0;

			//This causes a lot of lag...
			foreach(KeyValuePair<string, KeyCode> input in tempInputMap) {
				string strInputValue = (input.Value.ToString() == "None" && !setNewKey) ? "Click me!" : input.Value.ToString();
				GUI.Label(new Rect(10, 50*i+5, 500-30-100-100, 40), input.Key, labelCentered);
				if(GUI.Button(new Rect(500-20-100-100, 50*i+5, 100, 40), strInputValue)) { //Si no hay ninguna tecla nula (no se esta editando ninguna)
					setNewKey = true;
					if(!tempInputMap.ContainsValue(KeyCode.None)) {
						tempInputMap[input.Key] = KeyCode.None;
					}
				} else if(tempInputMap.ContainsValue(KeyCode.None) && Event.current.isKey && setNewKey) { //Si hay teclas para editar y se ha pulsado una, pues cambiar la que esta nula...
					KeyCode keySetted = Event.current.keyCode;
					string[] nullKeys = tempInputMap.Where(x => x.Value == KeyCode.None).Select(pair => pair.Key).ToArray();
					if(tempInputMap.ContainsValue(keySetted)) { //Se hace nulo la key repetida en caso de que haya alguna repeticion
						string[] repeatedKey = tempInputMap.Where(x => x.Value == keySetted && x.Key != nullKeys[0]).Select(pair => pair.Key).ToArray();
						tempInputMap[repeatedKey[0]] = KeyCode.None;
					}
					tempInputMap[nullKeys[0]] = keySetted;
					setNewKey = false;
				}
				if(GUI.Button(new Rect(500-10-100, 50*i+5, 100, 40), "Reset")) {
					mInput.RestoreKey(input.Key);
				}
				i++;
			}

			GUI.EndGroup();

			GUI.EndGroup();

			GUI.EndGroup();
			
			if(GUI.Button(new Rect(Screen.width/2-200-7, MarginH-287+75+515-95, 200, 50), "Hecho")) {
				mInput.InputMap = tempInputMap;
				mInput.StaticInputMap = tempInputMap;
				currentMenu = OptionsMenu.StartMenu;
			}

			if(GUI.Button(new Rect(Screen.width/2+7, MarginH-287+75+515-95, 200, 50), "Reset All Keys")) {
				mInput.RestoreAll();
			}
			break;
			
		case OptionsMenu.DriverMenu:

			if(style != "menu") {
				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);
			}
			
			string SenString = "";
			
			if(Sensitivity == 0.5f) {
				SenString = "*bostezo*";
			} else if(Sensitivity == 30) {
				SenString = "HIPERVELOCIDAD!!!";
			} else {
				SenString = Mathf.FloorToInt(((Sensitivity - 0.5f) / (30 - 0.5f)) * 200).ToString();
			}
			
			GUI.Label(new Rect(MarginW-300, MarginH-287+50, 300, 30), "Sensibilidad: " + SenString);
			
			Sensitivity = GUI.HorizontalSlider(new Rect(MarginW-300, MarginH-287+75, 300, 20), Sensitivity, 0.5f, 30);
			
			if(GUI.Button(new Rect(MarginW-100, MarginH-287+75+515-95, 200, 50), "Hecho")) {
				ApplyChanges();
				currentMenu = OptionsMenu.StartMenu;
			}
		
			break;

		case OptionsMenu.Minimap:

			if (style != "menu") {
				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);
			}
			
			string MinimapStr = "";
			
			if(MinimapZoom >= 30 && MinimapZoom < 100) {
				MinimapStr = "x16";
			} else if(MinimapZoom >= 100 && MinimapZoom < 200) {
				MinimapStr = "x8";
			} else if(MinimapZoom >= 200 && MinimapZoom < 400) {
				MinimapStr = "x4";
			} else if(MinimapZoom >= 400 && MinimapZoom < 600) {
				MinimapStr = "x2";
			} else if(MinimapZoom >= 600 && MinimapZoom < 800) {
				MinimapStr = "x1";
			} else if(MinimapZoom >= 800) {
				MinimapStr = "x0.5";
			}
			
			GUI.Label(new Rect(MarginW-300, MarginH-287+50, 300, 30), "Zoom del Minimapa: " + MinimapStr);
			
			MinimapZoom = GUI.HorizontalSlider(new Rect(MarginW-300, MarginH-287+75, 300, 20), MinimapZoom, 30, 1000);

			if(style != "menu" && GUI.Button(new Rect(MarginW-150, MarginH-287+75+515-95-65, 300, 50), "Editor de Waypoints")) {
				currentMenu = OptionsMenu.WaypointEditor;
			}
			
			if(GUI.Button(new Rect(MarginW-100, MarginH-287+75+515-95, 200, 50), "Hecho")) {
				ApplyChanges();
				currentMenu = OptionsMenu.StartMenu;
			}

			break;

		case OptionsMenu.WaypointEditor:

			if (style != "menu") {

				int waypointCount = 0;

				if(WaypointLib.waypointList != null) {
					waypointCount = WaypointLib.waypointList.Count;
				}

				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);

				cScroll = GUI.VerticalSlider(new Rect(Screen.width/2-350+20+700-40+2, Screen.height/2-252+20, 10, 525-100), cScroll, 0, 50*waypointCount);
				
				GUI.BeginGroup(new Rect(Screen.width/2-350+20, Screen.height/2-252+20, 680, 505));
				
				GUI.Box(new Rect(0, 0, 660, 425), "");
				
				GUI.BeginGroup(new Rect(0, -cScroll, 660, 50*waypointCount));

				labelCentered = new GUIStyle() {alignment = TextAnchor.MiddleLeft, normal = new GUIStyleState() {textColor = Color.white}, fontSize = 14, fontStyle = FontStyle.Bold};

				GUIStyle waypointTextArea = new GUIStyle(GUI.skin.textArea) {normal = new GUIStyleState() {textColor = Color.white}, alignment = TextAnchor.MiddleLeft, fontSize = 16};

				cScroll -= Input.GetAxis("Mouse ScrollWheel")*25;
				
				if(cScroll < 0) {
					cScroll += 25;
				} else if(cScroll > waypointCount * 50) {
					cScroll -= 25;
				}

				for(int j = 0; j < waypointCount; j++) {
					Texture2D wColor = new Texture2D(1,1);
					wColor.SetPixel(1, 1, WaypointLib.waypointList[j].color);
					wColor.Apply();
					Color colorState = Color.red;
					string strState = "Ocultar waypoint";
					if(WaypointLib.waypointList[j].state == WaypointState.Hidden) {
						strState = "Borrar waypoint";
						colorState = Color.red;
					} else if(WaypointLib.waypointList[j].state == WaypointState.Deleted) {
						strState = "Mostrar waypoint";
						colorState = Color.green;
					}
					GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) {normal = new GUIStyleState() {textColor = colorState}};
					GUI.Label(new Rect(10, 50*j+5, 640, 40), (j+1)+") "+WaypointLib.waypointList[j].name, labelCentered);
					float xPos, yPos, zPos;
					float.TryParse(GUI.TextArea(new Rect(290, 50*j+5, 50, 40), WaypointLib.waypointList[j].pos.x.ToString(), waypointTextArea), out xPos);
					float.TryParse(GUI.TextArea(new Rect(345, 50*j+5, 50, 40), WaypointLib.waypointList[j].pos.y.ToString(), waypointTextArea), out yPos);
					float.TryParse(GUI.TextArea(new Rect(400, 50*j+5, 50, 40), WaypointLib.waypointList[j].pos.z.ToString(), waypointTextArea), out zPos);
					WaypointLib.waypointList[j].pos.x = xPos;
					WaypointLib.waypointList[j].pos.y = yPos;
					WaypointLib.waypointList[j].pos.z = zPos;
					GUI.DrawTexture(new Rect(455, 50*j+5, 40, 40), wColor);
					if(GUI.Button(new Rect(455, 50*j+5, 40, 40), "", GUI.skin.label)) {
						selectedIndex = j;
						currentMenu = OptionsMenu.ColorSelector;
					}
					if(GUI.Button(new Rect(500, 50*j+5, 150, 40), strState, buttonStyle)) {
						if(WaypointLib.waypointList[j].state == WaypointState.Hidden) {
							WaypointLib.waypointList[j].state = WaypointState.Deleted;
						} else if(WaypointLib.waypointList[j].state == WaypointState.Deleted) {
							WaypointLib.waypointList[j].state = WaypointState.Shown;
						} else if(WaypointLib.waypointList[j].state == WaypointState.Shown) {
							WaypointLib.waypointList[j].state = WaypointState.Hidden;
						}
					}
				}

				GUI.EndGroup();

				GUI.EndGroup();

				if(GUI.Button(new Rect(MarginW-100, MarginH-287+75+515-95, 200, 50), "Hecho")) {
					GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
					for(int j = 0; j < waypointCount; j++) {
						GameObject goWaypoint = allWaypoints.FirstOrDefault(x => x.GetComponent<WaypointIndexer>().WaypointName == WaypointLib.waypointList[j].name);
						goWaypoint.transform.position = WaypointLib.waypointList[j].pos.GetVector3();
						if(WaypointLib.waypointList[j].state == WaypointState.Deleted) {
							GameObject.Destroy(goWaypoint);
						} else if(WaypointLib.waypointList[j].state == WaypointState.Hidden) {
							goWaypoint.transform.FindChild("WaypointText").GetComponent<WaypointRenderer>().waypointEnabled = false;
						} else if(WaypointLib.waypointList[j].state == WaypointState.Shown) {
							if(!goWaypoint.transform.FindChild("WaypointText").GetComponent<WaypointRenderer>().waypointEnabled) {
								goWaypoint.transform.FindChild("WaypointText").GetComponent<WaypointRenderer>().waypointEnabled = true;
							}
						}
					}
					currentMenu = OptionsMenu.Minimap;
				}

			}

			break;

		case OptionsMenu.ColorSelector:

			GUIStyle waypointLabels = new GUIStyle(GUI.skin.label) {normal = new GUIStyleState() {textColor = Color.white}, alignment = TextAnchor.MiddleCenter, fontSize = 16};
				
			Vector2 pad = new Vector2(Screen.width/2-506/2, -170);

			if(wpColorPreview == null) {
				wpColorPreview = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);
			}
				
			float tempRed = wpRed;
			float tempGreen = wpGreen;
			float tempBlue = wpBlue;

			string wpcolorpstr = "";
				
			if(wpColorPalette) {
				wpcolorpstr = "Activado";
			} else {
				wpcolorpstr = "Desactivado";
			}
				
			if(GUI.Button(new Rect(pad.x+253-150, pad.y+23+10+180+15+10, 300, 50), "Colores avanzados: "+wpcolorpstr)) {
				wpColorPalette = (wpColorPalette == false) ? true : false;
			}
				
			if(!wpColorPalette) {

				GUI.Label(new Rect(pad.x, pad.y+23+10+180+15+5+60, 506, 30), "Color", waypointLabels);

				waypointLabels.alignment = TextAnchor.UpperLeft;
					
				GUI.Label(new Rect(pad.x+10, pad.y+23+10+210+15+5+10+60, 100, 30), "Rojo ("+(wpRed*255).ToString("F0")+")");
				GUI.Label(new Rect(pad.x+10, pad.y+23+10+240+15+10+10+60, 100, 30), "Verde ("+(wpGreen*255).ToString("F0")+")");
				GUI.Label(new Rect(pad.x+10, pad.y+23+10+270+15+15+10+60, 100, 30), "Azul ("+(wpBlue*255).ToString("F0")+")");
					
				waypointLabels.alignment = TextAnchor.MiddleCenter;
					
				wpRed = GUI.HorizontalSlider(new Rect(pad.x+10+100+10, pad.y+23+10+210+15+5+15+60, 246, 30), wpRed, 0, 1);
				wpGreen = GUI.HorizontalSlider(new Rect(pad.x+10+100+10, pad.y+23+10+240+15+10+15+60, 246, 30), wpGreen, 0, 1);
				wpBlue = GUI.HorizontalSlider(new Rect(pad.x+10+100+10, pad.y+23+10+270+15+15+15+60, 246, 30), wpBlue, 0, 1);
					
				finalColor = new Color(wpRed, wpGreen, wpBlue, 1);
					
				if(wpRed != tempRed || wpGreen != tempGreen || wpBlue != tempBlue) {
					wpColorPreview.SetPixel(0, 0, finalColor);
					wpColorPreview.Apply();
				}
					
				GUI.DrawTexture(new Rect(pad.x+506-120-10, pad.y+23+10+210+15+5+10+60, 120, 120), wpColorPreview);

			} else {
					
				GUI.Label(new Rect(pad.x+506-150-10, pad.y+23+10+180+15+5+60, 100, 30), "Color", waypointLabels);
				
				Texture2D palette = Resources.Load<Texture2D>("images/Palette");

				GUI.DrawTexture(new Rect(pad.x+10+50+23, pad.y+23+10+180+15+60+5, 258, 200), palette);

				if(GUI.Button(new Rect(pad.x+10+50+23, pad.y+23+10+180+15+60+5, 258, 200), "", GUI.skin.label)) {
					
					Vector2 mouseFix = Event.current.mousePosition;
					Vector2 finalpos = new Vector2(mouseFix.x-pad.x-60-23, mouseFix.y-pad.y-288-5);
					
					finalColor = palette.GetPixel((int)finalpos.x, palette.height-(int)finalpos.y);
					
					wpColorPreview.SetPixel(0, 0, finalColor);
					wpColorPreview.Apply();
					
				}
				
				GUI.DrawTexture(new Rect(pad.x+506-150-10, pad.y+23+10+180+15+5+60+40, 120, 120), wpColorPreview);
					
			}

			if(GUI.Button(new Rect(MarginW-100-205, MarginH-287+75+515-95, 200, 50), "Hecho")) {
				WaypointLib.waypointList[selectedIndex].color = finalColor;
				currentMenu = OptionsMenu.WaypointEditor;
			}

			if(GUI.Button(new Rect(MarginW-100, MarginH-287+75+515-95, 200, 50), "Cancelar")) {
				currentMenu = OptionsMenu.WaypointEditor;
			}

			if(GUI.Button(new Rect(MarginW-100+205, MarginH-287+75+515-95, 200, 50), "Aleatorio")) {
				System.Random color = new System.Random();
				
				wpRed = (float)color.NextDouble();
				wpGreen = (float)color.NextDouble();
				wpBlue = (float)color.NextDouble();
				
				Color finalColor = new Color(wpRed, wpGreen, wpBlue, 1);
				wpColorPreview.SetPixel(0, 0, finalColor);
				wpColorPreview.Apply();
			}
			
			break;
			
		}
		
		return true;
		
	}
	
	public static void ReadValues() {

		//That means that the player is a ragdoll
		if(GameObject.Find("Player") == null) {
			return;
		}
		
		//if(GameGUI.GUIOptions.GetSensitivity() != 666 && GameGUI.GUIOptions.GetSensitivity() != Sensitivity) {
			GameGUI.GUIOptions.SetSensitivity(InvertMouse, Sensitivity);
		//}

		Camara = GameObject.Find("Player").transform.FindChild("mainCam").GetComponent<Camera>();

		if(Camara.fieldOfView != FOV || Camara.farClipPlane != RenderDistance) {
			Camara.fieldOfView = FOV;
			Camara.farClipPlane = RenderDistance;
		}
		
		RenderSettings.ambientLight = new Color(Brightness, Brightness, Brightness, 1.0f);

		if(QualitySettings.GetQualityLevel() != QualityLvl) {
			QualitySettings.SetQualityLevel(QualityLvl);
		}

	}

}
