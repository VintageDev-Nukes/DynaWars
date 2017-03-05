using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LvlSys {
	
	private static int _lvl;
	private static int _exp;
	
	public static int Level {
		get {return _lvl;}
		set {_lvl = value;}
	}
	
	public static int Exp {
		get {return _exp;}
		set {_exp = value;}
	}

	public static int HTS;
	public static int AccsX;
	public static int AccsY;
	public static int IIX;
	public static int IIY;
	public static int Ammo;

	//The percentages are used, to know how many units will be increased or decreased every stat on each level added...
	public const float Time_Die_Starving = 30;
	public const float TDS_Percentage = 12.5f;
	public const float Time_Die_Thirsty = 60;
	public const float TDH_Percentage = 12.5f;
	public const float No_Thirst_EnergyRestore_Walk = 90;
	public const float NTERW_Percentage = 11.5f;
	public const float No_Thirst_EnergyRestore_Idle = 35;
	public const float NTERI_Percentage = 11.5f;
	public const float Thirsty_EnergyRestore_Walk = 360;
	public const float TERW_Percentage = 11.5f;
	public const float Thirsty_EnergyRestore_Idle = 210;
	public const float TERI_Percentage = 11.5f;
	
	public const float Time_To_Regenerate = 600;
	public const float TTR_Percentage = 5;
	
	public const float Energy_Consumption_On_Running = 5;
	public const float ECOR_Percentage = 10;
	
	public static float TDS_var = Time_Die_Starving;
	public static float TDH_var = Time_Die_Thirsty;
	public static float NTERW_var = No_Thirst_EnergyRestore_Walk;
	public static float NTERI_var = No_Thirst_EnergyRestore_Idle;
	public static float TERW_var = Thirsty_EnergyRestore_Walk;
	public static float TERI_var = Thirsty_EnergyRestore_Idle;
	public static float TTR_var = Time_To_Regenerate;
	public static float ECOR_var = Energy_Consumption_On_Running;

	public static void Load() {
		Level = 0;
	}

	public static void SetSlotsByLvl() {
		int lvl = SkillSys.SearchSkillByName("maxload").currentLevel;
		switch(lvl) {

		case 0:
			HTS = 3;
			AccsX = 1;
			AccsY = 1;
			IIX = 3;
			IIY = 1;
			Ammo = 1;
			break;

		case 1:
			HTS = 3;
			AccsX = 2;
			AccsY = 1;
			IIX = 3;
			IIY = 2;
			Ammo = 1;
			break;

		case 2:
			HTS = 4;
			AccsX = 2;
			AccsY = 1;
			IIX = 4;
			IIY = 2;
			Ammo = 2;
			break;

		case 3:
			HTS = 5;
			AccsX = 2;
			AccsY = 2;
			IIX = 7;
			IIY = 2;
			Ammo = 2;
			break;

		case 4:
			HTS = 5;
			AccsX = 3;
			AccsY = 2;
			IIX = 9;
			IIY = 2;
			Ammo = 3;
			break;

		case 5:
			HTS = 5;
			AccsX = 3;
			AccsY = 2;
			IIX = 11;
			IIY = 2;
			Ammo = 3;
			break;

		case 6:
			HTS = 6;
			AccsX = 4;
			AccsY = 2;
			IIX = 12;
			IIY = 2;
			Ammo = 4;
			break;

		case 7:
			HTS = 6;
			AccsX = 4;
			AccsY = 2;
			IIX = 10;
			IIY = 3;
			Ammo = 4;
			break;

		case 8:
			HTS = 7;
			AccsX = 5;
			AccsY = 2;
			IIX = 11;
			IIY = 3;
			Ammo = 5;
			break;

		case 9:
			HTS = 8;
			AccsX = 5;
			AccsY = 2;
			IIX = 12;
			IIY = 3;
			Ammo = 5;
			break;
		
		case 10:
			HTS = 8;
			AccsX = 5;
			AccsY = 2;
			IIX = 12;
			IIY = 4;
			Ammo = 5;
			break;
			
		}
	}

	public static ulong GetExpfromLvl(int lvl, bool accumulative = false) 
	{

		ulong number = 0;

		if(accumulative) 
		{
			for(int i = 0; i<=lvl; ++i) 
			{
				number += (ulong)Mathf.RoundToInt(50 + i * Mathf.Pow((i+1), (1+i/Mathf.Pow(10, i.ToString().Length+3))) * 10);
			}
		} else 
		{
			number = (ulong)Mathf.RoundToInt(50 + lvl * Mathf.Pow((lvl+1), (1+lvl/Mathf.Pow(10, lvl.ToString().Length+3))) * 10);
		}
		
		return number;

	}

	public static ulong BonusCashfromLvlUp(int lvl, bool accumulative = false) {

		ulong number = 0;
		
		if(accumulative) 
		{
			for(int i = 0; i<=lvl; ++i) 
			{
				number += (ulong)Mathf.RoundToInt(25 + Mathf.Pow(i, (2+i/Mathf.Pow(10, i.ToString().Length))));
			}
		} else 
		{
			number = (ulong)Mathf.RoundToInt(25 + Mathf.Pow(lvl, (2+lvl/Mathf.Pow(10, lvl.ToString().Length))));
		}
		
		return number;

	} 

	public static void RecalculateLvl() {

		MoneySystem money = GameObject.Find("GameScripts").GetComponent<MoneySystem>();

		int actualLvl = PlayerStats.Lvl;
		int newlvl = PlayerStats.Lvl;
		ulong exp = PlayerStats.Exp;
		
		//bool reachednewlvl = false;
		
		if(GetExpfromLvl(actualLvl, true) < exp) 
		{

			newlvl = actualLvl + 1;
			PlayerStats.SkillPoints += 1+Mathf.FloorToInt(actualLvl/3)*2;
			money.ChangeMoney(BonusCashfromLvlUp(actualLvl));
			if(actualLvl % Random.Range(2, 5) == 0 && GameObject.Find("Audio: lvlup") == null) AudioManager.Play(Resources.Load<AudioClip>("sounds/lvlup"), Player.PlayerObj.transform.position);

		}

		PlayerStats.Lvl = newlvl;

		if(PlayerStats.Lvl > actualLvl) {
			NPCSystem.NPCCheck();
		}

	}

	public static ulong tempExp;
	public static string newExp;

	public static IEnumerator FadeExp(ulong newExpSum, float duration) {
		
		float t = 0;
		
		newExp = "+"+newExpSum.ToString();
		
		while(true) {
			
			yield return null;

			Color tmpColor = GUIStyles.ExpStyle.normal.textColor;
			
			if(t < duration) {
				tmpColor.a = Mathf.Lerp(0, 1, t/duration);
				GUIStyles.NewExpStyle.normal.textColor = tmpColor;
			} else if(t > duration && t < duration*2) {
				tmpColor.a = Mathf.Lerp(1, 0, t/duration/2);
				GUIStyles.NewExpStyle.normal.textColor = tmpColor;
			}
			
			t += Time.deltaTime;
			
			if(t > duration*2) {
				break;
			}
			
		}
		
	}
	
}

