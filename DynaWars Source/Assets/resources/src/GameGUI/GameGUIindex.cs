using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using Random = System.Random;

public enum GUIPlaces {settingsMenu, optionsMenu, Inv, waypointCreator, shop, personal, none};
public enum subPlaces {slots, craft, skills, storage, buy, sell, none};

public class MouseProperties 
{
	private static bool _mouseHelded;
	
	public static bool mouseHelded {
		get {return _mouseHelded;}
		set {_mouseHelded = value;}
	}
}

public class Menus {

	private static bool _pauseMenu;
	private static bool _wpCreator;
	private static float _tempZoom;
	
	public static bool pauseMenu {
		get {return _pauseMenu;}
		set {_pauseMenu = value;}
	}

	public static bool WaypointCreator {
		get {return _wpCreator;}
		set {_wpCreator = value;}
	}

	public static float MapZoom {
		get {return _tempZoom;}
		set {_tempZoom = value;}
	}

}

public class GUIStyles {

	private static GUIStyle _moneyStyle, _lvlStyle, _expStyle, _debugStyle, _debugVStyle, _dieStyle, _newExpStyle;
	private static Color _moneyColor = new Color(0, 0.75f, 0);

	public static GUIStyle MoneyStyle {
		get {return _moneyStyle;}
		set {_moneyStyle = value;}
	}

	public static Color MoneyColor {
		get {return _moneyColor;}
		set {_moneyColor = value;}
	}

	public static GUIStyle LvlStyle {
		get {return _lvlStyle;}
		set {_lvlStyle = value;}
	}

	public static GUIStyle ExpStyle {
		get {return _expStyle;}
		set {_expStyle = value;}
	}

	public static GUIStyle DebugStyle {
		get {return _debugStyle;}
		set {_debugStyle = value;}
	}

	public static GUIStyle DebugVStyle {
		get {return _debugVStyle;}
		set {_debugVStyle = value;}
	}

	public static GUIStyle DieStyle {
		get {return _dieStyle;}
		set {_dieStyle = value;}
	}

	public static GUIStyle NewExpStyle {
		get {return _newExpStyle;}
		set {_newExpStyle = value;}
	}

}

public class GameGUIindex : MonoBehaviour {

	//MiniMap Variables
	private Texture2D minimapFrame, expBar;

	//Lvl variables
	private Texture2D lvl;

	//ItemHUD Variables
	private Texture2D itemHUD;

	//Circle's variables
	int texSize;

	//Textures for HUD
	Texture2D Vida, Sed, Hambre, Energia;

	//Maths for HUD Bars
	float Radius, CoronaCircular, CircleX, CircleY;

	//Angles for HUD bars
	float AnguloVida, AnguloSed, AnguloHambre, AnguloEnergia;

	//Menus
	bool pauseMenu = true, bigMap, wpColorPalette;
	public GUIPlaces currentPlace = GUIPlaces.settingsMenu;
	subPlaces subPlace = subPlaces.slots;
	//, SettingsMenu, optionsMenu = true, Inv, slots = true, craft, skills, storage, shop, personal, bigMap, waypointCreator, wpColorPalette;

	//Scrolls
	float PersonalRowNum = 0, MenuvScroll = 0, SkillScroll = 0, shopScroll = 0, craftScroll = 0;

	//GUI Bools
	bool EnableGUI = true;

	//Menu Textures
	Texture2D preInv, tabInvNormal, tabInvHover, tabInvFocused, tabInvActive, preTab, centerGrid, lvlStripe, RedScreen;

	//GUI Styles + Transported to a new class
	GUIStyle lvlStripeStyle;

	//Tab System
	GUIStyle tabStyle;

	private PlayerSystem ps;

	public RenderTexture minimapTexture;
	public Material minimapMaterial;
	//public Texture2D minimapGear;

	//string lvlName = "";

	bool DebugScreen;
	string DebugText;

	//Inventory Inicialization
	Inv inv2;

	//This is the object the mouse is carrying
	InventoryItem mouseitem;

	//Skills textures
	public Texture2D gSkill, uSkill, bSkill;

	bool IsAdjusted;

	//public GameObject cCam;

	public RenderTexture invHudTexture;
	public Material invHudMaterial;

	public RenderTexture bigmapTexture;
	public Material bigmapMaterial;
	
	public GameObject cCam;

	//private GameObject player;

	public GameObject player;

	private float lastVida, lastHambre, lastSed, lastEnergia;

	public Font fuente;

	float tmpZoom;

	string waypointName, wpxCoordStr, wpyCoordStr, wpzCoordStr;

	Texture2D wpColorPreview;

	float wpRed, wpGreen, wpBlue;

	Color finalColor;

	int selectedShopNPC = 0, selectedShopItem = 0, stacktobuy = 0, selectedCraftItem = 0, selectedCrafting = 0, stacktocraft = 0;

	//private GUIStyle nullstyle;

	void OnApplicationQuit() {
		GameSaver.Save(Profile.currentProfile.lastPlayedWorld);
		//ps.SaveGame(player, Profile.lastPlayedWorld);
		mInput.SaveKeys();
		Profile.Save();
	}

	Vector2 mousePosFix;

	// Use this for initialization
	void Start() 
	{
		
		inv2 = new Inv ();
		ps = new PlayerSystem();
		
		player = GameObject.Find("Player");
		
		Menus.MapZoom = Options.MinimapZoom;
		
		GUIStyles.DebugStyle = new GUIStyle() {fontSize = 20, fontStyle = FontStyle.Bold};

		GUIStyles.DebugStyle.normal.textColor = Color.white;
		
		GUIStyles.DebugVStyle = new GUIStyle();
		GUIStyles.DebugVStyle.fontSize = 20;
		GUIStyles.DebugVStyle.fontStyle = FontStyle.Bold;
		GUIStyles.DebugVStyle.normal.textColor = Color.white;
		GUIStyles.DebugVStyle.alignment = TextAnchor.UpperRight;

		wpColorPreview = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);

		minimapFrame = (Texture2D)Resources.Load ("images/MINIMAP1");
		expBar = (Texture2D)Resources.Load ("images/exp-bar2");
		lvl = (Texture2D)Resources.Load ("images/lvl2");
		itemHUD = (Texture2D)Resources.Load ("images/item-hud2");

