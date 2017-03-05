using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using env = System.Environment;

public class Entity {

	public float life, lastTerCol, velocity;
	public bool isFalling, damaged;
	public Vector3 fallPoint, fallHeight, vVelocity, lastPosition, curPosition;
	public string lastCol;

	private static int _mobNum;
	
	public static int MobNum {
		get {return _mobNum;}
		set {_mobNum = value;}
	}

}

public class EntityLib {

	private static Dictionary<string, Entity> _entities = new Dictionary<string, Entity>();

	public static Dictionary<string, Entity> Entities {
		get {return _entities;}
		set {_entities = value;}
	}

}

public class Player {
	
	private static GameObject _player;
	private static GameObject _TplayerObject;
	private static GameObject _TplayerModel;
	private static Material _playerMat;
	private static Texture _playerTex;
	private static bool _thirdView;

	public static GameObject PlayerObj {
		get {return _player;}
		set {_player = value;}
	}

	public static GameObject TPlayerObject {
		get {return _TplayerObject;}
		set {_TplayerObject = value;}
	}
	
	public static GameObject TPlayerModel {
		get {return _TplayerModel;}
		set {_TplayerModel = value;}
	}

	public static bool ThirdView {
		get {return _thirdView;}
		set {_thirdView = value;}
	}

}

public class PlayerStats {

	private static float _health, _thirst, _hunger, _energy, _armor, _mana, _breath;

	private static bool _killed;

	//Max stats

	private static float _maxhealth, _maxthirst, _maxhunger, _maxenergy, _maxmana, _muscle, _lung, _endurance, _fat, _velocity;

	private static int _lvl, _skillpoints;
	private static ulong _exp, _money;

	public static bool died, invincible;

	//Independientes: Habilidad de conduccion y habilidad con armas

	public static float Health {
		get {return _health;}
		set {_health = value;}
	}

	public static float Thirst {
		get {return _thirst;}
		set {_thirst = value;}
	}

	public static float Hunger {
		get {return _hunger;}
		set {_hunger = value;}
	}

	public static float Energy {
		get {return _energy;}
		set {_energy = value;}
	}

	public static bool Killed {
		get {return _killed;}
		set {_killed = value;}
	}

	public static float Armor {
		get {return _armor;}
		set {_armor = value;}
	}

	public static float Mana {
		get {return _mana;}
		set {_mana = value;}
	}

	public static float MaxHealth {
		get {return _maxhealth;}
		set {_maxhealth = value;}
	}
	
	public static float MaxThirst {
		get {return _maxthirst;}
		set {_maxthirst = value;}
	}
	
	public static float MaxHunger {
		get {return _maxhunger;}
		set {_maxhunger = value;}
	}
	
	public static float MaxEnergy {
		get {return _maxenergy;}
		set {_maxenergy = value;}
	}
	
	public static float MaxMana {
		get {return _maxmana;}
		set {_maxmana = value;}
	}

	public static float Breath {
		get {return _breath;}
		set {_breath = value;}
	}

	public static float Muscle {
		get {return _muscle;}
		set {_muscle = value;}
	}

	public static float Lung {
		get {return _lung;}
		set {_lung = value;}
	}

	public static float Endurance {
		get {return _endurance;}
		set {_endurance = value;}
	}

	public static float Fat {
		get {return _fat;}
		set {_fat = value;}
	}

	public static float Velocity {
		get {return _velocity;}
		set {_velocity = value;}
	}

	private static bool _isFalling = false;

	private static Vector3 _fall;
	private static Vector3 _fallpoint;

	public static bool IsFalling {
		get {return _isFalling;}
		set {_isFalling = value;}
	}
	
	public static Vector3 FallPoint {
		get {return _fallpoint;}
		set {_fallpoint = value;}
	}
	
	public static Vector3 FallHeight {
		get {return _fall;}
		set {_fall = value;}
	}

	public static ulong Money {
		get {return _money;}
		set {_money = value;}
	}

	public static int SkillPoints {
		get {return _skillpoints;}
		set {_skillpoints = value;}
	}

	public static int Lvl {
		get {return _lvl;}
		set {_lvl = value;}
	}
	public static ulong Exp {
		get {return _exp;}
		set {_exp = value;}
	}

}

