using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using env = System.Environment;

//Weapon for melee
//Gun for ranged

public enum ItemList {Rock, Stone, Log, Branch, Stick, Wood, Leaf, Sapling, Dirt, Grass, Sand, Glass, Snow, Ice, Snowball, Gravel, Flint, Lime, Lime_Dust, Stucco, Mud, Sulfur, Sulfur_Dust, Saltpeter, Saltpeter_Dust, Powder, Resin, Plastic, Rubber, Glue, Coal, Charcoal, Clay, Brick, Feather, Brain, Zombie_Meat, Bone, Fang, Antler, Skull, Spine, Cobweb, String, Silk, Wool, Cloth, Rope, Leather, Ink, Wheat, Flour, Egg, Sugar, Sweet_Powder, Cactus, Bamboo, Bamboo_Splinter, Paper, Cardboard, Plywood, M9 = 64, SawnOff = 65, MP5 = 66, Bullets32 = 128, Gaugue20 = 140}

public enum SlotsTypes {None, Accesories, HotBar, Ammo, Inventory, Equip};
                                                                     //Tools
public enum ItemType {None, Gun, Ammo, Weapon, Money, Equip, Accesory, Kits, Block, Explosives, Bag}
//public enum EquipType {None, Head, Shirt, Pants, Boots}
public enum UseType {None, Consume, Potion, Repair, Fix}

public class InventoryItem
{
	public GameObject worldObject;
	public int id;
	public string itemname;
	public string DisplayName;
	public string Description;
	public Texture itemtex;
	public ItemType itemtype; //Gun, ammo, weapon, money, equip, accesory, explosives (Hacer un enum).
	public UseType usable;
	public float weight;
	public bool droppable;
	//public SlotsTypes slottype = SlotsTypes.None; //If it item is a lot which type I'm?
	//public Transform itemmodel;
	
	public int itemstacksize = 1;
	public int itemstacklimit = 1;
	//public bool divible; //Can be divided into single stacks?
	public bool showStack; //Show stack number on the corner
	
	/*public int bagsize;
	public bool showBag;
	public InventoryItem[] BagItem;*/

	//Third Person custom Pos, Rot and Scl
	public Vector3 TcustomPostion = Vector3.zero;
	public Vector3 TcustomRotation = Vector3.zero;
	public Vector3 TcustomScale = Vector3.zero;

	//First Person custom Pos, Rot and Scl
	public Vector3 FcustomPostion = Vector3.zero;
	public Vector3 FcustomRotation = Vector3.zero;
	public Vector3 FcustomScale = Vector3.zero;
	
	public GameObject dropObject;

	public Hands hands;

	public object indexedClass;

	public int shopValue;

	public string strGUI;

}

public class SafeInvItem
{
	
	public string worldObject;
	public int id;
	public string itemname = "";
	public string DisplayName = "";
	public string Description = "";
	public string itemtex = "";
	public ItemType itemtype; 
	public UseType usable;
	public float weight;
	public bool droppable = true;
	
	public int itemstacksize = 1;
	public int itemstacklimit = 1;
	public bool showStack = true;
	
	public Vector_3 TcustomPostion;
	public Vector_3 TcustomRotation;
	public Vector_3 TcustomScale;
	
	public Vector_3 FcustomPostion;
	public Vector_3 FcustomRotation;
	public Vector_3 FcustomScale;
	
	public string dropObject;
	
	public int shopValue;
	
}

public class ItemBase {

	public static InventoryItem[] MainBase {get; set;}

	public static bool ItemsLoaded;

}

[XmlType(Namespace = "Inventory Items",
         TypeName = "Inventory")]
public class SafeItemBase {

	public SafeItemBase() {}

	public void Load() {
		if(myBase == null) {
			myBase = new List<SafeInvItem>();
			
			for(int i = 0; i < ItemBase.MainBase.Length; i++) {
				myBase.Add(Inv.ConvertToSafe(ItemBase.MainBase[i]));
			}
			
			allBase = myBase;
		} else {
			myBase = allBase;
		}
	}

	[XmlElement("Items")]
	public List<SafeInvItem> myBase {get; set;}

	public static List<SafeInvItem> allBase {get; set;}

	public static SafeItemBase ExternalLoad() {
		return XMLTools.DeserializeFromFile<SafeItemBase>(Application.dataPath + "/ItemBase.xml");
	}

}

public class Slots {

	public static float slotWeightSupport = 0.8f;

	private static InventoryItem[] _HBSlots;
	private static InventoryItem[] _InvSlots;
	private static InventoryItem[] _ASlots;
	private static InventoryItem[] _AccSlots;
	private static InventoryItem[] _EquipedItem;

	private static InventoryItem[,] _WorldChests;

	public static InventoryItem[] HotBarSlots {
		get {return _HBSlots;}
		set {_HBSlots = value;}
	}

	public static InventoryItem[] InventorySlots {
		get {return _InvSlots;}
		set {_InvSlots = value;}
	}

	public static InventoryItem[] AmmoSlots {
		get {return _ASlots;}
		set {_ASlots = value;}
	}

	public static InventoryItem[] Accesories {
		get {return _AccSlots;}
		set {_AccSlots = value;}
	}

	public static InventoryItem[] EquipedItem {
		get {return _EquipedItem;}
		set {_EquipedItem = value;}
	}

	public static InventoryItem[,] WorldChests {
		get {return _WorldChests;}
		set {_WorldChests = value;}
	}

	public static float hotBarMaxWeight {
		get { return _HBSlots.Length * slotWeightSupport;}
	}

	public static float inventoryMaxWeight {
		get { return _InvSlots.Length * slotWeightSupport;}
	}

	public static float ammoSlotsMaxWeight {
		get { return _ASlots.Length * slotWeightSupport;}
	}

	public static float accSlotsMaxWeight {
		get { return _AccSlots.Length * slotWeightSupport;}
	}

	public static float MaxSupport {
		get { return (hotBarMaxWeight+inventoryMaxWeight)*((SkillSys.SearchSkillByName("stronger").currentLevel+1)*1.1f);}
	}

	private static Rect _HBSlotsRect;
	private static Rect _InvSlotsRect;
	private static Rect _ASlotsRect;
	private static Rect _AccSlotsRect;

	public static Rect HotBarSlotsRect {
		get {return _HBSlotsRect;}
		set {_HBSlotsRect = value;}
	}
	
	public static Rect InventorySlotsRect {
		get {return _InvSlotsRect;}
		set {_InvSlotsRect = value;}
	}
	
