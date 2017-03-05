using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Prof {
	public string lastLoadedProfile = "";
}

public class SProf {
	public string lastPlayedWorld = "";
	public int QualityLvl = 3;
	public float FOV = 60;
	public float Sensitivity = 15;
	public float Brightness = 0.3f;
	public float RenderDistance = 1500;
	public float MaxFPS = -1;
	public bool InvertMouse;
	public float MinimapZoom = 100;
	public int VSync = 1;
	public List<DataItem<string, string>> Keys = DataItem<string, string>.ToList(mInput.DefaultInputMap.ToDictionary(item => item.Key, item => item.Value.ToString()));
}

public class Profile {

	public static string LoadedProfile = "test";

	public static SProf currentProfile;

	public static Prof staticProfile;

	public static void Load() {
		if(System.IO.File.Exists(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.xml")) {
			currentProfile = XMLTools.DeserializeFromFile<SProf>(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.xml");
		} else {
			currentProfile = new SProf();
			throw new System.Exception("Profile '"+LoadedProfile+"' doesn't exist");
		}
	}

	public static void Save() {
		SetCurrentProfile();
		XMLTools.SerializeToFile<SProf>(currentProfile, Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.xml", true);
	}

	public static void SetCurrentProfile() {
		SProf tempProfile = currentProfile;
		currentProfile = new SProf() {
			lastPlayedWorld = currentProfile.lastPlayedWorld,
			QualityLvl = Options.QualityLvl,
			FOV = Options.FOV,
			Sensitivity = Options.Sensitivity,
			Brightness = Options.Brightness,
			RenderDistance = Options.RenderDistance,
			MaxFPS = Options.MaxFPS,
			InvertMouse = Options.InvertMouse,
			MinimapZoom = Options.MinimapZoom,
			VSync = Options.VSync,
			Keys = tempProfile.Keys //We have to get it from there, because we save the keys use minput.savekeys()
		};
	}

	//This is for appConfig.xml
	public static void StaticLoad() {
		if(System.IO.File.Exists(Application.dataPath + "/appConfig.xml"))
			staticProfile = XMLTools.DeserializeFromFile<Prof>(Application.dataPath + "/appConfig.xml");
		else
			staticProfile = new Prof();
			throw new System.Exception("Profile doesn't exist");
	}

	public static void StaticSave() {
		XMLTools.SerializeToFile<Prof>(staticProfile, Application.dataPath + "/appConfig.xml", true);
	}

	/*public static void SetProperty(ProfileProperty pProp, object value) {
		SProf sProfile = new SProf();
		if(System.IO.File.Exists(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.xml"))
			sProfile = XMLTools.DeserializeFromFile<SProf>(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.xml");
		switch(pProp) {
		case ProfileProperty.Brightness:
			sProfile.Brightness = (float)value;
			break;
		case ProfileProperty.FOV:
			sProfile.FOV = (float)value;
			break;
		case ProfileProperty.InvertMouse:
			sProfile.InvertMouse = (bool)value;
			break;
		case ProfileProperty.Keys:
			sProfile.Keys = (List<DataItem<string, string>>)value;
			break;
		case ProfileProperty.LastPlayedWorld:
			sProfile.lastPlayedWorld = (string)value;
			break;
		case ProfileProperty.MaxFPS:
			sProfile.MaxFPS = (float)value;
			break;
		case ProfileProperty.MinimapZoom:
			sProfile.MinimapZoom = (float)value;
			break;
		case ProfileProperty.QualityLvl:
			sProfile.QualityLvl = (int)value;
			break;
		case ProfileProperty.RenderDistance:
			sProfile.RenderDistance = (float)value;
			break;
		case ProfileProperty.Sensitivity:
			sProfile.Sensitivity = (float)value;
			break;
		case ProfileProperty.VSync:
			sProfile.VSync = (int)value;
			break;
		}
		XMLTools.SerializeToFile<SProf>(sProfile, Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.xml", true);
	}*/

	public static void CreateDefaultProfile(string profileName) {
		XMLTools.SerializeToFile<SProf>(new SProf(), Application.dataPath + "/Profiles/"+profileName+"/profile.xml", true);
	}

	//VARIABLES LOADED FROM FILE .CFG.......

	/*public static string lastPlayedWorld {
		get {
			try {
				return INI_Manager.Load_Value("lastWorldPlayed", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg");
			} catch {
				return null;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "lastWorldPlayed", value);
		}
	}

	public static int QualityLvl {
		get {
			try {
				return int.Parse(INI_Manager.Load_Value("QualityLvl", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return 3;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "QualityLvl", value.ToString());
		}
	}

	public static float FOV {
		get {
			try {
				return float.Parse(INI_Manager.Load_Value("FOV", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return 60;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "FOV", value.ToString("F5"));
		}
	}

	public static float Sensitivity {
		get {
			try {
				return float.Parse(INI_Manager.Load_Value("Sensitivity", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return 15;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "Sensitivity", value.ToString("F5"));
		}
	}

	public static float Brightness {
		get {
			try {
				return float.Parse(INI_Manager.Load_Value("Brightness", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return 0.3f;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "Brightness", value.ToString("F5"));
		}
	}

	public static float RenderDistance {
		get {
			try {
				return float.Parse(INI_Manager.Load_Value("RenderDistance", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return 1500;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "RenderDistance", value.ToString("F5"));
		}
	}

	public static float MaxFPS {
		get {
			try {
				return float.Parse(INI_Manager.Load_Value("MaxFPS", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return -1;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "MaxFPS", value.ToString("F5"));
		}
	}

	public static bool InvertMouse {
		get {
			try {
				return bool.Parse(INI_Manager.Load_Value("InvertMouse", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return false;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "InvertMouse", value.ToString());
		}
	}

	public static float MinimapZoom {
		get {
			try {
				return float.Parse(INI_Manager.Load_Value("MinimapZoom", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return 100;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "MinimapZoom", value.ToString("F5"));
		}
	}

	public static int VSync {
		get {
			try {
				return int.Parse(INI_Manager.Load_Value("VSync", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return 1;
			}
		}
		set {
			INI_Manager.Set_Value(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "VSync", value.ToString());
		}
	}

	public static Dictionary<string, string> Keys {
		get {
			try {
				return INI_Manager.Extract_Keys("", INI_Manager.Read_Group("Keys", Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg"));
			} catch {
				return mInput.DefaultInputMap.ToDictionary(item => item.Key, item => item.Value.ToString());
			}
		}
		set {
			INI_Manager.Edit_Group(Application.dataPath + "/Profiles/"+LoadedProfile+"/profile.cfg", "Keys", value);
		}
	}*/

}
