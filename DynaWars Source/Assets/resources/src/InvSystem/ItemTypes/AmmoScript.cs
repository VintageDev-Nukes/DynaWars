using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ammo {

	public float ammoboxSize;

	private static bool _setted = false;
	
	public static bool setted {
		get {return _setted;}
		set {_setted = value;}
	}

	/*private static float _ammoboxSize;
	private static bool _setted = false;

	public static float ammoboxSize {
		get {return _ammoboxSize;}
		set {_ammoboxSize = value;}
	}

	public static bool setted {
		get {return _setted;}
		set {_setted = value;}
	}*/

}

public class CurWeapons {

	public static Dictionary<string, Ammo> ammoLib = new Dictionary<string, Ammo>();

	public static void LoadWeapons(string name, float size) {
		Ammo ammoPreset;
		
		ammoPreset = new Ammo();
		ammoPreset.ammoboxSize = size;
		CurWeapons.SetNewWeapon(name, ammoPreset);
	}

	public static void SetNewWeapon(string str, Ammo lib) {
		if(!ammoLib.ContainsKey(str)) {
			ammoLib.Add(str, lib);
		}
	}

	public static Ammo RetrieveInfo(string name) {
		return ammoLib[name];
	}

	public static void ChangeAmmo(string name, float bullets) {

		Ammo tempAmmoPreset = ammoLib[name];
		tempAmmoPreset.ammoboxSize = bullets;

		ammoLib[name] = tempAmmoPreset;

	}

}

public class AmmoScript : MonoBehaviour {

	public float ammo_boxSize = 32;

	// Use this for initialization
	void Start () {
		CurWeapons.LoadWeapons(transform.name, ammo_boxSize);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