	public static Rect AmmoSlotsRect {
		get {return _ASlotsRect;}
		set {_ASlotsRect = value;}
	}
	
	public static Rect AccesoriesRect {
		get {return _AccSlotsRect;}
		set {_AccSlotsRect = value;}
	}

	public static void Set(SafeSlots slotsToConvert) {
		/*if(slotsToConvert == null) {
			return;
		}*/
		for(int i = 0; i < Accesories.Length; i++) {
			if(slotsToConvert.Accesories[i] != null)
				Accesories[i] = Inv.ConvertToUnsafe(slotsToConvert.Accesories[i]);
		}
		for(int i = 0; i < AmmoSlots.Length; i++) {
			if(slotsToConvert.AmmoSlots[i] != null)
				AmmoSlots[i] = Inv.ConvertToUnsafe(slotsToConvert.AmmoSlots[i]);
		}
		for(int i = 0; i < HotBarSlots.Length; i++) {
			if(slotsToConvert.HotBarSlots[i] != null)
				HotBarSlots[i] = Inv.ConvertToUnsafe(slotsToConvert.HotBarSlots[i]);
		}
		for(int i = 0; i < InventorySlots.Length; i++) {
			if(slotsToConvert.InventorySlots[i] != null)
				InventorySlots[i] = Inv.ConvertToUnsafe(slotsToConvert.InventorySlots[i]);
		}
	}

}

public class SafeSlots {
	
	public SafeInvItem[] HotBarSlots {get; set;}
	
	public SafeInvItem[] InventorySlots {get; set;}
	
	public SafeInvItem[] AmmoSlots {get; set;}
	
	public SafeInvItem[] Accesories {get; set;}

	public static InventoryItem[] EquipedItem {get; set;}
	
	public static InventoryItem[,] WorldChests {get; set;}

	public static SafeSlots Get() {
		SafeSlots returnSlots = new SafeSlots();
		returnSlots.Accesories = new SafeInvItem[Slots.Accesories.Length];
		for(int i = 0; i < returnSlots.Accesories.Length; i++) {
			if(Slots.Accesories[i] != null)
				returnSlots.Accesories[i] = Inv.ConvertToSafe(Slots.Accesories[i]);
		}
		returnSlots.AmmoSlots = new SafeInvItem[Slots.AmmoSlots.Length];
		for(int i = 0; i < returnSlots.AmmoSlots.Length; i++) {
			if(Slots.AmmoSlots[i] != null)
				returnSlots.AmmoSlots[i] = Inv.ConvertToSafe(Slots.AmmoSlots[i]);
		}
		//Equiped
		returnSlots.HotBarSlots = new SafeInvItem[Slots.HotBarSlots.Length];
		for(int i = 0; i < returnSlots.HotBarSlots.Length; i++) {
			if(Slots.HotBarSlots[i] != null)
				returnSlots.HotBarSlots[i] = Inv.ConvertToSafe(Slots.HotBarSlots[i]);
		}
		returnSlots.InventorySlots = new SafeInvItem[Slots.InventorySlots.Length];
		for(int i = 0; i < returnSlots.InventorySlots.Length; i++) {
			if(Slots.InventorySlots[i] != null) 
				returnSlots.InventorySlots[i] = Inv.ConvertToSafe(Slots.InventorySlots[i]);
		}
		return returnSlots;
	}

	public static SafeSlots GetDefaultSlots() {
		Inv inventory = new Inv();
		inventory.InitialLoad();
		SafeSlots tempSlots = Get();
		Slots.Accesories = null;
		Slots.AmmoSlots = null;;
		Slots.HotBarSlots = null;
		Slots.InventorySlots = null;
		return tempSlots;
	}
	
}

public class SelectedSlot {

	private static int _SSlot;
	private static GameObject _carriedObj;

	public static int SSlot {
		get {return _SSlot;}
		set {_SSlot = value;}
	}

	public static GameObject CarriedObject {
		get {return _carriedObj;}
		set {_carriedObj = value;}
	}

	public static string cName;
	public static int lastSlot;
	public static bool switchedPer;

}

public class RegItem {

	private static float _itemCount = 0;

	public static float itemCount {
		get {return _itemCount;}
		set {_itemCount = value;}
	}

}

public class Chests {
	public static bool isChestOpened;
	public static int openedChestIndex;
}

public class Inv {

	//Static access to this
	public static Inv me;

	public static SafeSlots safeInvSlots;

	public static InventoryItem FindItem(int id) {

		InventoryItem returnvalue = null;
		
		try {
			returnvalue = ItemBase.MainBase.FirstOrDefault(x => x.id == id);
		} catch(System.NullReferenceException e) {
			Debug.Log(e+"; ID = "+id);
		}
		
		return returnvalue;

	}

	private GameObject LoadItemPrefab(string name) {
		return (GameObject)Resources.Load("prefabs/Items/"+name);
	}
	
	private Texture LoadItemTex(string name) {
		return (Texture)Resources.Load("textures/Items/"+name);
	}

	private GameObject LoadItemModel(string name) {
		return (GameObject)Resources.Load ("models/Items/"+name);
	}
	