		preInv = (Texture2D)Resources.Load ("images/preInv2");
		
		tabInvNormal = (Texture2D)Resources.Load("images/tabInvNormal2");
		tabInvHover = (Texture2D)Resources.Load("images/tabInvHover2");
		tabInvFocused = (Texture2D)Resources.Load("images/tabInvFocused2");
		tabInvActive = (Texture2D)Resources.Load("images/tabInvActive2");
		
		preTab = (Texture2D)Resources.Load ("images/preTab2");
		
		centerGrid = (Texture2D)Resources.Load("images/center_grid");

		lvlStripe = (Texture2D)Resources.Load("images/lvlStripe");
		
		RedScreen = (Texture2D)Resources.Load("images/ScreenRed");

		gSkill  = (Texture2D)Resources.Load("images/gSkill");

		uSkill = (Texture2D)Resources.Load("images/uSkill");

		bSkill = (Texture2D)Resources.Load("images/bSkill");

		tmpZoom = Options.MinimapZoom;
		
		GUIStyles.DieStyle = new GUIStyle();

		GUIStyles.DieStyle.fontSize = 24;
		GUIStyles.DieStyle.alignment = TextAnchor.MiddleCenter;
		GUIStyles.DieStyle.normal.textColor = Color.white;
		GUIStyles.DieStyle.fontStyle = FontStyle.Bold;
		
		tabStyle = new GUIStyle() {
			alignment = TextAnchor.MiddleCenter, 
			fontStyle = FontStyle.Bold, 
			normal = new GUIStyleState() {
				textColor = new Color (1, 1, 1, 1), 
				background = tabInvNormal}, 
			hover = new GUIStyleState() {
				textColor = new Color (1, 1, 1, 1), 
				background = tabInvHover}, 
			focused = new GUIStyleState() {
				textColor = new Color (1, 1, 1, 1), 
				background = tabInvFocused},
			active = new GUIStyleState() {
				textColor = new Color (1, 0, 0, 1),
				background = tabInvActive}
		};
		
		//lvlName = INI_Manager.Load_Value("lastWorldPlayed", Application.dataPath + "/appConfig.cfg");
		
		Radius = 86.03278f;
		CoronaCircular = 76.8092f;
		CircleX = 115.8003f;
		CircleY = 118.5574f;
		
		GUIStyles.MoneyStyle = new GUIStyle();
		
		GUIStyles.MoneyStyle.fontSize = 60;
		GUIStyles.MoneyStyle.font = fuente;

		GUIStyles.LvlStyle = new GUIStyle();

		GUIStyles.LvlStyle.fontSize = 18;
		GUIStyles.LvlStyle.fontStyle = FontStyle.Bold;
		GUIStyles.LvlStyle.alignment = TextAnchor.MiddleCenter;
		GUIStyles.LvlStyle.normal.textColor = Color.white;

		GUIStyles.ExpStyle = new GUIStyle();
		
