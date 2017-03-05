using UnityEngine;
using System.Collections;

public enum GunActionTypes {Manual, Auto}
public enum AmmoList {Bullet, Musket_Ball}

public class GunStats 
{
	
	private static float _nextShot;
	private static bool _canShot;
	private static float _fireSpeed;
	private static float _lastReload;
	private static bool _canReload;
	private static bool _firstReload = true;
	private static bool _shoted;
	private static bool _laser;
	private static bool _flash;
	private static bool _playedSound;
	private static bool _isAiming;
	private static float _lastMF;
	private static bool _muzzleFlash;
	private static bool _isSafety;
	
	public static float nextShot {
		get {return _nextShot;}
		set {_nextShot = value;}
	}
	
	public static bool canShot {
		get {return _canShot;}
		set {_canShot = value;}
	}
	
	public static float fireSpeed {
		get {return _fireSpeed;}
		set {_fireSpeed = value;}
	}
	
	public static float lastReload {
		get {return _lastReload;}
		set {_lastReload = value;}
	}
	
	public static bool canReload {
		get {return _canReload;}
		set {_canReload = value;}
	}
	
	public static bool firstReload {
		get {return _firstReload;}
		set {_firstReload = value;}
	}
	
	public static bool shoted {
		get {return _shoted;}
		set {_shoted = value;}
	}
	
	public static bool Laser {
		get {return _laser;}
		set {_laser = value;}
	}
	
	public static bool Flash {
		get {return _flash;}
		set {_flash = value;}
	}
	
	public static bool PlayedSound {
		get {return _playedSound;}
		set {_playedSound = value;}
	}
	
	public static bool IsAiming {
		get {return _isAiming;}
		set {_isAiming = value;}
	}
	
	public static float lastMF {
		get {return _lastMF;}
		set {_lastMF = value;}
	}
	
	public static bool MuzzleFlash {
		get {return _muzzleFlash;}
		set {_muzzleFlash = value;}
	}
	
	public static bool isSafety {
		get {return _isSafety;}
		set {_isSafety = value;}
	}
	
}

public class GunSystem {

#pragma warning disable
	public static string GetAmmoNameFromType(AmmoList ammo) {
		switch(ammo) {
		case AmmoList.Bullet:
			return "bala";
			break;
		case AmmoList.Musket_Ball:
			return "plomo";
			break;
		default:
			return "";
			break;
		}
	}
#pragma warning restore

}