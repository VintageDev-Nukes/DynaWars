using UnityEngine;
using System.Collections;
using System.Linq;

public class GunScript : MonoBehaviour {

	public GunActionTypes actionType;
	public float damage = 10;
	public float criticProbability = 50;
	public float delay_between_shots;
	public float fireSpeed = 25;
	public float miss_radius = 0.05f;
	public string BulletType;
	public float reloadTime = 1;
	public int bulletQnt = 1;
	public float sep_in_meters = 0.1f;
	public AmmoList bulletType;
	public Color LaserColor = Color.red;
	public float LaserStartWidth = 0.01f;
	public float LaserEndWidth = 0.01f;
	public AudioClip shot;
	public AudioClip reload;
	public float muzzleFlashTime = 0.5f;
	public float knockBack = 1;

	bool laser = false;

	AudioSource disparo;

	float ammo_boxSize = 0;

	GameObject bullet;

	GameObject bulletobj;

	int slot;

	//GameObject GUN;

	Ammo curInfo;

	Inv invop = new Inv();

	int objid;

	string stringToPass;

	LineRenderer lineRenderer;

	RaycastHit hit;

	ItemScript item;

	bool captionUnchangable;

	void Start() {
		item = GetComponent<ItemScript>();
		if(GetComponent<LineRenderer>() == null) {
			lineRenderer = gameObject.AddComponent<LineRenderer>();
		} else {
			lineRenderer = GetComponent<LineRenderer>();
		}
	}
	
