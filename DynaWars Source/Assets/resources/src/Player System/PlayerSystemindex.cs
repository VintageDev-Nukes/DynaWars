using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerSystemindex : MonoBehaviour {

	PlayerSystem ps;
	
	public GameObject player, wCamera;

	public Camera cam;

	public Transform thirdPerson;

	private Animation anim;

	//string lvlName;

	//regular speed, crouching speed, run speed
	public float walkSpeed = 8.5f, crouchSpeed = 4.5f,  runSpeed = 12.5f; // run speed
	
	private CharacterMotor chMotor;
	//private float dist; // distance to ground

	private Transform FPC, TPC;

	public float waterLevel;

	public GameObject cCam;

	//handItem = 3ª persona, holdedItem = 1ª persona
	public GameObject handItem, holdedItem, tempItem;

	private float tempselectedSlot;
	private int selectedSlot;

	private Vector3 cPos, cRotation, cScale;

	private Texture2D cTex;
	private string cName;

	private Texture2D fistImage;

	private bool bigMap, EnableGUI = true, nulltex = true;

	[HideInInspector]
	public bool crouching, walking, running, jumping, idling, swimming;

	//Player coordinates
	public Vector3 playerpos, vVelocity, lastPosition, curPosition;
	public float velocity;

	//Hotbar slot actions
	
	private InventoryItem[] tempInv;
	private int tempSelected;
	private bool alphaSelected;

	// Use this for initialization
	void Start () 
	{

		Physics.IgnoreLayerCollision(8, 9);
		Physics.IgnoreLayerCollision(8, 10);
		Physics.IgnoreLayerCollision(8, 11);

		Player.PlayerObj = player;
		Player.TPlayerObject = player;
		Player.ThirdView = false;
		PlayerStats.Killed = false;

		fistImage = (Texture2D)Resources.Load("images/fist");

		// Bug !!!
		PlayerSystem.SwitchPerson(player, Player.ThirdView);

		if(Player.ThirdView) {
			foreach(Transform child in handItem.transform) {
				Destroy((child as Transform).gameObject);
			}
		}

		ps = new PlayerSystem();

		chMotor = player.GetComponent<CharacterMotor>();
		//CharacterController ch = player.GetComponent<CharacterController>();
		//dist = ch.height/2; // calculate distance to ground

		anim = thirdPerson.animation;

		//lvlName = Profile.currentProfile.lastPlayedWorld; //INI_Manager.Load_Value("lastWorldPlayed", Application.dataPath + "/appConfig.cfg");

	}

	void OnGUI() {

		if (EnableGUI && !bigMap) {

			GUIStyle itemHUDStyle = new GUIStyle();
			
			itemHUDStyle.alignment = TextAnchor.MiddleCenter;
			itemHUDStyle.fontSize = 12;
			itemHUDStyle.fontStyle = FontStyle.Bold;

			if(!nulltex) {

				GUI.DrawTexture (new Rect (30, 85, 96, 96), cTex);
				GUI.Label(new Rect(16, 176, 120, 46), cName, itemHUDStyle);

			} else {

				GUI.DrawTexture (new Rect (30, 85, 96, 96), fistImage);
				GUI.Label(new Rect(16, 176, 120, 46), "Arma primaria", itemHUDStyle);

			}

		}

	}

	void FixedUpdate() {

		curPosition = player.transform.position;
		vVelocity = (lastPosition - curPosition) / Time.deltaTime;
		velocity = (lastPosition - curPosition).magnitude / Time.deltaTime;
		lastPosition = player.transform.position;
		PlayerStats.Velocity = velocity;

		float curVelocity = Player.PlayerObj.GetComponent<CharacterController>().velocity.magnitude;
		float customVelocityConst = 1;

		if(Player.PlayerObj.GetComponent<CharacterController>().isGrounded) {
			//Si el jugador no esta en el suelo, velocity sera igual al la velocidad en el ultimo momento de no estar en el suelo
			//Multiplicar curVelocity por la dificultad
			customVelocityConst = curVelocity / 1.25f;
		}

		bool itemChanged = false;

		if(Input.GetKeyDown(KeyCode.Alpha0)) {
			selectedSlot = 9;
			alphaSelected = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha1)) {
			selectedSlot = 0;
			alphaSelected = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			selectedSlot = 1;
			alphaSelected = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha3)) {
			selectedSlot = 2;
			alphaSelected = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha4)) {
			selectedSlot = 3;
			alphaSelected = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha5)) {
			selectedSlot = 4;
			alphaSelected = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha6)) {
			selectedSlot = 5;
			alphaSelected = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha7)) {
			selectedSlot = 6;
			alphaSelected = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha8)) {
			selectedSlot = 7;
			alphaSelected = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha9)) {
			selectedSlot = 8;
			alphaSelected = true;
		}
		
		if(Slots.HotBarSlots != null) {
			
			if(!Menus.pauseMenu) {

				if(Slots.HotBarSlots.Length != 0) {

					tempselectedSlot -= Input.GetAxis("Mouse ScrollWheel")/2;

					if(Input.GetAxis("Mouse ScrollWheel") != 0) {
						alphaSelected = false;
					}
					
					if(!alphaSelected) {
						selectedSlot = (int)tempselectedSlot;
					}

					if(selectedSlot > Slots.HotBarSlots.Length - 1) {
						tempselectedSlot -= Slots.HotBarSlots.Length - 1;
					} else if(selectedSlot < 0) {
						tempselectedSlot += Slots.HotBarSlots.Length - 1;
					}

				}
				
			}

			//Debug.Log(selectedSlot);

			if(Slots.HotBarSlots.Length > selectedSlot && Slots.HotBarSlots[selectedSlot] != null) {
				
				if(Slots.HotBarSlots[selectedSlot].itemname != SelectedSlot.cName || selectedSlot != SelectedSlot.lastSlot || SelectedSlot.switchedPer) {
					itemChanged = true;
					SelectedSlot.switchedPer = false;
				}
				
				SelectedSlot.cName = Slots.HotBarSlots[selectedSlot].itemname;
				SelectedSlot.lastSlot = selectedSlot;

				cTex = Slots.HotBarSlots[selectedSlot].itemtex as Texture2D;
				cName = Slots.HotBarSlots[selectedSlot].DisplayName + " " + Slots.HotBarSlots[selectedSlot].strGUI;
				
				nulltex = false;

				if(itemChanged) {
				
					if(Player.ThirdView) {
						
						tempItem = (GameObject)Instantiate(handItem);
						tempItem.transform.parent = handItem.transform.parent;
						tempItem.transform.localPosition = handItem.transform.localPosition;
						Destroy(handItem);
						
						#pragma warning disable
						if (Slots.HotBarSlots[selectedSlot].TcustomPostion == null) {
							cPos = tempItem.transform.position;		
						} else {
							cPos = Slots.HotBarSlots[selectedSlot].TcustomPostion;
						}
						
						if (Slots.HotBarSlots[selectedSlot].TcustomRotation == null) {
							cRotation = tempItem.transform.localEulerAngles;
						} else {
							cRotation = Slots.HotBarSlots[selectedSlot].TcustomRotation;
						}
						
						if (Slots.HotBarSlots[selectedSlot].TcustomScale == null) {
							cScale = tempItem.transform.localScale;		
						} else {
							cScale = Slots.HotBarSlots[selectedSlot].TcustomScale;
						}
						#pragma warning restore
						
						/*if(Slots.HotBarSlots[selectedSlot].isDefault) {
							handItem = Instantiate(Slots.HotBarSlots[selectedSlot].worldObject) as GameObject;
						} else {
							handItem = Instantiate(Slots.HotBarSlots[selectedSlot].dropObject) as GameObject;
						}*/

						handItem = Instantiate(Slots.HotBarSlots[selectedSlot].worldObject) as GameObject;


						handItem.name = Slots.HotBarSlots[selectedSlot].itemname + "_" + selectedSlot;
						handItem.transform.name = Slots.HotBarSlots[selectedSlot].itemname + "_" + selectedSlot;
						handItem.transform.parent = tempItem.transform.parent;
						
						handItem.transform.localPosition = cPos;
						handItem.transform.localEulerAngles = cRotation;
						handItem.transform.localScale = cScale;
						
						handItem.layer = 10;
						
						foreach(Transform child in handItem.transform) {
							child.gameObject.layer = 10;
							foreach(Transform child2 in child) {
								child2.gameObject.layer = 10;
								foreach(Transform child3 in child2) {
									child3.gameObject.layer = 10;
									foreach(Transform child4 in child3) {
										child4.gameObject.layer = 10;
										foreach(Transform child5 in child4) {
											child5.gameObject.layer = 10;
										}
									}
								}
							}
						}
						
						handItem.GetComponent<BoxCollider>().isTrigger = true;
						handItem.rigidbody.isKinematic = true;
						
						Destroy(tempItem);
					
					} else {
						
						tempItem = (GameObject)Instantiate(holdedItem);
						tempItem.transform.parent = holdedItem.transform.parent;
						tempItem.transform.localPosition = holdedItem.transform.localPosition;
						Destroy(holdedItem);
						
						cPos = tempItem.transform.localPosition;
						
						#pragma warning disable
						if (Slots.HotBarSlots[selectedSlot].FcustomRotation == null) {
							cRotation = tempItem.transform.localEulerAngles;
						} else {
							cRotation = Slots.HotBarSlots[selectedSlot].FcustomRotation;
						}
						
						if (Slots.HotBarSlots[selectedSlot].FcustomScale == null) {
							cScale = tempItem.transform.localScale;		
						} else {
							cScale = Slots.HotBarSlots[selectedSlot].FcustomScale;
						}
						
						/*if(Slots.HotBarSlots[selectedSlot].isDefault) {
							holdedItem = Instantiate(Slots.HotBarSlots[selectedSlot].worldObject) as GameObject;
						} else {
							holdedItem = Instantiate(Slots.HotBarSlots[selectedSlot].dropObject) as GameObject;
						}*/

						holdedItem = Instantiate(Slots.HotBarSlots[selectedSlot].worldObject) as GameObject;

						#pragma warning restore

						holdedItem.name = Slots.HotBarSlots[selectedSlot].itemname + "_" + selectedSlot;
						holdedItem.transform.name = Slots.HotBarSlots[selectedSlot].itemname + "_" + selectedSlot;

						holdedItem.transform.parent = tempItem.transform.parent;
						
						holdedItem.transform.localPosition = cPos;
						holdedItem.transform.localEulerAngles = cRotation;
						holdedItem.transform.localScale = cScale;
						
						holdedItem.rigidbody.useGravity = false;
						holdedItem.rigidbody.detectCollisions = false;
						holdedItem.layer = 9;
						
						foreach(Transform child in holdedItem.transform) {
							child.gameObject.layer = 9;
							foreach(Transform child2 in child) {
								child2.gameObject.layer = 9;
								foreach(Transform child3 in child2) {
									child3.gameObject.layer = 9;
									foreach(Transform child4 in child3) {
										child4.gameObject.layer = 9;
										foreach(Transform child5 in child4) {
											child5.gameObject.layer = 9;
										}
									}
								}
							}
						}
						
						holdedItem.GetComponent<BoxCollider>().isTrigger = true;
						holdedItem.rigidbody.isKinematic = true;
						
						Destroy(tempItem);
						
						SelectedSlot.CarriedObject = holdedItem;
					
					}
			
				}
				
			} else {
				nulltex = true;
			}

		}
		
		Player.PlayerObj.GetComponent<PunchScript>().enabled = nulltex;

		handItem.gameObject.SetActive(!nulltex);	
		holdedItem.gameObject.SetActive(!nulltex);

		//Time.fixedTimeStamp*MaxDefaultHunger/90 minutes (or 5400 s); This is the time the hunger will be consumed without doing anything (90 minutes)
		float hungerDefaultConsum = 0.02f * 100 / 5400;
		
		//2 hours
		float thirstDefaultConsum = 0.02f * 100 / 7200;
		
		//A little control of hunger and thirst (0.02 seconds)
		
		//Regen
		
		if(PlayerStats.Hunger > 0) {
			PlayerStats.Hunger -= hungerDefaultConsum * customVelocityConst;
		} else {
			//The time to die can vary depending on the difficulty (default 30 seconds) //Now, if the difficulty is 1, set min hunger to 1/2, etc
			PlayerStats.Health -= 0.02f * PlayerStats.MaxHealth / LvlSys.TDS_var;
		}
		
		if(PlayerStats.Thirst > 0) {
			PlayerStats.Thirst -= thirstDefaultConsum * customVelocityConst;
		} else {
			//1 min to die after thirst empties...
			PlayerStats.Health -= 0.02f * PlayerStats.MaxHealth / LvlSys.TDH_var;
		}
		
		if(PlayerStats.Energy <= PlayerStats.MaxEnergy && (idling || walking)) {
			if(PlayerStats.Thirst > 0) {
				if(walking) {
					PlayerStats.Energy += 0.02f * PlayerStats.MaxEnergy / LvlSys.NTERW_var;
				} else if(idling) {
					PlayerStats.Energy += 0.02f * PlayerStats.MaxEnergy / LvlSys.NTERI_var;
				}
			} else {
				if(walking) {
					PlayerStats.Energy += 0.02f * PlayerStats.MaxEnergy / LvlSys.TERW_var;
				} else if(idling) {
					PlayerStats.Energy += 0.02f * PlayerStats.MaxEnergy / LvlSys.TERI_var;
				}
			}
		}
		
		if(PlayerStats.Hunger > 0 && PlayerStats.Thirst > 0 && PlayerStats.MaxHealth > PlayerStats.Health) {
			if(SkillSys.SearchSkillByName("regeneration").currentLevel > 0) {
				PlayerStats.Health += 0.02f * PlayerStats.MaxHealth / LvlSys.TTR_var;
			}
		}

		if(PlayerStats.invincible) {
			PlayerStats.Health = PlayerStats.MaxHealth;
		}

	}

	// Update is called once per frame
	void Update () 
	{

		if(Input.GetKeyDown(mInput.GetKey("SwitchView"))) 
		{
			Player.ThirdView = (Player.ThirdView == true) ? false : true;
			Player.PlayerObj.GetComponent<CharacterMotor>().enabled = !Player.ThirdView;
			PlayerSystem.SwitchPerson(player, Player.ThirdView);
		}

		if(Player.ThirdView && (Input.GetKey (KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetAxis ("Mouse ScrollWheel") != 0) {
			float r = Player.TPlayerObject.GetComponent<MouseLook>().r;
			r += Input.GetAxis("Mouse ScrollWheel")/10;

			if(r < 1) {
				r += 0.1f;
			} if(r > 6) {
				r -= 0.1f;
			}

			Player.TPlayerObject.GetComponent<MouseLook>().r = r;
		}

		if (Input.GetKeyDown(mInput.GetKey("EnableGUI"))) {
			EnableGUI = (EnableGUI == false) ? true : false;		
		}

		if(Input.GetKeyDown(mInput.GetKey("ToggleMap"))) {
			bigMap = (bigMap == false) ? true : false;
		}

		if (player.transform.position.y < waterLevel) 
		{
			swimming = true;
		} else {
			swimming = false;
		}

		//float vScale = 1.0f;

		walking = false;
		running = false;
		jumping = false;
		idling = false;

		if(Input.GetKeyDown(KeyCode.C)) {
			crouching = (crouching == true) ? false : true;
			if(crouching) {
				Vector3 camPos = Camera.main.transform.localPosition;
				Camera.main.transform.localPosition = new Vector3(camPos.x, 0.6f, camPos.z);
			} else {
				Vector3 camPos = Camera.main.transform.localPosition;
				Camera.main.transform.localPosition = new Vector3(camPos.x, 0.91f, camPos.z);
			}
		}

		if (Input.GetKey (KeyCode.W) && !jumping && !running && chMotor.grounded) {
			if(crouching) {
				anim.CrossFade("Male_crouch_WalkForward");
			} else {
				anim.CrossFade("Male_walkforward");
			}
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
				running = true;
				if(crouching) {
					anim.CrossFade("Male_crouch_RunForward");
				} else {
					anim.CrossFade("Male_runForwardFast");
				}
			}
			idling = false;
			walking = true;
		}

		if(Input.GetKey (KeyCode.A) && !jumping && !running && chMotor.grounded) {
				if(crouching) {
					anim.CrossFade("Male_crouch_WalkForward");
				} else {
					anim.CrossFade("Male_walkforward");
				}
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
					running = true;
					if(crouching) {
						anim.CrossFade("Male_crouch_RunForward");
					} else {
						anim.CrossFade("Male_runForwardFast");
					}
				}

			idling = false;
			walking = true;
		}

		if(Input.GetKey (KeyCode.D) && !jumping && !running && chMotor.grounded) {
				if(crouching) {
					anim.CrossFade("Male_crouch_WalkForward");
				} else {
					anim.CrossFade("Male_walkforward");
				}
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
					running = true;
					if(crouching) {
						anim.CrossFade("Male_crouch_RunForward");
					} else {
						anim.CrossFade("Male_runForwardFast");
					}
				}

			idling = false;
			walking = true;
		}

		if(Input.GetKey(KeyCode.S) && !jumping && !running && chMotor.grounded) {
				if(crouching) {
					anim.CrossFade("Male_crouch_WalkForward");
				} else {
					anim.CrossFade("Male_walkforward");
				}
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
					running = true;
					if(crouching) {
						anim.CrossFade("Male_crouch_RunForward");
					} else {
						anim.CrossFade("Male_runForwardFast");
					}
				}
			idling = false;
			walking = true;
		}

		if (Input.GetKey (KeyCode.Space) && !swimming && chMotor.grounded) {
			anim.CrossFade("Male_Jump");
			jumping = true;
			crouching = false;
		}

		if (running && PlayerStats.Energy > 0) 
		{
			idling = false;
			walking = false;
			ps.RunAndWalk(chMotor, runSpeed);
			PlayerStats.Energy -= LvlSys.ECOR_var;
		}

		if(walking) 
		{
			idling = false;
			ps.RunAndWalk(chMotor, walkSpeed);	
		}

		if(crouching) {
			idling = false;
			ps.Crouch(chMotor, crouchSpeed);
		}

		if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.LeftShift) || Input.GetKeyUp (KeyCode.RightShift) || Input.GetKeyUp (KeyCode.Space) || Input.GetKeyUp(KeyCode.C)) 
		{
			anim.CrossFade("Male_idle");	
			if(crouching) {
				anim.CrossFade("Male_crouch");
			} else {
				if(GunStats.IsAiming) {
					anim.CrossFade("Pistol_AimStraight");
				} else {
					anim.CrossFade("Male_idle");
				}
			}
		}

		if (!Input.anyKey) {
			idling = true;		
		}

		ps.MiniMap(cam, player, Options.MinimapZoom);

	}

	IEnumerator FallInvencibility() {

		while (!player.GetComponent<CharacterController>().isGrounded) {
			yield return null;
			PlayerStats.invincible = true;
		}

		PlayerStats.invincible = false;

	}

}