	public InventoryItem[] Load() {

		Inv.me = this;

		List<InventoryItem> localItemBase = new List<InventoryItem>();

		List<SafeInvItem> safeItems = SafeItemBase.ExternalLoad().myBase;

		foreach(SafeInvItem safeItem in safeItems) {
			localItemBase.Add(Inv.ConvertToUnsafe(safeItem));
		}

		/*InventoryItem preset;

		preset = new InventoryItem() {id = 64, itemname = "M9", DisplayName = "M9", itemtype = ItemType.Gun, droppable = true, itemtex = LoadItemTex("Guns/M9"), worldObject = LoadItemModel("Guns/M9/M9"), FcustomRotation = new Vector3(270, 0, 0), FcustomScale = new Vector3 (1,1,1), TcustomPostion = new Vector3 (0.1158914f, 0.003035218f, 0.114579f), TcustomRotation = new Vector3(0, 270, 70), TcustomScale = new Vector3 (1,1,1)};

		localItemBase.Add(preset);

		preset = new InventoryItem() {id = 65, itemname = "SawnOff", DisplayName = "Sawn-Off Shotgun", itemtype = ItemType.Gun, droppable = true, itemtex = LoadItemTex ("Guns/SawnOff"), worldObject = LoadItemModel ("Guns/SawnOff/SawnOff"), TcustomPostion = new Vector3 (-0.1256181f, 0.1603842f, -0.1737731f), TcustomRotation = new Vector3(44, 33, 296), TcustomScale = new Vector3 (0.07f, 0.07f, 0.05f), FcustomPostion = new Vector3(0.3671382f, 9241129f, 0.5188492f), FcustomRotation = new Vector3(0, 185, 0), FcustomScale = new Vector3 (0.07f, 0.07f, 0.05f), dropObject = LoadItemModel ("Guns/SawnOff/SawnOff")};

		localItemBase.Add(preset);

		preset = new InventoryItem() {id = 66, itemname = "MP5", DisplayName = "MP5", itemtype = ItemType.Gun, droppable = true, itemtex = LoadItemTex ("Guns/MP5"), worldObject = LoadItemModel ("Guns/MP5/MP5"), TcustomPostion = new Vector3 (-0.03994608f, -0.01932887f, -0.1486245f), TcustomRotation = new Vector3(21, 12, 103), TcustomScale = new Vector3 (0.5f, 0.5f, 0.5f), FcustomRotation = new Vector3(0, -180, -180), FcustomScale = new Vector3 (0.5f, 0.5f, 0.5f), dropObject = LoadItemModel ("Guns/MP5/MP5"), weight = 1};
		
		localItemBase.Add(preset);
		
		preset = new InventoryItem() {id = 128, itemname = "32_Bullets", DisplayName = "32 Bullets", itemtype = ItemType.Ammo, droppable = true, itemtex = LoadItemTex ("Bullets/32_Bullets"), worldObject = LoadItemModel ("Bullets/32_Bullets"), dropObject = LoadItemModel ("Bullets/32_Bullets"), itemstacksize = 64, itemstacklimit = 64};
		
		localItemBase.Add(preset);
		
		preset = new InventoryItem() {id = 140, itemname = "20_Gauge_Shells", DisplayName = "20 Gauge Shells", itemtype = ItemType.Ammo, droppable = true, itemtex = LoadItemTex ("Bullets/20_Gauge_Shells"), worldObject = LoadItemModel ("Bullets/20_Gauge_Shells"), dropObject = LoadItemModel ("Bullets/20_Gauge_Shells"), itemstacksize = 64, itemstacklimit = 64};
		
		localItemBase.Add(preset);*/
		
		return localItemBase.ToArray();
	
	}
	
	//This is for identify the slot that the mouse is hovering for display or not its description (tooltip)
	string hoveringSlot; 

	public Texture emptyTex;
	Texture bagtexture;
	int iconSizeW = 37;
	int iconSizeH = 39;

	//This is the object the mouse is carrying
	InventoryItem mouseitem = null;

	private bool itemsLoaded;

	AdvancedRepeatButton adrebutton;

	//private Transform player;
	//private GameObject playerObject;

	//Principal variable of Inventories, esta variable se asigna mas tarde segun el tipo de Item

	private InventoryItem[] inventory;

	private InventoryItem[] inv;

	/*public bool SceneItemToInv(int id) {
		return AddItem(FindItem(id));
	}*/

	public InventoryItem GetItem(InventoryItem[] inventory, int slot) {
		return inventory[slot];
	}

	private int SlotSetted;

	//private bool slotsLoaded = false;

	public void ChangeItemName(int id, string newname) {
		ItemBase.MainBase.FirstOrDefault(x => x.id == id).DisplayName = newname;
	}

	public void ChangeItemCaption(int id, string str) {
		ItemBase.MainBase.FirstOrDefault(x => x.id == id).strGUI = str;
	}

#pragma warning disable