public class PlayerSystem {

	//private bool PlayerLoaded = false;
	private GameObject player;
	//private bool playerposChanged = false;

	public void SetPlayerOnGround(GameObject PlayerObject)
	{

		/*float PlaneSize = GameObject.Find ("GameScripts").GetComponent<TerrainGenerator>().planeSize;

		float maxHeight = GameObject.Find ("GameScripts").GetComponent<TerrainGenerator>().heightScale;
		float x = PlayerObject.transform.position.x;
		float z = PlayerObject.transform.position.z;

		if (!PlayerLoaded) {
			player = (GameObject)GameObject.Instantiate (Resources.Load ("prefabs/Player"), new Vector3(x, maxHeight + 5, z), Quaternion.identity);
			PlayerLoaded = true;
		}

		if(!playerposChanged) {
			player.transform.position = new Vector3 (PlayerObject.transform.position.x, player.transform.position.y, PlayerObject.transform.position.z);
			float distance = Vector3.Distance (PlayerObject.transform.position, player.transform.position);
			PlayerObject.transform.position = new Vector3(PlayerObject.transform.position.x, player.transform.position.z+5, PlayerObject.transform.position.z);		
			GameObject.Destroy(player);
			playerposChanged = true;	
		}*/

		float x = PlayerObject.transform.position.x;
		float z = PlayerObject.transform.position.z;
		float y = GameObject.Find ("GameScripts").GetComponent<TerrainGenerator>().GetHeights(x, z)+2;

		PlayerObject.transform.position = new Vector3(x, y, z);

	}

	public void MiniMap(Camera camera, GameObject player, float seg) {

		camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + seg, player.transform.position.z);