public class LevelSystem : MonoBehaviour {

	// Use this for initialization
	void Start() {

		SkillSys.Load();
		//SkillSys.LoadSkills(Profile.currentProfile.lastPlayedWorld);
		LvlSys.Load();
		
		Inv inventory = new Inv();
		
		ItemBase.MainBase = inventory.Load();
		
		/*foreach(Skill skill in SkillSys.skills) {
			SkillSys.SkillCheck(skill.name); //This causes an error if the maxload lvl is greater than 1
			SkillSys.AddStat(skill.name);
		}*/
		
		SkillSys.SetUnsafeSkills(SkillSys.safeSkill);
		
		LvlSys.SetSlotsByLvl();
		inventory.InitialLoad();
		
		Slots.Set(Inv.safeInvSlots);

		NPCSystem.Load();

		CraftSystem.Load();
		
		//Inv.AddItem(ItemBase.MainBase.FirstOrDefault(x => x.id == 64), 0, SlotsTypes.Inventory);
		
		//Inv.LoadInventory(Profile.lastPlayedWorld);

	}
	
	// Update is called once per frame
	void Update() {
	
		if(PlayerStats.Exp != LvlSys.tempExp) {
			StartCoroutine(LvlSys.FadeExp(PlayerStats.Exp - LvlSys.tempExp, 2));
			LvlSys.RecalculateLvl();
		}

		LvlSys.tempExp = PlayerStats.Exp;

	}
}