	public static bool AddItem(InventoryItem item, int position = -1, SlotsTypes findPath = SlotsTypes.None) {

		//findPath: Obtiene el tipo del item y busca el sitio mas idoneo para meterlo. (String.Empty: Si, Else: enum.parse)

		//Debug.Log(item.id+" "+position+" "+findPath.ToString());

		bool autoDetect = findPath == SlotsTypes.None;

		if(item.weight*item.itemstacksize+Inv.me.totalWeight() > Slots.MaxSupport) { //(findPath == SlotsTypes.HotBar || findPath == SlotsTypes.Inventory) && 
			Inv.me.DropItem(item);
			return false;
		}

		if(!autoDetect && position >= 0) {

			switch(findPath) {

				case SlotsTypes.Accesories:
					if(Slots.Accesories[position] != null || Slots.Accesories.Length > position) {
						autoDetect = true;
					}
					break;

				case SlotsTypes.Ammo:
					if(Slots.AmmoSlots[position] != null || Slots.AmmoSlots.Length > position) {
						autoDetect = true;
					}
					break;

				case SlotsTypes.HotBar:
					if(Slots.HotBarSlots[position] != null || Slots.HotBarSlots.Length > position) {
						autoDetect = true;
					}
					break;

				case SlotsTypes.Inventory:
					if(Slots.InventorySlots[position] != null || Slots.InventorySlots.Length > position) {
						autoDetect = true;
					}
					break;

			}

		}

		if(autoDetect) {
			switch(item.itemtype) {

			case ItemType.Accesory:
				for(int x=0; x<Slots.Accesories.Length;x++) {
					if(Slots.Accesories[x] == null) {
						Slots.Accesories[x] = item;
						return true;
					}
				}
				Inv.me.DropItem(item);
				return false;
				break;

			case ItemType.Equip:
				for(int x=0; x<Slots.EquipedItem.Length;x++) {
					if(Slots.EquipedItem[x] == null) {
						Slots.EquipedItem[x] = item;
						return true;
					}
				}
				Inv.me.DropItem(item);
				return false;
				break;

			case ItemType.Ammo:
				for(int x=0; x<Slots.AmmoSlots.Length;x++) {
					if(Slots.AmmoSlots[x] == null) {
						Slots.AmmoSlots[x] = item;
						return true;
					}
				}
				Inv.me.DropItem(item);
				return false;
				break;

			default:

				if(position < 0) {
					
					//Detectar si el inventario esta lleno para asignar los HotBarslots o los InventorySlots, y si esta lleno del todo salir del sub...
					
					//If autodetect, determinar el ultimo caso como el invetario (default case), es decir el actual, y mientras hacer switches 'case itemType: poner en un slot del tipo deseado'

					for(int x=0; x<Slots.InventorySlots.Length;x++) {
						if(Slots.InventorySlots[x] == null) {
							Slots.InventorySlots[x] = item;
							return true;
						}
					}

					//Debug.Log("Inventory is full...");

					for(int x=0;x<Slots.HotBarSlots.Length;x++) {
						if(Slots.HotBarSlots[x] == null) {
							Slots.HotBarSlots[x] = item;
							return true;
						}
					}

					//Debug.Log("HotB too...");

					Inv.me.DropItem(item);
					return false;
					
				} else {
					try {
						Slots.InventorySlots[position] = item;
					} catch {
						try {
							Slots.HotBarSlots[position] = item;
						} catch {
							//This means that inventory is full
							Inv.me.DropItem(item); //Drop it!
							return false;
						}
					}
				}
				break;

			}
		} else {

			//private readonly string[] SlotsTypes = new string[] {"Accesories", "HotBar", "Ammo", "Inventory", "Equip"};

			if(position < 0) {

				if(findPath == SlotsTypes.Accesories) {

					for(int x=0; x<Slots.Accesories.Length;x++) {
						if(Slots.Accesories[x] == null) {
							Slots.Accesories[x] = item;
							return true;
						}
						
						if(x == Slots.Accesories.Length) {
							Inv.me.DropItem(item);
							return false;
						}
					}

				} else if(findPath == SlotsTypes.HotBar) {

					for(int x=0;x<Slots.HotBarSlots.Length;x++) {
						if(Slots.HotBarSlots[x] == null) {
							Slots.HotBarSlots[x] = item;
							return true;
						}
						
						if(x == Slots.HotBarSlots.Length) {
							Inv.me.DropItem(item);
							return false;
						}
					}

				} else if(findPath == SlotsTypes.Ammo) {

					for(int x=0; x<Slots.AmmoSlots.Length;x++) {
						if(Slots.AmmoSlots[x] == null) {
							Slots.AmmoSlots[x] = item;
							return true;
						}
						
						if(x == Slots.AmmoSlots.Length) {
							Inv.me.DropItem(item);
							return false;
						}
					}
					
				} else if(findPath == SlotsTypes.Inventory) {

					for(int x=0; x<Slots.InventorySlots.Length;x++) {
						if(Slots.InventorySlots[x] == null) {
							Slots.InventorySlots[x] = item;
							return true;
						}
						
						if(x == Slots.InventorySlots.Length) {
							Inv.me.DropItem(item);
							return false;
						}
						//inventory = InventorySlots;
					}
					
				} else if(findPath == SlotsTypes.Equip) {

					for(int x=0; x<Slots.EquipedItem.Length;x++) {
						if(Slots.EquipedItem[x] == null) {
							Slots.EquipedItem[x] = item;
							return true;
						}
						
						if(x == Slots.EquipedItem.Length) {
							Inv.me.DropItem(item);
							return false;
						}
					}

				}

			} else {

				if(findPath == SlotsTypes.Accesories) {
					try {
						Slots.Accesories[position] = item;
						return true;
					} catch {
						Inv.me.DropItem(item);
						return false;
					}
				} else if(findPath == SlotsTypes.HotBar) {
					try {
						Slots.HotBarSlots[position] = item;
						return true;
					} catch {
						Inv.me.DropItem(item);
						return false;
					}
				} else if(findPath == SlotsTypes.Ammo) {
					try {
						Slots.AmmoSlots[position] = item;
						return true;
					} catch {
						Inv.me.DropItem(item);
						return false;
					}
				} else if(findPath == SlotsTypes.Inventory) {
					try {
						Slots.InventorySlots[position] = item;
						return true;
					} catch {
						Inv.me.DropItem(item);
						return false;
					}
				} else if(findPath == SlotsTypes.Equip) {
					try {
						Slots.EquipedItem[position] = item;
						return true;
					} catch {
						Inv.me.DropItem(item);
						return false;
					}
				}

			}

		}

		//Debug.Log("Autodetect = "+autoDetect+", id = "+item.id+", pos = "+position+",  type = "+findPath.ToString());
		Debug.Log("Item dropped!");
		Inv.me.DropItem(item);
		return false;

	} 

	#pragma warning restore
	
	//ScrapInventory.cs

	private GameObject blah;

	public bool DropSelectedItem(int slot, Vector3? nulDropPos = null, SlotsTypes slotType = SlotsTypes.HotBar) {

		blah = new GameObject();

		InventoryItem[] selectedSlot = getSlotType(slotType);
		InventoryItem selectedItem = new InventoryItem();
		try {
			selectedItem = selectedSlot[slot];
		} catch {
			return false; //This means that the selectedItem is null... but I don't want to throw a exception and stop the game...
		}

		blah.name = "Item_"+selectedItem.itemname;

		if(mouseitem.dropObject == null && mouseitem.worldObject == null) {
			return false;
		} else {
			if(mouseitem.dropObject == null) {
				blah = mouseitem.worldObject;
			} else {
				blah = mouseitem.dropObject;
			}
		}

		GameObject currentPlayer = GameObject.Find("Player");
		Vector3 dropPos = currentPlayer.transform.position;

		if(nulDropPos != null) {
			dropPos = (Vector3)nulDropPos;
		}

		try {
			GameObject dropObject = (GameObject)GameObject.Instantiate(blah, dropPos, Quaternion.identity);
			dropObject.GetComponent<ItemScript>().stacksize = selectedItem.itemstacksize;
			return true;
		} catch {
			return false;
		}

	}

	public bool DropItem(InventoryItem item = null, Vector3? nulDropPos = null) { //InvItemToScene

		//Debug.Log(System.Environment.StackTrace);

		if (item == null) {
			item = mouseitem;		
		}

		blah = new GameObject();

		blah.name = "Item_"+item.id;

		if(item.dropObject == null && item.worldObject == null) {
			return false;
		} else {
			if(item.dropObject == null) {
				blah = item.worldObject;
			} else {
				blah = item.dropObject;
			}
		}

		GameObject currentPlayer = GameObject.Find("Player");
		Vector3 dropPos = currentPlayer.transform.position;
		
		if(nulDropPos != null) {
			dropPos = (Vector3)nulDropPos;
		}

		try {
			GameObject dropObject = (GameObject)GameObject.Instantiate(blah, dropPos, Quaternion.identity);
			dropObject.GetComponent<ItemScript>().stacksize = item.itemstacksize;
			return true;
		} catch {
			return false;
		}

	}

	private bool IsInventory(Vector2? nulPos = null) {

		Vector2 Pos = Event.current.mousePosition;

		if(nulPos != null) {
			Pos = (Vector2)nulPos;
		}

		if (Slots.AccesoriesRect.Contains(Pos)) {
			return true;
		} else if(Slots.AmmoSlotsRect.Contains(Pos)) {
			return true;
		} else if(Slots.HotBarSlotsRect.Contains(Pos)) {
			return true;
		} else if(Slots.InventorySlotsRect.Contains(Pos)) {
			return true;
		} else {
			return false;
		}

	}