		GUIStyles.ExpStyle.fontSize = 18;
		GUIStyles.ExpStyle.fontStyle = FontStyle.Bold;
		GUIStyles.ExpStyle.alignment = TextAnchor.MiddleCenter;
		GUIStyles.ExpStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f);

		GUIStyles.NewExpStyle = new GUIStyle();

		GUIStyles.NewExpStyle.fontSize = 18;
		GUIStyles.NewExpStyle.fontStyle = FontStyle.Bold;
		GUIStyles.NewExpStyle.alignment = TextAnchor.MiddleCenter;
		GUIStyles.NewExpStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f);

		lvlStripeStyle = new GUIStyle();

		lvlStripeStyle.normal.background = lvlStripe;
		
		texSize = 256;
		Vida = new Texture2D (texSize, texSize);
		Sed = new Texture2D (texSize, texSize);
		Hambre = new Texture2D (texSize, texSize);
		Energia = new Texture2D (texSize, texSize);
		
		GameGUI.Draw.Circle(Vida, CircleX, CircleY, Radius, AnguloVida, CoronaCircular,new Color(0.75f, 0, 0), true, texSize);
		Vida.Apply();
		
		GameGUI.Draw.Circle(Sed, CircleX-5, CircleY-3, Radius, AnguloSed, CoronaCircular,new Color(0, 0.75f, 0.75f), false, texSize);
		Sed.Apply();
		
		GameGUI.Draw.Circle(Hambre, CircleX, CircleY, Radius, AnguloHambre, CoronaCircular,new Color(0, 0, 0.75f), false, texSize);
		Hambre.Apply();
		
		GameGUI.Draw.Circle(Energia, CircleX+6, CircleY-4, Radius, AnguloEnergia, CoronaCircular,new Color(0, 0.75f, 0), true, texSize);
		Energia.Apply();

		finalColor = Color.white;
		
	}
	
	void OnGUI() 
	{

		if(EnableGUI) {

			if(!bigMap) {

				GUI.DrawTexture (new Rect (Screen.width - 266, 10, 256, 256), minimapFrame);

				GameObject.Find("Camera").camera.targetTexture = minimapTexture;

				if(Event.current.type == EventType.Repaint) {
					Graphics.DrawTexture (new Rect (Screen.width - 215, 61, 150, 150), minimapTexture, minimapMaterial);
				}
			
			} else {

				GameObject.Find("Camera").camera.targetTexture = bigmapTexture;

				if(Event.current.type == EventType.Repaint) {
					Graphics.DrawTexture (new Rect (Screen.width*0.1f, Screen.height*0.1f, Screen.width*0.8f, Screen.height*0.8f), bigmapTexture, bigmapMaterial);
				}

			}

			if(!bigMap) {

				string expString = (PlayerStats.Exp-LvlSys.GetExpfromLvl(PlayerStats.Lvl-1, true)).ToString("n0").Replace(",", ".")+"/"+(LvlSys.GetExpfromLvl(PlayerStats.Lvl, true)-LvlSys.GetExpfromLvl(PlayerStats.Lvl-1, true)).ToString("n0").Replace(",", ".")+" Exp";
				int numCharsExp = expString.Length;
				string spaceString = "";

				for(int i = 0; i < numCharsExp; i++) {
					spaceString += "   ";
				}

				GUI.DrawTexture(new Rect (10, 12, 328, 56), expBar);
				GUI.DrawTexture(new Rect (11, 13, (PlayerStats.Exp-LvlSys.GetExpfromLvl(PlayerStats.Lvl-1, true))*320/(LvlSys.GetExpfromLvl(PlayerStats.Lvl, true)-LvlSys.GetExpfromLvl(PlayerStats.Lvl-1, true)), 48), lvlStripe);
				GUI.DrawTexture(new Rect (0, 5, 70, 70), lvl);
				GUI.DrawTexture(new Rect (10, 75, 138, 162), itemHUD);
				GUI.Label(new Rect (3, 8, 60, 61), PlayerStats.Lvl.ToString(), GUIStyles.LvlStyle);
				GUI.Label(new Rect (10, 12, 322, 50), expString, GUIStyles.ExpStyle);
				GUI.Label(new Rect (10, 12, 322, 50), spaceString+LvlSys.newExp, GUIStyles.NewExpStyle);
				ShadowAndOutlineCS.DrawOutline(new Rect(158, 138, 100, 100), "$ "+PlayerStats.Money.ToString(), GUIStyles.MoneyStyle, Color.black, GUIStyles.MoneyColor, 15);
			}

			if(EnableGUI && !bigMap && ((Player.ThirdView && GunStats.IsAiming) || !Player.ThirdView)) {
				GUI.DrawTexture (new Rect (Screen.width/2-13, Screen.height/2-13, 27, 27), centerGrid);
			}

			if(!bigMap) {

				Matrix4x4 svMat = GUI.matrix;
			
				GUIUtility.RotateAroundPivot (90, new Vector2 (Screen.width - 140, 132));
				GUI.DrawTexture (new Rect (Screen.width - 256, 0, texSize, texSize), Hambre);

				GUIUtility.RotateAroundPivot (180, new Vector2 (Screen.width - 140, 132));
				GUI.DrawTexture (new Rect (Screen.width - 256, 0, texSize, texSize), Vida);

				GUIUtility.RotateAroundPivot (90, new Vector2 (Screen.width - 140, 132));
				GUI.DrawTexture (new Rect (Screen.width - 256, 0, texSize, texSize), Energia);

				GUIUtility.RotateAroundPivot (0, new Vector2 (Screen.width - 140, 132));
				GUI.DrawTexture (new Rect (Screen.width - 256, 0, texSize, texSize), Sed);

				GUI.matrix = svMat;

			}

		
		}

		if(pauseMenu) {

			GUI.depth = 2;

			GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			
			GUI.DrawTexture(new Rect(Screen.width/2-357, Screen.height/2-287, 720, 574), preInv);

			if(GUI.Button(new Rect(Screen.width/2-357+30, Screen.height/2-287+9, 102, 36), "Inventario", tabStyle)) {
				pauseMenu = true;
				currentPlace = GUIPlaces.Inv;
			}

			if(GUI.Button(new Rect(1*107+Screen.width/2-357+30, Screen.height/2-287+9, 102, 36), "Tienda", tabStyle)) {
				pauseMenu = true;
				currentPlace = GUIPlaces.shop;
			}

			if(GUI.Button(new Rect(2*107+Screen.width/2-357+30, Screen.height/2-287+9, 102, 36), "Ayudantes", tabStyle)) {
				pauseMenu = true;
				currentPlace = GUIPlaces.personal;
			}

			if(GUI.Button(new Rect(3*107+Screen.width/2-357+30, Screen.height/2-287+9, 102, 36), "Opciones", tabStyle)) {
				pauseMenu = true;
				currentPlace = GUIPlaces.settingsMenu;
			}

			switch(currentPlace) {

			case GUIPlaces.settingsMenu:

				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);

				if(GUI.Button(new Rect(Screen.width/2-200, Screen.height/2-287+75, 400, 50), "Volver al juego")) {
					
					currentPlace = GUIPlaces.none;
					subPlace = subPlaces.none;
					pauseMenu = false;
					
				}

				if(GUI.Button(new Rect(Screen.width/2-200, Screen.height/2-287+150, 400, 50), "Opciones")) {

					currentPlace = GUIPlaces.optionsMenu;
					pauseMenu = true;

				}

				if(GUI.Button(new Rect(Screen.width/2-200, Screen.height/2-287+225, 400, 50), "Salir")) {

					//ps.SaveGame(player, Profile.currentProfile.lastPlayedWorld);
					GameSaver.Save(Profile.currentProfile.lastPlayedWorld);
					mInput.SaveKeys();
					Profile.Save();
					Application.LoadLevel("Main");
					
				}

				break;


			case GUIPlaces.Inv:

				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);

				GUI.BeginGroup(new Rect(Screen.width/2-350+20, Screen.height/2-252+20, 660, 40));

				GUI.Box(new Rect(0, 0, 660, 40), "");

				if(GUI.Button(new Rect(10, 5, 100, 30), "Slots")) {
					subPlace = subPlaces.slots;
				}

				if(GUI.Button(new Rect(120, 5, 100, 30), "Crafting")) {
					subPlace = subPlaces.craft;
				}

				if(GUI.Button(new Rect(230, 5, 100, 30), "Skills")) {
					subPlace = subPlaces.skills;
				}

				if(Chests.isChestOpened) {
					if(GUI.Button(new Rect(340, 5, 100, 30), "Almacenaje")) {
						subPlace = subPlaces.storage;
					}
				}

				GUI.EndGroup();

				switch(subPlace) {

				case subPlaces.slots:

				inv2.Draw();

				Rect invHudRect = new Rect (Screen.width/2-350+275, Screen.height/2-252+35+50, 100, 150);

				if(Event.current.type == EventType.Repaint) {
					Graphics.DrawTexture (invHudRect, invHudTexture, invHudMaterial);
				}
				
					break;

				case subPlaces.craft:

					GUI.BeginGroup(new Rect(Screen.width/2-350+20, Screen.height/2-252+20+40, 660, 485));
					
					List<CraftingStations> availableCraftings = player.GetComponent<PCraftScript>().nearCraftingStation;

					for(int i = 0; i < availableCraftings.Count; i++) {
						if(GUI.Button(new Rect(5*i+64*i, 5, 64, 64), availableCraftings[i].myCraft.ToString())) {
							selectedCrafting = i;
						}
					}
					
					GUI.BeginGroup(new Rect(0, 75, 325, 370));
					
					GUI.Box(new Rect(0, 0, 325, 370), "");
					
					int[] craftableItems = availableCraftings[selectedCrafting].availableItems;
					
					craftScroll = GUI.VerticalSlider(new Rect(315, 0, 10, 370), craftScroll, 0, craftableItems.Length*84-350);
					
					GUI.BeginGroup(new Rect(0, -craftScroll, 325, craftableItems.Length*84+40));

					for(int i = 0; i < craftableItems.Length; i++) {
						InventoryItem item = ItemBase.MainBase.FirstOrDefault(x => x.id == craftableItems[i]);
						bool availableCraft = CraftSystem.CheckRecipeAvailability(craftableItems[i]); //I need to know if there are materials to use in crafting in the inventory or hotbar
						if(GUIExt.Hover(new Rect(5, 5+5*(i+1)+79*i, 315, 79)) && availableCraft) {
							GUI.Box(new Rect(5, 5+5*(i+1)+79*i, 305, 79), "");
							if(GUI.Button(new Rect(5, 5+5*(i+1)+79*i, 315, 79), "", GUI.skin.label)) {
								stacktocraft = 1;
								selectedCraftItem = craftableItems[i];
							}
						}
						string materials = "";
						RecipeMaterial[] recmats = CraftSystem.recipeList
							.FirstOrDefault(x => x.resultItemID == craftableItems[i]).materials;
						for(int j = 0; j < recmats.Length; j++) {
							materials += Inv.FindItem(recmats[j].itemID).DisplayName+" ("+recmats[j].Quantity+")"+((j < recmats.Length-1) ? ", " : "");
						}
						GUI.DrawTexture(new Rect(20, 20*(i+1)+64*i, 64, 64), item.itemtex);
						GUI.Label(new Rect(104, 20*(i+1)+64*i, 200, 64), item.DisplayName+"\n"+materials);
					}
					
					GUI.EndGroup();
					
					GUI.EndGroup();
					
					GUI.BeginGroup(new Rect(330, 75, 325, 370));
					
					GUI.Box(new Rect(0, 0, 325, 370), "");
					
					if(selectedCraftItem != 0) {
						
						InventoryItem item = ItemBase.MainBase.FirstOrDefault(x => x.id == selectedCraftItem);
						
						GUI.DrawTexture(new Rect(98, 20, 128, 128), item.itemtex);
						
						GUI.Label(new Rect(98, 168, 128, 50), "Item: "+item.DisplayName+"\n"+"Peso: "+item.weight+" Kg");

						List<RecipeMaterialsPosition> recmatpos = new List<RecipeMaterialsPosition>();
						int qnt = 1;
						bool possibleCrafting = CraftSystem.CheckRecipeAvailability(selectedCraftItem, stacktocraft, ref recmatpos, ref qnt);

						//I need to calculate the maximum possible items to craft
						stacktocraft = GUIExt.NumUpDown(new Rect(98, 238, 128, 20), stacktocraft, 1, qnt);

						if(GUI.Button(new Rect(98, 310, 128, 40), "Craftear") && possibleCrafting) {
							InventoryItem itemtoadd = Inv.NewItem(item);
							itemtoadd.itemstacksize = stacktocraft;
							if(Inv.AddItem(itemtoadd)) {
								for(int i = 0; i < recmatpos.Count; i++) {
									RecipeMaterialsPosition rmp = recmatpos[i];
									InventoryItem itemUsed = Inv.getSlotType(rmp.slottype)[rmp.slot];
									RecipeMaterial recmat = CraftSystem.recipeList
										.FirstOrDefault(x=>x.resultItemID==selectedCraftItem).materials
											.FirstOrDefault(x=>x.itemID==itemUsed.id);
									itemUsed.itemstacksize -= recmat.Quantity*stacktocraft;
								}
								selectedCraftItem = 0;
							}
						}
						
					}
					
					GUI.EndGroup();
					
					GUI.EndGroup();

					break;

				case subPlaces.skills:

					GUI.BeginGroup(new Rect(Screen.width/2-350+30, Screen.height/2-252+70, 680, 440));
					
					GUI.Box(new Rect(0, 0, 650, 50), "Puntos de Skill: "+PlayerStats.SkillPoints, new GUIStyle(GUI.skin.box) {fontSize = 16, alignment = TextAnchor.MiddleCenter});

					Skill[] skillArray = SkillSys.skills.ToArray();

					int SkillNum = skillArray.Length;

					SkillScroll = GUI.VerticalSlider(new Rect(652.5f, 60, 10, 375), SkillScroll, 0, 45*SkillNum-375);

					GUI.BeginGroup(new Rect(0, 60, 650, 375));

					GUI.BeginGroup(new Rect(0, -SkillScroll, 650, SkillNum*45+20));

					SkillSys.Draw();

					GUI.EndGroup();

					GUI.EndGroup();

					GUI.EndGroup();

					break;

				case subPlaces.storage:

					break;

				default:
					subPlace = subPlaces.slots;
					break;

				}


				break;

			case GUIPlaces.shop:

				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);

				GUI.BeginGroup(new Rect(Screen.width/2-350+20, Screen.height/2-252+20, 660, 485));
				
				GUI.Box(new Rect(0, 0, 660, 40), "");
				
				GUI.Button(new Rect(10, 5, 100, 30), "Comprar");
				
				GUI.Button(new Rect(120, 5, 100, 30), "Vender");

				List<NPC> unblockedNPCs = NPCSystem.unblockedNPCs;

				for(int i = 0; i < unblockedNPCs.Count; i++) {
					if(GUI.Button(new Rect(5*i+64*i, 45, 64, 64), unblockedNPCs[i].name)) {
						selectedShopNPC = i;
					}
				}

				GUI.BeginGroup(new Rect(0, 115, 325, 370));

				GUI.Box(new Rect(0, 0, 325, 370), "");

				int[] sellableItems = unblockedNPCs[selectedShopNPC].itemSellID;

				shopScroll = GUI.VerticalSlider(new Rect(315, 0, 10, 370), shopScroll, 0, sellableItems.Length*84-350);

				GUI.BeginGroup(new Rect(0, -shopScroll, 325, sellableItems.Length*84+40));

				for(int i = 0; i < sellableItems.Length; i++) {
					InventoryItem item = ItemBase.MainBase.FirstOrDefault(x => x.id == sellableItems[i]);
					if(GUIExt.Hover(new Rect(5, 5+5*(i+1)+79*i, 315, 79))) {
						GUI.Box(new Rect(5, 5+5*(i+1)+79*i, 305, 79), "");
						if(GUI.Button(new Rect(5, 5+5*(i+1)+79*i, 315, 79), "", GUI.skin.label)) { //new GUIStyle(GUI.skin.box) {fontStyle = FontStyle.Bold, fontSize = 15, normal = new GUIStyleState() {textColor = Color.white}}
							stacktobuy = 1;
							selectedShopItem = sellableItems[i];
						}
					}
					GUI.DrawTexture(new Rect(20, 20*(i+1)+64*i, 64, 64), item.itemtex);
					GUI.Label(new Rect(104, 20*(i+1)+64*i, 200, 64), item.DisplayName+"\n"+item.shopValue+" $");
				}

				GUI.EndGroup();

				GUI.EndGroup();

				GUI.BeginGroup(new Rect(330, 115, 325, 370));

				GUI.Box(new Rect(0, 0, 325, 370), "");

				if(selectedShopItem != 0) {

					InventoryItem item = ItemBase.MainBase.FirstOrDefault(x => x.id == selectedShopItem);

					GUI.DrawTexture(new Rect(98, 20, 128, 128), item.itemtex);

					GUI.Label(new Rect(98, 168, 128, 50), "Item: "+item.DisplayName+"\n"+"Peso: "+item.weight+" Kg\n"+"Precio: "+item.shopValue+" $");

					stacktobuy = GUIExt.NumUpDown(new Rect(98, 238, 128, 20), stacktobuy, 1, item.itemstacklimit);

					GUI.Label(new Rect(98, 264, 128, 20), "Precio final: "+(stacktobuy*item.shopValue)+" $", new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter});

					if(GUI.Button(new Rect(98, 310, 128, 40), "Comprar")) {
						if(PlayerStats.Money >= (ulong)(stacktobuy*item.shopValue)) {
							InventoryItem itemtoadd = Inv.NewItem(item);
							itemtoadd.itemstacksize = stacktobuy;
							if(Inv.AddItem(itemtoadd)) {
								PlayerStats.Money -= (ulong)(stacktobuy*item.shopValue);
							}
						} else {
							Debug.Log("You need more money!");
						}
					}

				}

				GUI.EndGroup();

				GUI.EndGroup();

				break;

			case GUIPlaces.personal:

				PersonalRowNum = Mathf.Floor(12/3);

				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);

				//MenuvScroll = GUI.VerticalSlider (new Rect (Screen.width/2-350+670, Screen.height/2-252+20, 10, 485), MenuvScroll, 0, 75*PersonalRowNum-480);

				GUIStyle npcBox = new GUIStyle(GUI.skin.button) {alignment = TextAnchor.MiddleRight, richText = true};

				GUI.BeginGroup(new Rect(Screen.width/2-350+20, Screen.height/2-252+20, 640, 485));

				GUI.Box(new Rect(0, 0, 640, 485), "");
				
				GUI.BeginGroup (new Rect (0, -MenuvScroll, 640, PersonalRowNum * 75+20));

				//Hacer que cuando se adquieran distintos tipos de NPC se desbloques nuevas categorias en la tienda

				for(int i = 0; i < NPCSystem.npcs.Count; i++) {
					GUI.Button(new Rect(10*(((i+1)%3)+1)+200*((i+1)%3), 5*(((Mathf.Floor(i/3)-1)<0) ? 0 : (Mathf.Floor(i/3)-1))+5*(Mathf.Floor(i/3)+1)+65*Mathf.Floor(i/3), 200, 65), "<size=14>"+NPCSystem.npcs[i].name+"</size>"+Environment.NewLine+"<size=12>"+NPCSystem.npcs[i].description+"</size>", npcBox);
				}
				
				/*GUI.Button(new Rect(10, 5, 200, 65), "<size=14>Artillero</size>"+Environment.NewLine+"<size=12>Vende armas de fuego.</size>", npcBox);
				
				GUI.Button(new Rect(220, 5, 200, 65), "<size=14>Herrero</size>"+Environment.NewLine+"<size=12>Vende y mejora armaduras y armas.</size>", npcBox); 
				
				GUI.Button(new Rect(430, 5, 200, 65), "<size=14>Mecánico</size>"+Environment.NewLine+"<size=12>Arregla y vende piezas de coche.</size>", npcBox); //Vende armas y munción

				GUI.Button(new Rect(10, 75, 200, 65), "<size=14>Cocinero</size>"+Environment.NewLine+"<size=12>Vende alimentos procesados.</size>", npcBox); //Reparar items y mejorarlos
				
				GUI.Button(new Rect(220, 75, 200, 65), "<size=14>Granjero</size>"+Environment.NewLine+"<size=12>Vende productos del campo.</size>", npcBox); //Reparar cosas mecanicas y mejorarlas
				
				GUI.Button(new Rect(430, 75, 200, 65), "<size=14>Doctor</size>"+Environment.NewLine+"<size=12>Cura y vende medicamentos.</size>", npcBox); //Ingeniero, pero no me gusta la idea //Mejor alguien que venda accesorios

				GUI.Button(new Rect(10, 150, 200, 65), "<size=14>Mago</size>"+Environment.NewLine+"<size=12>Vende y mejora armas mágicas.</size>", npcBox); //Restaura tus stats (vida, hambre, sed, energia)
				
				GUI.Button(new Rect(220, 150, 200, 65), "<size=14>Mercader</size>"+Environment.NewLine+"<size=12>Vende productos básicos.</size>", npcBox); //Para comprar comida
				
				GUI.Button(new Rect(430, 150, 200, 65), "<size=14>Minero</size>"+Environment.NewLine+"<size=12>Recolecta y vende materiales.</size>", npcBox); //Para cultivar semillas, cuidar ganado y recoger el fruto

				GUI.Button(new Rect(10, 225, 200, 65), "<size=14>Estilista</size>"+Environment.NewLine+"<size=12>Vende objetos de apariencia.</size>", npcBox); //Comprar, mejorar y reparar cosas mágicas
				
				GUI.Button(new Rect(220, 225, 200, 65), "<size=14>Militar</size>"+Environment.NewLine+"<size=12>Vende objetos de defensa personal.</size>", npcBox);
				
				GUI.Button(new Rect(430, 225, 200, 65), "??? (Moar incoming)", npcBox);*/ //Reparar cosas mágicas
				                       
				//Faltan: un cirujano? (para cambiar tus stats rapidamente), un estilista para comprar y cambiar el color de la ropa, y alguien para cambiar el color de tu piel, skin, pelo (+ su tipo), etc
				//También un pintor (para los tintes y pinturas), un carpintero/albañil/contructor (para la venta de crafting, hornos, yunques y demás)
				//Poner un minero para poder obtener bloques, minerales y recursos

				GUI.EndGroup();

				GUI.EndGroup();

				break;

			case GUIPlaces.optionsMenu:
				
				GUI.DrawTexture(new Rect(Screen.width/2-350, Screen.height/2-252, 700, 525), preTab);

				if(!Options.Draw("")) {
					currentPlace = GUIPlaces.settingsMenu;
				}

				break;

			default:
				currentPlace = GUIPlaces.none;
				pauseMenu = false;
				break;

			}

			GUI.EndGroup();
			
			GUI.depth = 0;

		}

		if(DebugScreen) {

			#if UNITY_EDITOR
			
			GUI.Label(new Rect(10, 10, Screen.width/2-10, Screen.height), GameGUI.DebugScreen.DScreen(), GUIStyles.DebugStyle);
			GUI.Label(new Rect(Screen.width/2, 10, Screen.width/2-10, Screen.height), GameGUI.DebugScreen.DebugVScreen(), GUIStyles.DebugVStyle);

			#endif

			GUI.Label(new Rect(10, 10, Screen.width/2-10, Screen.height), GameGUI.DebugScreen.DebugScreenLite(), GUIStyles.DebugStyle);
			GUI.Label(new Rect(Screen.width/2, 10, Screen.width/2-10, Screen.height), GameGUI.DebugScreen.DebugVScreenLite(), GUIStyles.DebugVStyle);

		}

		if(currentPlace == GUIPlaces.waypointCreator) {

			//Create a waypoint

			float tempRed = wpRed;
			float tempGreen = wpGreen;
			float tempBlue = wpBlue;

			GUIStyle waypointTxtFldStyle = GUI.skin.GetStyle("TextField");

			waypointTxtFldStyle.fontSize = 20;
			waypointTxtFldStyle.normal.textColor = Color.white;
			waypointTxtFldStyle.alignment = TextAnchor.MiddleCenter;

			GUIStyle waypointLabels = GUI.skin.GetStyle("Label");

			waypointLabels.normal.textColor = Color.white;
			waypointLabels.alignment = TextAnchor.MiddleCenter;
			waypointLabels.fontSize = 16;

			Vector2 pad = new Vector2(Screen.width/2-506/2, Screen.height/2-506/2);

			GUIExt.Window("Crear Waypoint", pad);

			GUI.Label(new Rect(pad.x, pad.y+23+10, 506, 30), "Nombre del Waypoint", waypointLabels);

			waypointName = GUI.TextField(new Rect(pad.x+8, pad.y+23+10+30, 490, 30), waypointName, waypointTxtFldStyle);

			GUI.Label(new Rect(pad.x, pad.y+23+10+30+30, 506, 30), "Coordenadas", waypointLabels);

			GUI.Box(new Rect(pad.x+4, pad.y+23+10+30+30+30+5-2, 498, 30+4), "");
			GUI.Box(new Rect(pad.x+4, pad.y+23+10+30+30+30+30+10-2, 498, 30+4), "");
			GUI.Box(new Rect(pad.x+4, pad.y+23+10+30+30+30+30+30+15-2, 498, 30+4), "");

			waypointLabels.alignment = TextAnchor.MiddleLeft;

			string wpxLabel = "xCoord:";
			string wpyLabel = "yCoord:";
			string wpzLabel = "zCoord:";

			if(wpxCoordStr == Mathf.Round(player.transform.position.x).ToString("F0")) {
				wpxLabel += " (Actual)";
			}

			if(wpyCoordStr == Mathf.Round(player.transform.position.y).ToString("F0")) {
				wpyLabel += " (Actual)";
			}

			if(wpzCoordStr == Mathf.Round(player.transform.position.z).ToString("F0")) {
				wpzLabel += " (Actual)";
			}

			GUI.Label(new Rect(pad.x+10, pad.y+23+10+30+30+30+5, 350, 30), wpxLabel, waypointLabels);
			GUI.Label(new Rect(pad.x+10, pad.y+23+10+30+30+30+30+10, 350, 30), wpyLabel, waypointLabels);
			GUI.Label(new Rect(pad.x+10, pad.y+23+10+30+30+30+30+30+15, 350, 30), wpzLabel, waypointLabels);

			waypointLabels.alignment = TextAnchor.MiddleCenter;

			wpxCoordStr = GUI.TextField(new Rect(pad.x+350+20, pad.y+23+10+30+30+30+5, 130, 30), wpxCoordStr, waypointTxtFldStyle);
			wpyCoordStr = GUI.TextField(new Rect(pad.x+350+20, pad.y+23+10+30+30+30+30+10, 130, 30), wpyCoordStr, waypointTxtFldStyle);
			wpzCoordStr = GUI.TextField(new Rect(pad.x+350+20, pad.y+23+10+30+30+30+30+30+15, 130, 30), wpzCoordStr, waypointTxtFldStyle);

			wpxCoordStr = Regex.Replace(wpxCoordStr, @"[^0-9.\-]", "");
			wpyCoordStr = Regex.Replace(wpyCoordStr, @"[^0-9.\-]", "");
			wpzCoordStr = Regex.Replace(wpzCoordStr, @"[^0-9.\-]", "");

			string wpcolorpstr = "";

			if(wpColorPalette) {
				wpcolorpstr = "Activado";
			} else {
				wpcolorpstr = "Desactivado";
			}

			if(GUI.Button(new Rect(pad.x+253-150, pad.y+23+10+180+15+10, 300, 50), "Colores avanzados: "+wpcolorpstr)) {
				wpColorPalette = (wpColorPalette == false) ? true : false;
			}

			if(!wpColorPalette) {

				GUI.Label(new Rect(pad.x, pad.y+23+10+180+15+5+60, 506, 30), "Color", waypointLabels);

				waypointLabels.alignment = TextAnchor.UpperLeft;

				GUI.Label(new Rect(pad.x+10, pad.y+23+10+210+15+5+10+60, 100, 30), "Rojo ("+(wpRed*255).ToString("F0")+")");
				GUI.Label(new Rect(pad.x+10, pad.y+23+10+240+15+10+10+60, 100, 30), "Verde ("+(wpGreen*255).ToString("F0")+")");
				GUI.Label(new Rect(pad.x+10, pad.y+23+10+270+15+15+10+60, 100, 30), "Azul ("+(wpBlue*255).ToString("F0")+")");

				waypointLabels.alignment = TextAnchor.MiddleCenter;

				wpRed = GUI.HorizontalSlider(new Rect(pad.x+10+100+10, pad.y+23+10+210+15+5+15+60, 246, 30), wpRed, 0, 1);
				wpGreen = GUI.HorizontalSlider(new Rect(pad.x+10+100+10, pad.y+23+10+240+15+10+15+60, 246, 30), wpGreen, 0, 1);
				wpBlue = GUI.HorizontalSlider(new Rect(pad.x+10+100+10, pad.y+23+10+270+15+15+15+60, 246, 30), wpBlue, 0, 1);

				finalColor = new Color(wpRed, wpGreen, wpBlue, 1);

				if(wpRed != tempRed || wpGreen != tempGreen || wpBlue != tempBlue) {
					wpColorPreview.SetPixel(0, 0, finalColor);
					wpColorPreview.Apply();
				}

				GUI.DrawTexture(new Rect(pad.x+506-120-10, pad.y+23+10+210+15+5+10+60, 120, 120), wpColorPreview);

			} else {

				GUI.Label(new Rect(pad.x+506-150-10, pad.y+23+10+180+15+5+60, 100, 30), "Color", waypointLabels);

				Texture2D palette = Resources.Load<Texture2D>("images/Palette");

				GUI.DrawTexture(new Rect(pad.x+10+50+23, pad.y+23+10+180+15+60, 258, 200), palette);

				if(GUI.Button(new Rect(pad.x+10+50+23, pad.y+23+10+180+15+60, 258, 200), "", GUI.skin.label)) {

					Vector2 mouseFix = Event.current.mousePosition;
					Vector2 finalpos = new Vector2(mouseFix.x-pad.x-60-23, mouseFix.y-pad.y-288);
				
					finalColor = palette.GetPixel((int)finalpos.x, palette.height-(int)finalpos.y);

					wpColorPreview.SetPixel(0, 0, finalColor);
					wpColorPreview.Apply();

				}

				GUI.DrawTexture(new Rect(pad.x+506-150-10, pad.y+23+10+180+15+5+60+40, 120, 120), wpColorPreview);

			}

			if(GUI.Button(new Rect(pad.x+253-100-10, pad.y+506-30-10+23, 100, 30), "Crear")) {
				if(WaypointLib.waypointList == null) {
					WaypointLib.waypointList = new System.Collections.Generic.List<Waypoint>();
				}

				int i = GameObject.FindGameObjectsWithTag("Waypoint").Length;

				GameObject waypointText = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Waypoint"), new Vector3(float.Parse(wpxCoordStr), float.Parse(wpyCoordStr), float.Parse(wpzCoordStr)), Quaternion.identity);
				waypointText.name = "Waypoint"+i;
				waypointText.tag = "Waypoint";

				waypointText.AddComponent<WaypointIndexer>().WaypointName = waypointName;
				
				Texture2D navtexture = Resources.Load<Texture2D>("images/waypoint");
				Texture2D finalTexture = navtexture;
				finalTexture = TextureExt.ReplaceColor(navtexture, Color.black, finalColor);
				
				finalTexture.filterMode = FilterMode.Point;
				finalTexture.anisoLevel = 0;
				
				waypointText.transform.FindChild("WaypointText").FindChild("Point").GetComponent<MeshRenderer>().materials[0].mainTexture = finalTexture;

				WaypointLib.waypointList.Add(new Waypoint() {pos = new Vector_3() {x = float.Parse(wpxCoordStr), y = float.Parse(wpyCoordStr), z = float.Parse(wpzCoordStr)}, name = waypointName, color = finalColor});
				currentPlace = GUIPlaces.none;
				pauseMenu = false;
			}

			if(GUI.Button(new Rect(pad.x+253+10, pad.y+506-30-10+23, 100, 30), "Cancelar")) {
				currentPlace = GUIPlaces.none;
				pauseMenu = false;
			}

		}

		if(PlayerStats.Killed) {

			GUI.depth = 2;

			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), RedScreen);
			
			GUI.Label(new Rect(Screen.width/2-250, 25, 500, 50), "Has muerto.", GUIStyles.DieStyle);

			ps.Die();

			if(GUI.Button(new Rect(Screen.width/2-50, 100, 100, 30), "Reespawnear")) {
				ps.Respawn();
			}

			GUI.depth = 0;
			
		}

	}

	float r = 2;
	float theta = 0;
	
	private float heldedTime;
	private bool firsttimeHelded;

	// Update is called once per frame
	void Update () 
	{

		Menus.pauseMenu = pauseMenu;
		Menus.WaypointCreator = currentPlace == GUIPlaces.waypointCreator;

		/*if(GameGUI.GUIOptions.GetSensitivity() != 0 && (currentPlace == GUIPlaces.optionsMenu || currentPlace == GUIPlaces.settingsMenu || currentPlace == GUIPlaces.waypointCreator || PlayerStats.Killed)) {
			GameGUI.GUIOptions.SetSensivity(Options.InvertMouse, 0);
			Screen.lockCursor = false;
		}*/

		if(Input.GetKey(mInput.GetKey("Exp"))) {
			PlayerStats.Exp += 200;
		}

		if(Input.GetKey(mInput.GetKey("Kill")) && !PlayerStats.invincible) {
			PlayerStats.Health = 0;
		}

		//bool DebugOnGui = transform.GetComponent<RetrieveDebug>().enabled;

		/*if(Input.GetKeyDown(KeyCode.F4) && !PlayerStats.Killed) {
			transform.GetComponent<RetrieveDebug>().enabled = (DebugOnGui == false) ? true : false;
		}*/

		if(Input.GetMouseButton (0)) {
			if(!firsttimeHelded) {
				firsttimeHelded = true;
				heldedTime = Time.time;
			}

			if(heldedTime > Time.time + 1) {
				MouseProperties.mouseHelded = true;
			}

		} else {
			MouseProperties.mouseHelded = false;
		}

		if(Input.GetMouseButtonUp(0)) 
		{
			firsttimeHelded = false;
			MouseProperties.mouseHelded = true;
		}

		if(Input.GetKeyDown (mInput.GetKey("FPS"))) 
		{
			DebugScreen = (DebugScreen == true) ? false : true;
		}

		AnguloVida = (float)(int)(PlayerStats.Health*90/PlayerStats.MaxHealth); //pHealth.health*90/pHealth.maxHealth;

		if(AnguloVida != lastVida) {
			GameGUI.Draw.Circle(Vida, CircleX, CircleY, Radius, AnguloVida, CoronaCircular, new Color(0.75f, 0, 0), true, texSize);
			Vida.Apply();
			lastVida = AnguloVida;
		}

		AnguloSed = (float)(int)(PlayerStats.Thirst*90/PlayerStats.MaxThirst);
		
		if(AnguloSed != lastSed) {
			GameGUI.Draw.Circle(Sed, CircleX-5, CircleY-3, Radius, AnguloSed, CoronaCircular, new Color(0, 0.75f, 0.75f), false, texSize);
			Sed.Apply();
			lastSed = AnguloSed;
		}
		
		AnguloHambre = (float)(int)(PlayerStats.Hunger*90/PlayerStats.MaxHunger);
		
		if(AnguloHambre != lastHambre) {
			GameGUI.Draw.Circle(Hambre, CircleX, CircleY, Radius, AnguloHambre, CoronaCircular, new Color(0, 0, 0.75f), false, texSize);
			Hambre.Apply();
			lastHambre = AnguloHambre;
		}

		AnguloEnergia = (float)(int)(PlayerStats.Energy*90/PlayerStats.MaxEnergy);

		if(AnguloEnergia != lastEnergia) {
			GameGUI.Draw.Circle(Energia, CircleX+6, CircleY-4, Radius, AnguloEnergia, CoronaCircular, new Color(0, 0.75f, 0), true, texSize);
			Energia.Apply();
			lastEnergia = AnguloEnergia;
		}

		mousePosFix = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);

		Rect invHudRect = new Rect (Screen.width/2-350+275, Screen.height/2-252+35, 100, 150);

		float x0 = player.transform.position.x;
		float z0 = player.transform.position.z;

		float x = x0 + r * Mathf.Cos(theta * Mathf.PI / 180);
		float z = z0 + r * Mathf.Sin(theta * Mathf.PI / 180);
		
		if(currentPlace == GUIPlaces.Inv && invHudRect.Contains(mousePosFix) && Input.GetMouseButton(0)) {
			theta = (mousePosFix.x - invHudRect.x) * invHudRect.width / 20; //27.5f;
		}

		cCam.transform.position = new Vector3 (x, player.transform.position.y, z);

		cCam.transform.LookAt (player.transform);

		if(Input.GetKeyDown(mInput.GetKey("EnableGUI")) && !PlayerStats.Killed) {
			EnableGUI = (EnableGUI == false) ? true : false;		
		}

		if(Input.GetKeyDown(mInput.GetKey("GameMenu")) && !PlayerStats.Killed) {

			//Ternary
			if(currentPlace == GUIPlaces.waypointCreator) {
				currentPlace = GUIPlaces.none;
				pauseMenu = false;
			} else {
				if(currentPlace != GUIPlaces.settingsMenu) {
					pauseMenu = true;
					currentPlace = GUIPlaces.settingsMenu;
				} else {
					pauseMenu = false;
					currentPlace = GUIPlaces.none;
				}
			}

		}

		if(Input.GetKeyDown (mInput.GetKey("Inv")) && !PlayerStats.Killed) {
			//Ternary
			if(currentPlace != GUIPlaces.Inv) {
				pauseMenu = true;
				currentPlace = GUIPlaces.Inv;
			} else {
				pauseMenu = false;
				currentPlace = GUIPlaces.none;
			}
		}

		if(Input.GetKeyDown(mInput.GetKey("Waypoint")) && !PlayerStats.Killed && !pauseMenu) {

			waypointName = "";

			currentPlace = (currentPlace == GUIPlaces.waypointCreator) ? GUIPlaces.none : GUIPlaces.waypointCreator;
			wpxCoordStr = Mathf.Round(player.transform.position.x).ToString("F0");
			wpyCoordStr = Mathf.Round(player.transform.position.y).ToString("F0");
			wpzCoordStr = Mathf.Round(player.transform.position.z).ToString("F0");

			Random color = new Random();

			wpRed = (float)color.NextDouble();
			wpGreen = (float)color.NextDouble();
			wpBlue = (float)color.NextDouble();

			Color finalColor = new Color(wpRed, wpGreen, wpBlue, 1);
			wpColorPreview.SetPixel(0, 0, finalColor);
			wpColorPreview.Apply();

		}

		if(currentPlace == GUIPlaces.optionsMenu || currentPlace == GUIPlaces.settingsMenu || currentPlace == GUIPlaces.waypointCreator) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}

		if(Input.GetKeyDown(mInput.GetKey("ToggleMap")) && !PlayerStats.Killed && currentPlace != GUIPlaces.waypointCreator) {
			bigMap = (bigMap == false) ? true : false;
			if(bigMap) {
				Options.MinimapZoom = 1000;
			} else {
				Options.MinimapZoom = tmpZoom;
			}
		}

	}

}
