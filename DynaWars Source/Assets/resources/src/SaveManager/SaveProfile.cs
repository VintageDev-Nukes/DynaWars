using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveProfile {

	//Player stats, hours, inventory, skills, objects in scene, etc

	public string lvlName;
	public Vector_3 playerPos;
	public Vector_3 playerRot;
	public int seed;
	public string strSeed;
	public float maxHealth;
	public float maxEnergy;
	public float maxThirst;
	public float maxHunger;
	public float health;
	public float energy;
	public float thirst;
	public float hunger;
	public ulong playerMoney;
	public int skillPoints;
	public int playerLvl;
	public ulong playerExp;
	public float currentHour;
	public List<SafeSkill> skillColletion;
	public SafeSlots slots;

	public void SetPlayerValues() {
		playerPos = new Vector_3(Random.Range(Random.Range(-1000, 0), Random.Range(0, 1000)), 30, Random.Range(Random.Range(-1000, 0), Random.Range(0, 1000)));
		playerRot = new Vector_3();	
		skillColletion = SkillSys.GetDefaultSkills();
		slots = SafeSlots.GetDefaultSlots();
		currentHour = 0.35f;
		maxEnergy = 100;
		maxHealth = 100;
		maxHunger = 100;
		maxThirst = 100;
		energy = 100;
		health = 100;
		hunger = 100;
		thirst = 100;
	}

}

public class GameSaver {

	public static SaveProfile profile;

	//Set playerstats, hour, etc
	public static void Load(string lvlName) {
		GameObject player = GameObject.Find("Player");
		SaveProfile savedProfile = XMLTools.DeserializeFromFile<SaveProfile>(Application.dataPath + "/saves/" + lvlName + "/level.xml");
		Profile.currentProfile.lastPlayedWorld = savedProfile.lvlName;
		player.transform.position = savedProfile.playerPos.GetVector3();
		player.transform.eulerAngles = savedProfile.playerRot.GetVector3();
		PlayerStats.MaxEnergy = savedProfile.maxEnergy;
		PlayerStats.MaxHealth = savedProfile.maxHealth;
		PlayerStats.MaxHunger = savedProfile.maxHunger;
		PlayerStats.MaxThirst = savedProfile.maxThirst;
		PlayerStats.Energy = savedProfile.energy;
		PlayerStats.Health = savedProfile.health;
		PlayerStats.Hunger = savedProfile.hunger;
		PlayerStats.Thirst = savedProfile.thirst;
		PlayerStats.Money = savedProfile.playerMoney;
		PlayerStats.Lvl = savedProfile.playerLvl;
		PlayerStats.SkillPoints = savedProfile.skillPoints;
		GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().slider = savedProfile.currentHour;
		GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().slider2 = savedProfile.currentHour;
		GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().Hour = savedProfile.currentHour*24;
		GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().Tod = savedProfile.currentHour*24;
		SkillSys.safeSkill = savedProfile.skillColletion;
		Inv.safeInvSlots = savedProfile.slots;
		TerrainGenerator.mySeed = savedProfile.strSeed;
	}

	public static void Save(string lvlName) {

		SetCurrentProfile();

		//profile.slots = SafeSlots.Get();

		XMLTools.SerializeToFile<SaveProfile>(profile, Application.dataPath + "/saves/" + lvlName + "/level.xml", true);

	}

	public static void SetCurrentProfile() {
		SkillSys.SetSafeSkills(SkillSys.skills);
		GameObject player = GameObject.Find("Player");
		profile = new SaveProfile() {
			lvlName = Profile.currentProfile.lastPlayedWorld,
			playerPos = new Vector_3(player.transform.position),
			playerRot = new Vector_3(player.transform.eulerAngles),
			maxEnergy = PlayerStats.MaxEnergy,
			maxHealth = PlayerStats.MaxHealth,
			maxHunger = PlayerStats.MaxHunger,
			maxThirst = PlayerStats.MaxThirst,
			energy = PlayerStats.Energy,
			health = PlayerStats.Health,
			hunger = PlayerStats.Hunger,
			thirst = PlayerStats.Thirst,
			playerMoney = PlayerStats.Money,
			playerLvl = PlayerStats.Lvl,
			skillPoints = PlayerStats.SkillPoints,
			currentHour = GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().slider,
			skillColletion = SkillSys.safeSkill,
			slots = SafeSlots.Get(),
			seed = new SeedExtension(TerrainGenerator.mySeed).ToInt(),
			strSeed = TerrainGenerator.mySeed
		};
	}

}