	void Update() {

		if(!item.isDropped) {

			if(Input.GetMouseButton(1)) {
				GunStats.IsAiming = true;
			}

			if(Input.GetMouseButtonUp(1)) {
				GunStats.IsAiming = false;
			}

			if(Player.ThirdView) {
				if(GunStats.IsAiming) {
					PlayerSystem.SwitchPerson(Player.PlayerObj, true, true);
				}
			}

			if(Time.time-GunStats.lastMF < muzzleFlashTime && !GunStats.MuzzleFlash) {
				SelectedSlot.CarriedObject.transform.FindChild("Muzzle").FindChild("MuzzleFlash").gameObject.SetActive(true);
				GunStats.MuzzleFlash = true;
			} else if(Time.time-GunStats.lastMF > muzzleFlashTime && GunStats.MuzzleFlash) {
				SelectedSlot.CarriedObject.transform.FindChild("Muzzle").FindChild("MuzzleFlash").gameObject.SetActive(false);
			}

			if(transform.FindChild("Laser") != null) {
				laser = true;
			}

			if(Input.GetKeyDown(mInput.GetKey("Laser")) && laser && !PlayerStats.Killed) {
				GunStats.Laser = (GunStats.Laser == false) ? true : false;
			}

			if(Input.GetKeyDown(mInput.GetKey("FlashLight")) && !PlayerStats.Killed) {
				GunStats.Flash = (GunStats.Flash == false) ? true : false;
			}

			objid = transform.GetComponent<ItemScript>().id;

			GunStats.fireSpeed = fireSpeed;

			curInfo = CurWeapons.RetrieveInfo(transform.name);

			//itemName = ItemBase.MainBase.FirstOrDefault(x => x.id == objid).DisplayName;

			if(!captionUnchangable) {
				stringToPass = "("+CurWeapons.RetrieveInfo(transform.name).ammoboxSize+"-"+transform.GetComponent<AmmoScript>().ammo_boxSize+")";
			}

			if(CurWeapons.RetrieveInfo(transform.name).ammoboxSize == transform.GetComponent<AmmoScript>().ammo_boxSize) {
				stringToPass = "[Full]";
				captionUnchangable = false;
			}
		
			if(GunStats.canShot && !PlayerStats.Killed) { 

				if(curInfo.ammoboxSize > 0 && !Menus.pauseMenu) {

					if(!GunStats.shoted) {
						if(actionType == GunActionTypes.Auto) {
							if(Input.GetMouseButton(0)) {
								Shot();
							}
						}
					}

					if(!GunStats.shoted) {
						if(actionType == GunActionTypes.Manual) {
							if(Input.GetMouseButtonDown(0)) {
								Shot();
							}
						}
					}

				} else if(curInfo.ammoboxSize == 0 && !Menus.pauseMenu && !PlayerStats.Killed) {

					if(Input.GetMouseButton(0) || Input.GetKeyDown(mInput.GetKey("Reload"))) {

						bool emptyAmmo = false;

						if(GunStats.firstReload) {
							GunStats.lastReload = Time.time;
							GunStats.firstReload = false;
							//Debug.Log("Last reload set");
						}

						if(Time.time > (GunStats.lastReload + reloadTime)) {
							//Debug.Log("Reloading...");
							GunStats.canReload = true;
							emptyAmmo = Reload(BulletType);
							stringToPass = "(Reloading)";
							captionUnchangable = true;
						} else {
							if(!GunStats.PlayedSound) {
								AudioManager.Play(reload, transform.position);
								GunStats.PlayedSound = true;
								stringToPass = "[Reloaded]";
								captionUnchangable = true;
							}
						}

						if(emptyAmmo && GunStats.canReload) {
							stringToPass = "[Empty]";
							captionUnchangable = true;
						}
					
					}

				}
			}

			invop.ChangeItemCaption(objid, stringToPass);

			if (Time.time > GunStats.nextShot) {
				GunStats.canShot = true;		
			}

			GunStats.shoted = false;

			if(GunStats.Laser && laser && !PlayerStats.Killed) {

				lineRenderer.enabled = true;

				Material laserMat = new Material(Shader.Find("Particles/Additive"));

				lineRenderer.material = laserMat;
				lineRenderer.SetWidth(LaserStartWidth,LaserEndWidth);
				lineRenderer.castShadows = false;
				lineRenderer.receiveShadows = false;

				lineRenderer = GetComponent<LineRenderer>();

				lineRenderer.SetPosition(0, transform.FindChild("Laser").position);

				Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width*0.5f, Screen.height*0.5f, 0));
				float LaserLength = 1000; 

				Vector3 Lpoint = Vector3.zero;

				if (Physics.Raycast (ray, out hit, LaserLength)){
					Lpoint = hit.point;
				} else {
					Lpoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, LaserLength));
				}

				lineRenderer.SetPosition(1, Lpoint);

				float Ldist = Vector3.Distance(transform.FindChild("Laser").position, Lpoint);

				Color newLaserColor = Color.Lerp(LaserColor, ColorExt.InvertColor(LaserColor), Ldist/LaserLength);

				lineRenderer.SetColors(newLaserColor, newLaserColor);	

			}

			if(!GunStats.Laser && lineRenderer != null) {
				lineRenderer.enabled = false;
			}

			transform.FindChild("Flash").GetComponent<Light>().enabled = GunStats.Flash;

			Vector3 point; 
			point = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, Screen.height / 2, 1000));
			
			transform.FindChild("Flash").LookAt(point);

		}
		
	}

	void Shot() {

		if(Time.time-GunStats.lastMF > muzzleFlashTime) {
			GunStats.lastMF = Time.time;
			GunStats.MuzzleFlash = false;
		}

		//Bug !!! [Particles]

		AudioManager.Play(shot, transform.position);
	
		bulletobj = (GameObject)Resources.Load("Models/Items/Bullets/"+GunSystem.GetAmmoNameFromType(bulletType));

		Vector3 eul = transform.eulerAngles;

		Vector3 diff = Vector3.zero;

		for(int i = 0; i < bulletQnt; i++) {

			if(Player.ThirdView) {
				transform.FindChild("Muzzle").localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
			}

			bullet = GameObject.Instantiate(bulletobj, transform.FindChild("Muzzle").position, transform.FindChild("Muzzle").rotation) as GameObject; //+new Vector3(x, -0.1478059f, z) //+new Vector3(x, 0, z) //+new Vector3(0.05442379f, -0.1478059f, -0.4104809f)

			bullet.name = "bullet"+GameObject.FindGameObjectsWithTag("Bullet").Length;

			bullet.GetComponent<BulletScript>().criticProb = criticProbability;

			bullet.GetComponent<BulletScript>().knockBack += knockBack;

			//bullet.GetComponent<Damager>().dmg += Random.Range((int)this.damage-Random.Range(1, 5), (int)this.damage+Random.Range(1, 5)); //damage;

			if(bulletQnt == 1) {

				float theta = 1000;
				float r = miss_radius;
				float x0 = 0; //transform.localPosition.x;
				float z0 = 0; //transform.localPosition.z;
				float x = x0 + r * Mathf.Cos((Time.time * theta) * Mathf.PI / 180);
				float z = z0 + r * Mathf.Sin((Time.time * theta) * Mathf.PI / 180);
				diff = new Vector3(x, 0, z);

			} else {

				diff = new Vector3((i-Mathf.FloorToInt(bulletQnt/2))*sep_in_meters, 0, 0);

			}


			if(!Player.ThirdView) {

				//Bug !!!

				RaycastHit hit;
				Vector3 point = Vector3.zero; 

				if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit)) { //+ diff
					point = hit.point;
				}
				else {
					point = Camera.main.transform.position + (Camera.main.transform.forward) * 1000; //+ diff
				}

				bullet.transform.LookAt(point);

			} else {
				if(GunStats.IsAiming) {

					Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width*0.5f, Screen.height*0.5f, 0));
					float AimLength = 1000; 
					Vector3 point = Vector3.zero;

					if (Physics.Raycast (ray, out hit, AimLength, 8)){
						point = hit.point;
					} else {
						point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, AimLength));
					}

					bullet.transform.LookAt(point);

				}
			}

		}

		GunStats.canShot = false;
		GunStats.nextShot = Time.time + delay_between_shots;

		CurWeapons.ChangeAmmo(transform.name, curInfo.ammoboxSize - 1);

		stringToPass = "("+CurWeapons.RetrieveInfo(transform.name).ammoboxSize+"-"+transform.GetComponent<AmmoScript>().ammo_boxSize+")";

		if(captionUnchangable) {
			captionUnchangable = false;
		}

		GunStats.shoted = true;

	}

	bool Reload(string bullettype) {

		GunStats.canShot = false;

		bool antiBug = false;
		int tempSize = 0;

		if(GunStats.canReload) {

			//GUN = SelectedSlot.CarriedObject;
			
			ammo_boxSize = transform.GetComponent<AmmoScript>().ammo_boxSize;

			//First, check if is there are some ammo in the inventory

			if(Slots.HotBarSlots != null) {
				for (int h = 0; h < LvlSys.HTS; h++) {
					if(Slots.HotBarSlots[h] != null) {
						if(Slots.HotBarSlots[h].DisplayName == bullettype) {
							if(Slots.HotBarSlots[h].itemstacksize > 1) {
								if(tempSize < Slots.InventorySlots[slot].itemstacksize) {
									Slots.InventorySlots[h].itemstacksize -= 1;
								}
								if(!antiBug) {
									tempSize = Slots.InventorySlots[h].itemstacksize;
									antiBug = true;
								}
							} else {
								Slots.HotBarSlots[h] = null;
							}
							CurWeapons.ChangeAmmo(transform.name, ammo_boxSize);
							break;
						}
					}
				}
			}

			if(Slots.AmmoSlots != null) {
				for (int am = 0; am < LvlSys.Ammo; am++) {
					if(Slots.AmmoSlots[am] != null) {
						if(Slots.AmmoSlots[am].DisplayName == bullettype) {
							if(Slots.AmmoSlots[am].itemstacksize > 1) {
								if(tempSize < Slots.InventorySlots[slot].itemstacksize) {
									Slots.InventorySlots[am].itemstacksize -= 1;
								}
								if(!antiBug) {
									tempSize = Slots.InventorySlots[am].itemstacksize;
									antiBug = true;
								}
							} else {
								Slots.AmmoSlots[am] = null;
							}
							CurWeapons.ChangeAmmo(transform.name, ammo_boxSize);
							break;
						}
					}
				}
			}

			if(Slots.InventorySlots != null) {
				slot = 0;

				for (int iy = 0; iy < LvlSys.IIY; iy++) {
					for (int ix = 0; ix < LvlSys.IIX; ix++) {
						if(Slots.InventorySlots[slot] != null) {
							if(Slots.InventorySlots[slot].DisplayName == bullettype) {
								if(Slots.InventorySlots[slot].itemstacksize > 1) {
									if(tempSize < Slots.InventorySlots[slot].itemstacksize) {
										Slots.InventorySlots[slot].itemstacksize -= 1;
									}
									if(!antiBug) {
										tempSize = Slots.InventorySlots[slot].itemstacksize;
										antiBug = true;
									}
								} else {
									Slots.InventorySlots[slot] = null;
								}
								CurWeapons.ChangeAmmo(transform.name, ammo_boxSize);
								break;
							}
						}
						slot++;
					}
				}
			}
		
			GunStats.canReload = false;
			GunStats.canShot = true;
			GunStats.firstReload = true;

		}

		return true;

	}

	IEnumerator particles() {
		float t = 0;
		while(true) {
			Debug.Log("Test");
			SelectedSlot.CarriedObject.transform.FindChild("Muzzle").FindChild("MuzzleFlash").gameObject.SetActive(true);
			GunStats.MuzzleFlash = true;
			t += Time.deltaTime;
			
			if(t > 1) {
				break;
			}
			
			yield return null;
		}
		GunStats.MuzzleFlash = false;
	}

}