	public void InitialLoad() {

		//Part where the slots are charged
		if(!itemsLoaded) {

			//ItemBase.MainBase = new InventoryItem[MaxItemBase];
			//ItemBase.MainBase = Load();
			Slots.HotBarSlots = new InventoryItem[LvlSys.HTS];
			Slots.InventorySlots = new InventoryItem[LvlSys.IIX*LvlSys.IIY];
			Slots.AmmoSlots = new InventoryItem[LvlSys.Ammo];
			Slots.Accesories = new InventoryItem[LvlSys.AccsX*LvlSys.AccsY];
			SelectedSlot.SSlot = 0;

			//Slots.EquipedItem = new InventoryItem[Set_Ammo(lvl)+Set_AccsX(lvl)*Set_AccsY(lvl)+Armor];
			Slots.AccesoriesRect = new Rect(Screen.width/2-(43*LvlSys.AccsX/2)+235, Screen.height/2-240+20+50, LvlSys.AccsX*43, LvlSys.AccsY*43);
			Slots.AmmoSlotsRect = new Rect(Screen.width/2-(43*LvlSys.Ammo/2)+235, Screen.height/2-130+20+50, LvlSys.Ammo*43, 43);
			Slots.InventorySlotsRect = new Rect(Screen.width/2-(LvlSys.IIX*43)/2, Screen.height/2+LvlSys.IIY/2+40+(((68.5f-43*(LvlSys.IIY-1)) > 0) ? (68.5f-43*(LvlSys.IIY-1)) : 0), LvlSys.IIX*43, LvlSys.IIY*43);
			Slots.HotBarSlotsRect = new Rect(Screen.width/2-(43*LvlSys.HTS/2), Screen.height/2+220, 43*LvlSys.HTS, 43);

			//Add starting kit items

			/*AddItem(FindItem(64));
			AddItem(FindItem(65));
			AddItem(FindItem(66));
			AddItem(FindItem(128));
			AddItem(FindItem(140));*/

			//AddItem(FindItem(65));
			//AddItem(FindItem(128));

			itemsLoaded = true;

		}
		//End that part

	}

	public void Draw() {

		if(adrebutton == null) {
			adrebutton = new AdvancedRepeatButton();
		}

		//Not changeable things
#pragma warning disable
		int Armor = 4;
		int Clothes = 0; //Not implemented yet
		int Dyes = 0; //Not implemented yet
#pragma warning restore

		GUIStyle lblStyle = new GUIStyle();

		lblStyle.fontStyle = FontStyle.Bold;
		lblStyle.normal.textColor = Color.white;
		lblStyle.fontSize = 12;
		lblStyle.alignment = TextAnchor.MiddleCenter;

		int slot = 0;

		GUI.Label(new Rect(Screen.width/2-(43*LvlSys.AccsX/2)+235, Screen.height/2-240+50, LvlSys.AccsX*43, 20), "Accesorios", lblStyle);

		GUI.Label(new Rect(Screen.width/2-350+275, Screen.height/2-240+50, 100, 20), "Previsualización", lblStyle);

		GUI.Label(new Rect(Screen.width/2-(43*LvlSys.Ammo/2)+235, Screen.height/2-130+50, LvlSys.Ammo*43, 20), "Munición", lblStyle);

		GUI.Label(new Rect(Screen.width/2-50, Screen.height/2+20, 100, 20), "<color=#ffffffff><size=14>Inventario ("+totalWeight()+"kg/"+Slots.MaxSupport.ToString("F2")+"kg)</size></color>", new GUIStyle() {richText = true, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter});

		for (int h = 0; h < LvlSys.HTS; h++) {
			inventoryItemSlot(Slots.HotBarSlots, h, Screen.width/2-(43*LvlSys.HTS/2)+(43*h), Screen.height/2+220);
		}

		for (int am = 0; am < LvlSys.Ammo; am++) {
			inventoryItemSlot(Slots.AmmoSlots, am, Screen.width/2-(43*LvlSys.Ammo/2)+(43*am)+235, Screen.height/2-110+50);
		}

		slot = 0;

		for (int ax = 0; ax < LvlSys.AccsX; ax++) {
			for (int ay = 0; ay < LvlSys.AccsY; ay++) {
				inventoryItemSlot(Slots.Accesories, slot, Screen.width/2-(43*LvlSys.AccsX/2)+(43*ax)+235, Screen.height/2+(ay*43)-220+50);
				slot++;
			}
		}

		slot = 0;

		for (int iy = 0; iy < LvlSys.IIY; iy++) {
			for (int ix = 0; ix < LvlSys.IIX; ix++) {
				inventoryItemSlot(Slots.InventorySlots, slot, Screen.width/2-(LvlSys.IIX*43)/2+(43*ix), Screen.height/2+LvlSys.IIY/2+(43*iy)+40+(((68.5f-43*(LvlSys.IIY-1)) > 0) ? (68.5f-43*(LvlSys.IIY-1)) : 0));
				slot++;
			}
		}

		toolTip();

	}

	private InventoryItem[] SetItem(InventoryItem[] arraypassed, int slot, InventoryItem item) {

		arraypassed[slot] = item;

		return arraypassed;

	}

	private bool ItemBox(float x, float y, string str) {

		GUIStyle boxstyle = new GUIStyle ();

		//Fondo y textura
		Texture2D boxfondo = (Texture2D)Resources.Load ("images/ItemBox3");
		boxstyle.normal.background = boxfondo;

		//Font
		boxstyle.fontSize = 12;
		boxstyle.fontStyle = FontStyle.Bold;
		boxstyle.normal.textColor = Color.white;
		boxstyle.alignment = TextAnchor.MiddleCenter;

		Rect slotPos = new Rect(x, y, iconSizeW+2, iconSizeH+2);
			
		if(Event.current.button == 0) {
			return GUI.Button(slotPos, "", boxstyle);
		} else {
			return adrebutton.Draw(slotPos, "", boxstyle, 1, 1);
		}

	}