		//Avoid mob desplacing
		//camera.transform.eulerAngles = new Vector3(90, player.transform.eulerAngles.y, player.transform.eulerAngles.z); //player.transform.rotation;

	}

	public void RandomSpawn(GameObject player) 
	{

		TerrainGenerator tg = GameObject.Find("GameScripts").GetComponent<TerrainGenerator>();

		float randX = Random.Range(Random.Range(-1000, 0), Random.Range(0, 1000));
		float randZ = Random.Range(Random.Range(-1000, 0), Random.Range(0, 1000));
		float randY = tg.GetHeights(randX, randZ);

		player.transform.position = new Vector3 (randX, Mathf.Abs(randY)+1, randZ);

	}

	public void RunAndWalk(CharacterMotor chMotor, float speed) 
	{
		chMotor.movement.maxForwardSpeed = speed; // set max speed
	}
	
	public void Crouch(CharacterMotor chMotor, float speed) 
	{
		chMotor.movement.maxForwardSpeed = speed;

		//Vector3 camPos = Camera.main.transform.position;

		//thirdPerson.localScale = new Vector3(1, vScale, 1);
		
		//This was an example of how to make player look smaller when it was crouching (like slendy tubbies)
		/*float ultScale = thirdPerson.localScale.y; // crouch/stand up smoothly 
		
		Vector3 tmpScale = thirdPerson.localScale;
		Vector3 tmpPosition = thirdPerson.position;
		
		tmpScale.y = Mathf.Lerp(thirdPerson.localScale.y, vScale, 5 * Time.deltaTime);
		thirdPerson.localScale = tmpScale;
		
		tmpPosition.y += dist * (thirdPerson.localScale.y - ultScale); // fix vertical position       
		thirdPerson.position = tmpPosition;*/
	}

	/*public void SaveGame(GameObject player2, string lvlName) {

		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "playerX", player2.transform.position.x.ToString("F3")); //VectorSerializer.Searialize3(player.transform.position)
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "playerY", player2.transform.position.y.ToString("F3"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "playerZ", player2.transform.position.z.ToString("F3"));

		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "playerotX", player2.transform.rotation.x.ToString("F1"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "playerotY", player2.transform.rotation.y.ToString("F1"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "playerotZ", player2.transform.rotation.z.ToString("F1"));

		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "maxHealth", PlayerStats.MaxHealth.ToString("F3"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "maxEnergy", PlayerStats.MaxEnergy.ToString("F3"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "maxThirst", PlayerStats.MaxThirst.ToString("F3"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "maxHunger", PlayerStats.MaxHunger.ToString("F3"));

		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "health", PlayerStats.Health.ToString("F5"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "energy", PlayerStats.Energy.ToString("F5"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "thirst", PlayerStats.Thirst.ToString("F5"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "hunger", PlayerStats.Hunger.ToString("F5"));

		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "money", PlayerStats.Money.ToString("F0"));

		INI_Manager.Set_Value (Application.dataPath + "/saves/" + lvlName + "/level.dat", "skillpoints", PlayerStats.SkillPoints.ToString ());

		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "lvl", PlayerStats.Lvl.ToString("F0"));
		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "exp", PlayerStats.Exp.ToString("F0"));

		float currentHour = GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().slider;

		INI_Manager.Set_Value(Application.dataPath + "/saves/"+lvlName+"/level.dat", "currentHour", currentHour.ToString("F5"));

		//Save inventory (done), player stats (done), lvl (done), money (done), objects in scene (including dropped items and mobs), current Hour

		SkillSys.SaveSkills(lvlName);
		Inv.SaveInventory(lvlName);

		TextExt.CleanEmptyLines(Application.dataPath + "/saves/"+lvlName+"/level.dat");

	}

	public static void LoadLevel(string lvlName, GameObject player) {

		PlayerSystem ps = new PlayerSystem();

		bool NewWorld = false; //NewWorld va a ser siempre false...

		try {
			string testStr = INI_Manager.Load_Value("maxHealth", Application.dataPath + "/saves/" + lvlName + "/level.dat");
		} catch { //A no ser que la primera key que siempre se guarda al final de la primera partida no se encuentre...
			NewWorld = true;
		}

		//if (string.IsNullOrEmpty (INI_Manager.Load_Value("playerX", Application.dataPath + "/saves/" + lvlName + "/level.dat")) || string.IsNullOrEmpty(INI_Manager.Load_Value("playerY", Application.dataPath + "/saves/" + lvlName + "/level.dat")) || string.IsNullOrEmpty(INI_Manager.Load_Value("playerZ", Application.dataPath + "/saves/" + lvlName + "/level.dat")) || string.IsNullOrEmpty (INI_Manager.Load_Value("playerotX", Application.dataPath + "/saves/" + lvlName + "/level.dat")) || string.IsNullOrEmpty (INI_Manager.Load_Value("playerotY", Application.dataPath + "/saves/" + lvlName + "/level.dat")) || string.IsNullOrEmpty(INI_Manager.Load_Value("playerotZ", Application.dataPath + "/saves/" + lvlName + "/level.dat"))) {
		if(NewWorld) {

			//SPAWN...

			ps.RandomSpawn(player); //Causa un array out of range //[FIXED!]
			ps.SetPlayerOnGround(player);

			PlayerStats.MaxHealth = 100;
			PlayerStats.MaxEnergy = 100;
			PlayerStats.MaxThirst = 100;
			PlayerStats.MaxHunger = 100;

			ps.FillStats();

			//This doesnt work
			//if(!player.GetComponent<CharacterController>().isGrounded) {
				//StartCoroutine(FallInvencibility());
			//}

		} else {

			//LOAD SAVED VARIABLES...

			player.transform.position = new Vector3(float.Parse(INI_Manager.Load_Value("playerX", Application.dataPath + "/saves/" + lvlName + "/level.dat")), float.Parse(INI_Manager.Load_Value("playerY", Application.dataPath + "/saves/" + lvlName + "/level.dat"))+0.1f, float.Parse(INI_Manager.Load_Value("playerZ", Application.dataPath + "/saves/" + lvlName + "/level.dat")));
			player.transform.Rotate(new Vector3(float.Parse(INI_Manager.Load_Value("playerotX", Application.dataPath + "/saves/" + lvlName + "/level.dat")), float.Parse(INI_Manager.Load_Value("playerotY", Application.dataPath + "/saves/" + lvlName + "/level.dat")), float.Parse(INI_Manager.Load_Value("playerotZ", Application.dataPath + "/saves/" + lvlName + "/level.dat"))));

			PlayerStats.MaxHealth = float.Parse(INI_Manager.Load_Value("maxHealth", Application.dataPath + "/saves/" + lvlName + "/level.dat"));
			PlayerStats.MaxEnergy = float.Parse(INI_Manager.Load_Value("maxEnergy", Application.dataPath + "/saves/" + lvlName + "/level.dat"));
			PlayerStats.MaxThirst = float.Parse(INI_Manager.Load_Value("maxThirst", Application.dataPath + "/saves/" + lvlName + "/level.dat"));
			PlayerStats.MaxHunger = float.Parse(INI_Manager.Load_Value("maxHunger", Application.dataPath + "/saves/" + lvlName + "/level.dat"));

			PlayerStats.Health = float.Parse(INI_Manager.Load_Value("health", Application.dataPath + "/saves/" + lvlName + "/level.dat"));
			PlayerStats.Energy = float.Parse(INI_Manager.Load_Value("energy", Application.dataPath + "/saves/" + lvlName + "/level.dat"));
			PlayerStats.Thirst = float.Parse(INI_Manager.Load_Value("thirst", Application.dataPath + "/saves/" + lvlName + "/level.dat"));
			PlayerStats.Hunger = float.Parse(INI_Manager.Load_Value("hunger", Application.dataPath + "/saves/" + lvlName + "/level.dat"));

			PlayerStats.Exp = ulong.Parse(INI_Manager.Load_Value("exp", Application.dataPath + "/saves/" + lvlName + "/level.dat"));
			PlayerStats.Lvl = int.Parse(INI_Manager.Load_Value("lvl", Application.dataPath + "/saves/" + lvlName + "/level.dat"));
			PlayerStats.Money = ulong.Parse(INI_Manager.Load_Value("money", Application.dataPath + "/saves/" + lvlName + "/level.dat"));

			PlayerStats.SkillPoints = int.Parse(INI_Manager.Load_Value("skillpoints", Application.dataPath + "/saves/" + lvlName + "/level.dat"));

			float slider = float.Parse(INI_Manager.Load_Value("currentHour", Application.dataPath + "/saves/" + lvlName + "/level.dat"));

			GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().slider = slider;
			GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().slider2 = slider;
			GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().Hour = slider*24;
			GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().Tod = slider*24;

		}

	}*/

	public static void SwitchPerson(GameObject player, bool thirdView, bool isAiming = false) {

		Player.PlayerObj.GetComponent<FPSInputController>().ThirdView = thirdView;

		if (thirdView) {

			Player.PlayerObj.transform.FindChild("mainCam").camera.cullingMask = ~((1 << 9) | (1 << 16));

			Player.PlayerObj.transform.FindChild("mainCam").FindChild("Hands").gameObject.SetActive(false);

			if(isAiming) {

				Player.PlayerObj.transform.FindChild("mainCam").transform.localPosition = new Vector3(0.25f, 1.25f, -3);
				
				Player.PlayerObj.GetComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseX;
				Player.PlayerObj.GetComponent<MouseLook>().IsFixed = false;
				Player.PlayerObj.GetComponent<MouseLook>().r = 0;
				
				Player.PlayerObj.transform.FindChild("mainCam").GetComponent<MouseLook>().enabled = true;
				
				Player.PlayerObj.transform.FindChild("mainCam").GetComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseY;
				Player.PlayerObj.transform.FindChild("mainCam").GetComponent<MouseLook>().r = 0;

				Player.PlayerObj.transform.FindChild("mainCam").GetComponent<MouseLook>().maximumY = 90;
				Player.PlayerObj.transform.FindChild("mainCam").GetComponent<MouseLook>().minimumY = -90;

			} else {

				Player.PlayerObj.transform.FindChild("mainCam").transform.localPosition = new Vector3(-3, 0.907f, -3);

				Player.PlayerObj.transform.FindChild("mainCam").GetComponent<MouseLook>().enabled = false;
				Player.PlayerObj.GetComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseXAndY;
				Player.PlayerObj.GetComponent<MouseLook>().IsFixed = true;
				Player.PlayerObj.GetComponent<MouseLook>().r = 3;

			}

		} else {

			Player.PlayerObj.transform.FindChild("mainCam").FindChild("Hands").gameObject.SetActive(true);
			
			Player.PlayerObj.GetComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseX;
			Player.PlayerObj.GetComponent<MouseLook>().IsFixed = false;
			
			Player.PlayerObj.transform.FindChild("mainCam").GetComponent<MouseLook>().enabled = true;
			Player.PlayerObj.transform.FindChild("mainCam").transform.localPosition = new Vector3(0, 0.907f, 0);
			
			Player.PlayerObj.transform.FindChild("mainCam").GetComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseY;
			Player.PlayerObj.transform.FindChild("mainCam").GetComponent<MouseLook>().r = 0;
			
			Player.PlayerObj.transform.FindChild("mainCam").camera.cullingMask = ~(1 << 8);

		}

		SelectedSlot.switchedPer = true;

	}

	public void FillStats() {

		PlayerStats.Health = PlayerStats.MaxHealth;
		PlayerStats.Energy = PlayerStats.MaxEnergy;
		PlayerStats.Thirst = PlayerStats.MaxThirst;
		PlayerStats.Hunger = PlayerStats.MaxHunger;

	}

	public void Respawn() {

		GameObject.Find("Ragdoll").SetActive(false);
		Player.PlayerObj.SetActive(true);

		GameObject.Find("Player").GetComponent<CharacterMotor>().enabled = true;
		GameObject.Find("Player").GetComponent<FPSInputController>().enabled = true;

		PlayerStats.Health = PlayerStats.MaxHealth;
		PlayerStats.Money = 0;

		PlayerStats.Killed = false;
		PlayerStats.died = false;

		// Bug !!!
		PlayerSystem.SwitchPerson(player, Player.ThirdView);

		RandomSpawn(Player.PlayerObj);

	}

	public void Die() {

		GameObject newGm = null;

		if(!PlayerStats.died) {
			if(Player.PlayerObj.transform.FindChild("MaleCharacter").FindChild("Reference").gameObject.GetComponent<RagdollBuilder>() == null) {

				PlayerSystem.SwitchPerson(Player.PlayerObj, true);

				newGm = new GameObject();
				
				newGm.name = "Ragdoll";

				Player.PlayerObj.transform.FindChild("MaleCharacter").GetComponent<Animation>().enabled = false;
				GameObject.Instantiate(Player.PlayerObj.transform.FindChild("MaleCharacter"));
				Player.PlayerObj.transform.FindChild("MaleCharacter").GetComponent<Animation>().enabled = true;
				Player.PlayerObj.transform.FindChild("KillCam").gameObject.SetActive(true);
				GameObject.Instantiate(Player.PlayerObj.transform.FindChild("KillCam"));
				Player.PlayerObj.transform.FindChild("KillCam").gameObject.SetActive(false);

				RagdollBuilder ragdollbuild = GameObject.Find("MaleCharacter(Clone)").transform.FindChild("Reference").gameObject.AddComponent<RagdollBuilder>();
				RagdollExt.PlayerToRagdoll(ragdollbuild, GameObject.Find("MaleCharacter(Clone)").transform.FindChild("Reference").gameObject).CreateAll();

				GameObject.Find("MaleCharacter(Clone)").transform.parent = newGm.transform;
				GameObject.Find("KillCam(Clone)").transform.parent = newGm.transform;

				newGm.transform.position = Player.PlayerObj.transform.position;

				GameObject.Find("Player").SetActive(false);

				PlayerStats.died = true;

			}
		}

		if(GameObject.Find("Ragdoll").activeSelf) {
			GameObject.Find("Ragdoll").transform.FindChild("KillCam(Clone)").LookAt(GameObject.Find("Ragdoll").transform.FindChild("MaleCharacter(Clone)").FindChild("Reference").FindChild("Hips").position);
			GameObject.Find("Ragdoll").transform.FindChild("KillCam(Clone)").position = GameObject.Find("Ragdoll").transform.FindChild("MaleCharacter(Clone)").FindChild("Reference").FindChild("Hips").position+new Vector3(0, 0, 3);
		}
		
		//Trying to make Ragdoll gameobject follow ragdoll
		//GameObject.Find("Ragdoll").transform.position = GameObject.Find("Ragdoll").transform.FindChild("MaleCharacter(Clone)").FindChild("Reference").FindChild("Hips").position+new Vector3(0, 0, 3);


	}

}
