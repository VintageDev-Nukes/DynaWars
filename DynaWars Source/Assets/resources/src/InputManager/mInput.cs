using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class mInput {

	//This causes a lot of lag... (30 FPS reduction) [He arreglado esto, haciendo que las keys solo se puedan obtener de un Diccionario que se ha cargado, y no de algo dinamico que tarda mucho mas en cargar]
	//[Por igual tengo que hacer que los controles cuando se dibujen se dibujen de una forma mas express ya que dan bastante lag al abrir el menu]
	//[Si no afecta demasiado al juego in-game no pasa nada, pero aun asi prefiero optimizarlo]

	private static Dictionary<string, KeyCode> inputMap;

	private static Dictionary<string, KeyCode> defInputMap;

	private static string[] dftKMStrArray;

	private static Dictionary<string, KeyCode> _sInput = new Dictionary<string, KeyCode>();

	public static Dictionary<string, KeyCode> StaticInputMap {
		get {
			if(_sInput != null) {
				return _sInput;
			} else {
				return InputMap;
			}
		}
		set {
			_sInput = value;
		}
	}

	public static Dictionary<string, KeyCode> InputMap {
		/*get {
			Dictionary<string, KeyCode> tempInputMap = inputMap;
			if (inputMap == null)
			{
				Dictionary<string, KeyCode> tmp = LoadAllKeysFromConfig();
				if(tmp == null) {
					tempInputMap = LoadAllDefaultKeys();
				} else {
					return tmp;
				}
			}
			return tempInputMap;
		}*/
		get {
			if (inputMap == null)
			{
				var tmp = LoadAllKeysFromConfig();
				if(tmp == null) {
					return LoadAllDefaultKeys();
				}
				return tmp;
			}
			return inputMap;
		}
		set {
			inputMap = value;
		}
	}

	public static Dictionary<string, KeyCode> DefaultInputMap {
		get {
			//Dictionary<string, KeyCode> returnvalue = LoadAllDefaultKeys();
			if(defInputMap == null) {
				return LoadAllDefaultKeys();
			}
			return defInputMap;
		}
		set {
			defInputMap = value;
		}
	}

	public static string[] DefaultKeysActions {
		get {

			List<string> tempdefstrarray = new List<string>();
			
			foreach(KeyValuePair<string, KeyCode> kvpDefMap in DefaultInputMap) {
				tempdefstrarray.Add(kvpDefMap.Key);
			}

			return tempdefstrarray.ToArray();

		}
	}

	public static Dictionary<string, KeyCode> LoadAllKeysFromConfig() {

		//Set here all the values "Fire1", "ScreenShot" from config file

		//Test value
		//inputMap.Add("Fire1", GetKeyFromString("Mouse0")); //Charges those values with a for cicle extracted from a file

		/*if(!INI_Manager.Group_Exists(Application.dataPath + "/Profiles/"+Profile.LoadedProfile+"/profile.cfg", "Keys")) {
			//string nl = System.Environment.NewLine;
			//string InitialLoadAllKeysFromConfig = "Exp=L"+nl+"Kill=K"+nl+"FPS=F10"+nl+"EnableGUI=F1"+nl+"GameMenu=Escape"+nl+"Inv=E"+nl+"Waypoint=P"+nl+"ToggleMap=M"+nl+"SwitchView=V"+nl+"TakeScreenShot=F2"+nl+"Laser=L"+nl+"FlashLight=F"+nl+"Reload=R"+nl+"Console=F12"+nl+"SendCommand=Return"+nl+"Unlock=U";
			INI_Manager.Create_Group(Application.dataPath + "/Profiles/"+Profile.LoadedProfile+"/profile.cfg", "Keys", DefaultInputMap.ToDictionary(item => item.Key, item => item.Value.ToString()));
		}*/

		//string GetKeys = INI_Manager.Read_Group("Keys", Application.dataPath + "/appConfig.cfg");
		Dictionary<string, string> tempInputMapRaw = DataItem<string, string>.ToDictionary(Profile.currentProfile.Keys);
		Dictionary<string, KeyCode> tempInputMap = new Dictionary<string, KeyCode>();
		List<string> tmpImportedKeys = new List<string>();
		foreach(KeyValuePair<string, string> kvpInputRaw in tempInputMapRaw) {
			tempInputMap.Add(kvpInputRaw.Key, GetKeyFromString(kvpInputRaw.Value));
			tmpImportedKeys.Add(kvpInputRaw.Key);
		}

		if(tmpImportedKeys.ToArray().Length != DefaultKeysActions.Length) {
			//Check if some index is missing
			string[] missingIndexes = tmpImportedKeys.ToArray().Except(DefaultKeysActions).ToArray();

			foreach(string missingIndex in missingIndexes) {
				tempInputMap.Add(missingIndex, GetKeyFromDefLib(missingIndex));
			}
		}

		return tempInputMap;

	}

	public static Dictionary<string, KeyCode> LoadAllDefaultKeys() {
		
		defInputMap = new Dictionary<string, KeyCode>();

		defInputMap.Add("Exp", GetKeyFromString("L"));
		
		defInputMap.Add("Kill", GetKeyFromString("K"));
		
		defInputMap.Add("FPS", GetKeyFromString("F10"));
		
		defInputMap.Add("EnableGUI", GetKeyFromString("F1"));
		
		defInputMap.Add("GameMenu", GetKeyFromString("Escape"));
		
		defInputMap.Add("Inv", GetKeyFromString("E"));
		
		defInputMap.Add("Waypoint", GetKeyFromString("P"));
		
		defInputMap.Add("ToggleMap", GetKeyFromString("M"));
		
		defInputMap.Add("SwitchView", GetKeyFromString("V"));
		
		defInputMap.Add("TakeScreenShot", GetKeyFromString("F2"));
		
		defInputMap.Add("Laser", GetKeyFromString("L"));
		
		defInputMap.Add("FlashLight", GetKeyFromString("F"));
		
		defInputMap.Add("Reload", GetKeyFromString("R"));
		
		defInputMap.Add("Console", GetKeyFromString("F12"));
		
		defInputMap.Add("SendCommand", GetKeyFromString("Return"));
		
		defInputMap.Add("Unlock", GetKeyFromString("U"));

		return defInputMap;
		
	}

	public static void SaveKeys() {
		Dictionary<string, string> convertedKeys = InputMap.ToDictionary(x => x.Key, x => x.Value.ToString());
		//INI_Manager.Edit_Group(Application.dataPath + "/Profiles/"+Profile.LoadedProfile+"/profile.cfg", "Keys", convertedKeys);
		var uniqueValues = convertedKeys.GroupBy(pair => pair.Value)
							.Select(group => group.First())
							.ToDictionary(pair => pair.Key, pair => pair.Value);
		Profile.currentProfile.Keys = DataItem<string, string>.ToList(uniqueValues);
	}

	public static KeyCode GetKeyFromDefLib(string actionName) {
		//KeyCode returnvalue = 
		return DefaultInputMap[actionName];
		/*if(returnvalue == null) {
			Debug.LogError("The action "+actionName+" was not found.");
			return KeyCode.None;
		}
		return returnvalue;*/
	}

	public static void RestoreAll() {
		InputMap = DefaultInputMap;
	}

	public static void RestoreKey(string actionName) {
		InputMap[actionName] = DefaultInputMap[actionName];
	}

	public static bool IsButtonPressed(string actionName) {
		return Input.GetKey(InputMap[actionName]);
	}

	public static bool IsButtonPressedDown(string actionName) {
		return Input.GetKeyDown(InputMap[actionName]);
	}

	public static bool IsButtonPressedUp(string actionName) {
		return Input.GetKeyUp(InputMap[actionName]);
	}

	public static KeyCode GetKeyFromString(string KeyStr, bool IgnoreCase = true) {
		return (KeyCode)System.Enum.Parse(typeof(KeyCode), KeyStr, IgnoreCase);
	}

	public static KeyCode GetKey(string action) {
		/*KeyCode retKey = KeyCode.None;
		InputMap.TryGetValue(action, out retKey);*/
		/*if(INI_Manager.Group_Exists(Application.dataPath + "/Profiles/"+Profile.LoadedProfile+"/profile.cfg", "Keys")) {
			//string GetKeys = INI_Manager.Read_Group("Keys", Application.dataPath + "/appConfig.cfg");
			Dictionary<string, string> tempInputMapRaw = Profile.Keys; //INI_Manager.Extract_Keys("", GetKeys);
			return (KeyCode)System.Enum.Parse(typeof(KeyCode), tempInputMapRaw[action]);
		}
		return KeyCode.None;*/
		try {
			return StaticInputMap[action];
		} catch(System.Exception ex) {
			Debug.LogError(ex.Message + "; Action: " + action);
			return KeyCode.None;
		}
	}

}