	public void ItemReCheck(float x, float y, string str, InventoryItem item) {

		Rect slotPos = new Rect(x, y, 39, 41);

		Vector2 mousePos = Event.current.mousePosition;

		if(item != null) { //Draw the desired item in their respective slot...
			GUI.DrawTexture(new Rect(x+1,y+1,iconSizeW, iconSizeH), item.itemtex);
			if(item.itemstacksize > 1) {
				GUIStyle stackstyle = new GUIStyle();
				
				stackstyle.fontStyle = FontStyle.Bold;
				stackstyle.normal.textColor = Color.white;
				stackstyle.fontSize = 12;
				stackstyle.alignment = TextAnchor.LowerRight;

				GUI.Label(new Rect(x, y, iconSizeW, iconSizeH), item.itemstacksize.ToString(), stackstyle);

			}
		}

		if(mouseitem != null) {
			MouseDraw();
		} else {
			
			if(slotPos.Contains(mousePos)) {
				hoveringSlot = str;
			}
			
		}

	}

#pragma warning disable

	public static InventoryItem[] getSlotType(SlotsTypes slotType) {
		switch(slotType) {
		case SlotsTypes.Inventory:
			return Slots.InventorySlots;
			break;
			
		case SlotsTypes.Accesories:
			return Slots.Accesories;
			break;
			
		case SlotsTypes.Ammo:
			return Slots.AmmoSlots;
			break;
			
		case SlotsTypes.HotBar:
			return Slots.HotBarSlots;
			break;
			
		case SlotsTypes.Equip:
			return Slots.EquipedItem;
			break;
			
		default:
			return null;
			break;
		}
	}

	public static InventoryItem[] getShortcut(string str) {
		switch(str) {
			case "Invs":
				return Slots.InventorySlots;
				break;
				
			case "Accs":
				return Slots.Accesories;
				break;
				
			case "Ammo":
				return Slots.AmmoSlots;
				break;
				
			case "HotB":
				return Slots.HotBarSlots;
				break;
				
			case "Equip":
				return Slots.EquipedItem;
				break;
				
			default:
				return null;
				break;
		}
	}

	public static SlotsTypes getType(string str) {
		switch(str) {
		case "Invs":
			return SlotsTypes.Inventory;
			break;
			
		case "Accs":
			return SlotsTypes.Accesories;
			break;
			
		case "Ammo":
			return SlotsTypes.Ammo;
			break;
			
		case "HotB":
			return SlotsTypes.HotBar;
			break;
			
		case "Equip":
			return SlotsTypes.Equip;
			break;
			
		default:
			return SlotsTypes.None;
			break;
		}
	}

	public static InventoryItem setProperty(InventoryItem item, string property) {

		string[] splitted = property.Split('=');

		string prop = splitted[0];
		string value = splitted[1];

		switch(prop) {
			case "ItemID":
				item.id = System.Convert.ToInt32(value);
				break;
		case "StackSize":
			item.itemstacksize = System.Convert.ToInt32(value);
			break;
		}

		return item;

	}

	public bool canBeThere(SlotsTypes slotType, ItemType itemT) { //Checks if the current passed item can be in the selected slot
		switch(slotType) {

			case SlotsTypes.Accesories:
				return itemT == ItemType.Accesory;
				break;

			case SlotsTypes.Ammo:
				return itemT == ItemType.Ammo;
				break;

			case SlotsTypes.HotBar:
				return true;
				break;

			case SlotsTypes.Inventory:
				return true;
				break;

		}
		return false;
	}

#pragma warning restore

	public static void SaveInventory(string lvlName) {

		//Inventory
		
		// Save the inventory only if the items was loaded...
		
		//Si habia algo escrito anteriormente, borrarlo, para asegurarse que no se repita doblemente todo...
		
		string oldContents = System.IO.File.ReadAllText(Application.dataPath + "/saves/"+lvlName+"/level.dat");
		string newFileContent = Regex.Replace(oldContents, "\\[Inventory\\].*?\\[\\/EndInventory\\]", "", RegexOptions.Singleline);
		
		if(newFileContent != System.IO.File.ReadAllText(Application.dataPath + "/saves/"+lvlName+"/level.dat")) {
			System.IO.File.WriteAllText(Application.dataPath + "/saves/"+lvlName+"/level.dat", newFileContent);
		}
		
		if(ItemBase.ItemsLoaded) {
			
			//En caso de que haya alguna index del array con contenido.
			
			if(Slots.InventorySlots.Any(x => x != null) || Slots.HotBarSlots.Any(x => x != null) || Slots.AmmoSlots.Any(x => x != null) || Slots.Accesories.Any(x => x != null)) {
				
				StringBuilder sb = new StringBuilder(env.NewLine + env.NewLine + "[Inventory]" + env.NewLine);
				
				if(Slots.InventorySlots.Any(x => x != null)) {
					
					InventoryItem[] notNulls = Slots.InventorySlots.Where(x => x != null).ToArray();
					int notNullLength = notNulls.Length;
					
					sb = sb.Append("[InvsSlots] { ");
					
					//string tempInv = env.NewLine + "[Inventory]" + env.NewLine + "[InvSlots] { ";
					
					for(int i = 0; i < notNullLength; i++)
					{
						sb = sb.Append("SlotID="+i+" [ItemID="+notNulls[i].id+";StackSize="+notNulls[i].itemstacksize+"]" + ((i < notNullLength-1) ? " | " : ""));
					}
					
					sb = sb.AppendLine(" } [/EndInvsSlots]");
					
				}
				
				if(Slots.HotBarSlots.Any(x => x != null)) {
					
					InventoryItem[] notNulls = Slots.HotBarSlots.Where(x => x != null).ToArray();
					int notNullLength = notNulls.Length;
					
					sb = sb.Append("[HotBSlots] { ");
					
					for(int i = 0; i < notNullLength; i++)
					{
						sb = sb.Append("SlotID="+i+" [ItemID="+notNulls[i].id+";StackSize="+notNulls[i].itemstacksize+"]" + ((i < notNullLength-1) ? " | " : ""));
					}
					
					sb = sb.AppendLine(" } [/EndHotBSlots]");
					
				}
				
				if(Slots.AmmoSlots.Any(x => x != null)) {
					
					InventoryItem[] notNulls = Slots.AmmoSlots.Where(x => x != null).ToArray();
					int notNullLength = notNulls.Length;
					
					sb = sb.Append("[AmmoSlots] { ");
					
					for(int i = 0; i < notNullLength; i++)
					{
						sb = sb.Append("SlotID="+i+" [ItemID="+notNulls[i].id+";StackSize="+notNulls[i].itemstacksize+"]" + ((i < notNullLength-1) ? " | " : ""));
					}
					
					sb = sb.AppendLine(" } [/EndAmmoSlots]");
					
				}
				
				if(Slots.Accesories.Any(x => x != null)) {
					
					InventoryItem[] notNulls = Slots.Accesories.Where(x => x != null).ToArray();
					int notNullLength = notNulls.Length;
					
					sb = sb.Append("[AccsSlots] { ");
					
					for(int i = 0; i < notNullLength; i++)
					{
						sb = sb.Append("SlotID="+i+" [ItemID="+notNulls[i].id+";StackSize="+notNulls[i].itemstacksize+"]" + ((i < notNullLength-1) ? " | " : ""));
					}
					
					sb = sb.AppendLine(" } [/EndAccsSlots]");
					
				}
				
				//Equip should be there...
				
				sb = sb.Append("[/EndInventory]");
				
				string tempInv = sb.ToString();
				
				System.IO.File.AppendAllText(Application.dataPath + "/saves/"+lvlName+"/level.dat", tempInv);
				
			}
			
		}

	}

