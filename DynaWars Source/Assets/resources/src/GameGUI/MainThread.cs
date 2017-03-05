using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainThread : MonoBehaviour {

	void Awake() {

		#if UNITY_EDITOR
		if(!System.IO.File.Exists(Application.dataPath + "/Profiles/"+Profile.LoadedProfile+"/profile.xml")) {
			Profile.CreateDefaultProfile(Profile.LoadedProfile);
		}
		Profile.Load();
		#endif
		
		//Set all the values at start
		Options.SetValues(true);
		
		QualitySettings.vSyncCount = Options.VSync;
		Application.targetFrameRate = (int)Options.MaxFPS;
		
		//This avoids bunches of lagg...
		
		Dictionary<string, string> tmp = DataItem<string, string>.ToDictionary(Profile.currentProfile.Keys);
		Dictionary<string, KeyCode> parsed = new Dictionary<string, KeyCode>();
		
		foreach(KeyValuePair<string, string> keyC in tmp) {
			parsed.Add(keyC.Key, mInput.GetKeyFromString(keyC.Value));
		}
		
		mInput.StaticInputMap = parsed;
		
		GameSaver.Load(Profile.currentProfile.lastPlayedWorld);

	}

	void Start() {
		/*foreach(var item in System.Enum.GetValues(typeof(ItemList))) {
			Debug.Log((int)item+", "+((ItemList)item)+System.Environment.NewLine);
		}*/
	}

	void Update() {
		Options.ReadValues();
		if(Player.PlayerObj.transform.position.y <= -100 && !PlayerStats.Killed) {
			Player.PlayerObj.GetComponent<DamageSystem>().MakeDamage(Random.Range(5, 15));
		}
	}

	void LateUpdate() {
		PlayerStats.Killed = PlayerStats.Health <= 0;
	}


}