	public static void LoadInventory(string lvlName) {
		
		if(!ItemBase.ItemsLoaded) {
			
			//Inv inv2 = new Inv();
			
			string Content = System.IO.File.ReadAllText(Application.dataPath + "/saves/" + lvlName + "/level.dat");
			Match FileRead = Regex.Match(Content, "\\[Inventory\\].*?\\[\\/EndInventory\\]", RegexOptions.Singleline);
			
			if(FileRead.Success) {
				
				using(System.IO.StringReader reader = new System.IO.StringReader(FileRead.Value))
				{
					string line;
					int lineNum = 0;
					int numLines = FileRead.Value.Split('\n').Length;
					while((line = reader.ReadLine()) != null)
					{
						if(lineNum != 0 && lineNum < numLines-1) {
							InventoryItem item = new InventoryItem();
							int slot = -1;
							string[] split = line.Split('|');
							SlotsTypes slotType = getType(line.Substring(1,4));
							for(int i = 0; i < split.Length; i++) {
								string data = Regex.Replace(split[i], "(\\[\\w+\\]|\\[/\\w+\\])", ""); //Clean [Inventory] and [/EndInventory] tags
								Match mSlot = Regex.Match(data, "(?<=SlotID=)\\d+");
								Match mProperties = Regex.Match(data, "(?<=[\\[]).+?(?=[\\]])");
								string properties;
								if(mSlot.Success) {
									slot = System.Convert.ToInt32(mSlot.Value);
								}
								if(mProperties.Success) {
									properties = mProperties.Value;
									string[] propertyArray = properties.Split(';');
									for(int j = 0; j < propertyArray.Length; j++) {
										if(j == 0) {
											item = FindItem(System.Convert.ToInt32(propertyArray[j].Split('=').Last()));
										} else {
											item = setProperty(item, propertyArray[j]);
										}
									}
								}
								Inv.getSlotType(slotType)[slot] = item;
								//AddItem(item, slot, slotType);
								//Debug.Log("Item = "+item.id+", slot = "+slot+", type = "+slotType.ToString());
							}
						}
						lineNum++;
					}
				}
			}
			
			ItemBase.ItemsLoaded = true;
			
		}
		
	}

	//End of misc functions...

	//Inventory scripts start there...

	public void inventoryItemSlot(InventoryItem[] arraypassed, int slot, float xpos, float ypos) {

		string slotIDStr = "";
		
		if(arraypassed == Slots.InventorySlots) {
			slotIDStr = "Invs_";
		} else if(arraypassed == Slots.Accesories) {
			slotIDStr = "Accs_";
		} else if(arraypassed == Slots.AmmoSlots) {
			slotIDStr = "Ammo_";
		} else if(arraypassed == Slots.HotBarSlots) {
			slotIDStr = "HotB_";
		} else if(arraypassed == Slots.EquipedItem) {
			slotIDStr = "Equip_";
		}
		
		slotIDStr = slotIDStr + slot;

		if(ItemBox(xpos, ypos, slotIDStr)) { //That means that you click a slot..
			//Debug.Log(Time.time);
			if(Event.current.button == 0) {
				if(mouseitem == null) { //Pick the item
					if(arraypassed[slot] != null) {
						mouseitem = arraypassed[slot];
						arraypassed[slot] = null;
					}
				} else { //Put it on the slot
					if(arraypassed[slot] == null) {
						if(canBeThere(getType(slotIDStr.Substring(0,4)), mouseitem.itemtype)) {
							arraypassed[slot] = mouseitem;
							mouseitem = null;
						}
					} else {
						if(arraypassed[slot].itemstacklimit > 1 && arraypassed[slot].id == mouseitem.id) {
							if(arraypassed[slot].itemstacklimit >= arraypassed[slot].itemstacksize + mouseitem.itemstacksize) {
								arraypassed[slot].itemstacksize += mouseitem.itemstacksize;
								mouseitem = null;
							} else {
								int stackbefore = arraypassed[slot].itemstacksize;
								arraypassed[slot].itemstacksize = arraypassed[slot].itemstacklimit;
								mouseitem.itemstacksize = stackbefore + mouseitem.itemstacksize - arraypassed[slot].itemstacklimit;
							}
						}
					}
				}
			} else if(Event.current.button == 1) {
				if(mouseitem == null) { //Pick one stack
					if(arraypassed[slot] != null) {
						InventoryItem itemtopick = NewItem(FindItem(arraypassed[slot].id));
						//itemtopick.itemstacksize = 1;
						mouseitem = itemtopick;
						arraypassed[slot].itemstacksize -= 1;
					}
				} else {
					if(arraypassed[slot] != null && arraypassed[slot].id == mouseitem.id && mouseitem.itemstacklimit >= mouseitem.itemstacksize+1) {
						mouseitem.itemstacksize += 1;
						arraypassed[slot].itemstacksize -= 1;
					}
				}
			}
		}

		if(arraypassed[slot] != null && arraypassed[slot].itemstacksize == 0) {
			arraypassed[slot] = null;
		}

		ItemReCheck(xpos, ypos, slotIDStr, arraypassed[slot]);

		if(Input.GetMouseButtonDown(0) && mouseitem != null && mouseitem.droppable && !IsInventory() && DropItem()) {
			mouseitem = null;
		}
		
		if(mouseitem != null && mouseitem.itemstacksize == 0) {
			mouseitem = null;
		}

	}

	public void toolTip() {
		Vector2 mousePos = Event.current.mousePosition;
		InventoryItem item = new InventoryItem();
		if(!System.String.IsNullOrEmpty(hoveringSlot)) {
			InventoryItem[] parsedSlot = getShortcut(hoveringSlot.Substring(0,4));
			int slot = System.Convert.ToInt32(hoveringSlot.Substring(5));
			item = parsedSlot[slot];
		}
		string tooltip = "";
		Screen.showCursor = !(item != null && IsInventory() || mouseitem != null);
		if(item != null) {
			tooltip = item.DisplayName+env.NewLine+item.Description+env.NewLine;
			switch(item.itemtype) {
				case ItemType.Gun:
					tooltip = "Puede infligir daño.";
					break;

				default:
					tooltip = "";
					break;
			}
			if(IsInventory()) {
				GUI.Box(new Rect(mousePos.x, mousePos.y, 150, 75), tooltip);
			}
		}
	}

	public void MouseDraw() {
		Vector2 mousePos = Event.current.mousePosition;
		GUI.DrawTexture(new Rect(mousePos.x, mousePos.y, iconSizeW, iconSizeH), mouseitem.itemtex);
		if(mouseitem.itemstacksize > 1) {
			GUIStyle stackstyle = new GUIStyle();
			
			stackstyle.fontStyle = FontStyle.Bold;
			stackstyle.normal.textColor = Color.white;
			stackstyle.fontSize = 12;
			stackstyle.alignment = TextAnchor.LowerRight;
			
			GUI.Label(new Rect(mousePos.x, mousePos.y, iconSizeW, iconSizeH), mouseitem.itemstacksize.ToString(), stackstyle);
		}
	}

	public float totalWeight() {
		float returnWeight = 0;
		for(int i = 0; i < Slots.InventorySlots.Length; i++) {
			if(Slots.InventorySlots[i] != null) {
				returnWeight += Slots.InventorySlots[i].weight*Slots.InventorySlots[i].itemstacksize;
			}
		}
		for(int i = 0; i < Slots.HotBarSlots.Length; i++) {
			if(Slots.HotBarSlots[i] != null) {
				returnWeight += Slots.HotBarSlots[i].weight*Slots.HotBarSlots[i].itemstacksize;
			}
		}
		return returnWeight;
	}

	public static SafeInvItem ConvertToSafe(InventoryItem item) {
		SafeInvItem returnitem = new SafeInvItem();
		returnitem.Description = item.Description;
		returnitem.DisplayName = item.DisplayName;
		returnitem.dropObject = AssetDatabase.GetAssetPath(item.dropObject);
		returnitem.droppable = item.droppable;
		returnitem.FcustomPostion = new Vector_3(item.FcustomPostion);
		returnitem.FcustomRotation = new Vector_3(item.FcustomRotation);
		returnitem.FcustomScale = new Vector_3(item.FcustomScale);
		returnitem.id = item.id;
		returnitem.itemname = item.itemname;
		returnitem.itemstacklimit = item.itemstacklimit;
		returnitem.itemstacksize = item.itemstacksize;
		returnitem.itemtex = AssetDatabase.GetAssetPath(item.itemtex);
		returnitem.itemtype = item.itemtype;
		returnitem.showStack = item.showStack;
		returnitem.TcustomPostion = new Vector_3(item.TcustomPostion);
		returnitem.TcustomRotation = new Vector_3(item.TcustomRotation);
		returnitem.TcustomScale = new Vector_3(item.TcustomScale);
		returnitem.usable = item.usable;
		returnitem.weight = item.weight;
		returnitem.worldObject = AssetDatabase.GetAssetPath(item.worldObject);
		returnitem.shopValue = item.shopValue;
		return returnitem;
	}

	public static InventoryItem ConvertToUnsafe(SafeInvItem item) {
		InventoryItem returnitem = new InventoryItem();
		returnitem.Description = item.Description;
		returnitem.DisplayName = item.DisplayName;
		returnitem.dropObject = (GameObject)AssetDatabase.LoadAssetAtPath(item.dropObject, typeof(GameObject));
		returnitem.droppable = item.droppable;
		returnitem.FcustomPostion = Vector_3.Revert(item.FcustomPostion);
		returnitem.FcustomRotation = Vector_3.Revert(item.FcustomRotation);
		returnitem.FcustomScale = Vector_3.Revert(item.FcustomScale);
		returnitem.id = item.id;
		returnitem.itemname = item.itemname;
		returnitem.itemstacklimit = item.itemstacklimit;
		returnitem.itemstacksize = item.itemstacksize;
		returnitem.itemtex = (Texture)AssetDatabase.LoadAssetAtPath(item.itemtex, typeof(Texture));
		returnitem.itemtype = item.itemtype;
		returnitem.showStack = item.showStack;
		returnitem.TcustomPostion = Vector_3.Revert(item.TcustomPostion);
		returnitem.TcustomRotation = Vector_3.Revert(item.TcustomRotation);
		returnitem.TcustomScale = Vector_3.Revert(item.TcustomScale);
		returnitem.usable = item.usable;
		returnitem.weight = item.weight;
		returnitem.worldObject = (GameObject)AssetDatabase.LoadAssetAtPath(item.worldObject, typeof(GameObject));
		returnitem.shopValue = item.shopValue;
		return returnitem;
	}

	//Esto es para que un item de una variable pierda su referencia estatica y al editarlo no afecte a un mayor comun
	public static InventoryItem NewItem(InventoryItem item) {
		InventoryItem returnitem = new InventoryItem();
		returnitem.Description = item.Description;
		returnitem.DisplayName = item.DisplayName;
		returnitem.dropObject = item.dropObject;
		returnitem.droppable = item.droppable;
		returnitem.FcustomPostion = item.FcustomPostion;
		returnitem.FcustomRotation = item.FcustomRotation;
		returnitem.FcustomScale = item.FcustomScale;
		returnitem.id = item.id;
		returnitem.itemname = item.itemname;
		returnitem.itemstacklimit = item.itemstacklimit;
		returnitem.itemstacksize = item.itemstacksize;
		returnitem.itemtex = item.itemtex;
		returnitem.itemtype = item.itemtype;
		returnitem.showStack = item.showStack;
		returnitem.TcustomPostion = item.TcustomPostion;
		returnitem.TcustomRotation = item.TcustomRotation;
		returnitem.TcustomScale = item.TcustomScale;
		returnitem.usable = item.usable;
		returnitem.weight = item.weight;
		returnitem.worldObject = item.worldObject;
		returnitem.shopValue = item.shopValue;
		return returnitem;
	}

}
