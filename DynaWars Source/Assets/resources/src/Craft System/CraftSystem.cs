using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Craftings {Hand, Workbench, Furnance, Campfire}

public class CraftingStations {

	public CraftingStations(Craftings crafts, int[] itemIDs) {
		this.myCraft = crafts;
		this.availableItems = itemIDs;
	}

	public Craftings myCraft;
	public int[] availableItems;

}

public class Recipe {

	public Recipe(int id, params RecipeMaterial[] mats) {
		this.resultItemID = id;
		this.materials = mats;
		this.station = Craftings.Hand;
		this.lvlRequired = 0;
	}
	
	public Recipe(int id, Craftings station = Craftings.Hand, int lvl = 0, params RecipeMaterial[] mats) {
		this.resultItemID = id;
		this.materials = mats;
		this.station = station;
		this.lvlRequired = lvl;
	}

	public int resultItemID;
	public RecipeMaterial[] materials;
	public Craftings station;
	public int lvlRequired;
	
}

public class RecipeMaterial {
	
	public RecipeMaterial(int id, int qnt = 1) {
		this.itemID = id;
		this.Quantity = qnt;
	}
	
	public int itemID;
	public int Quantity;
	
}

public class RecipeMaterialsPosition {
	public SlotsTypes slottype;
	public int slot;
	public int quantitytocraft;
}

public class CraftSystem {

	public static List<Recipe> recipeList = new List<Recipe>();

	public static List<CraftingStations> craftSList = new List<CraftingStations>();

	public static void Load() {

		//Para hacer el item (id = 40) se necesita 1 de item (id = 10) y 1 de item (id = 11)
		Recipe rec = new Recipe(40, new RecipeMaterial(10), new RecipeMaterial(11));

		recipeList.Add(rec);

		LoadCraftings();

	}

	public static void LoadCraftings() {

		CraftingStations crafts = new CraftingStations(Craftings.Hand, new int[] {40});

		craftSList.Add(crafts);

	}

	public static bool CheckRecipeAvailability(int recipedItem, ref List<RecipeMaterialsPosition> referencedPosition, ref int craftableQuantity) {
		return CheckRecipeAvailability(recipedItem, 1, ref referencedPosition, ref craftableQuantity);
	}
	
	public static bool CheckRecipeAvailability(int recipedItem) {
		return CheckRecipeAvailability(recipedItem, 1);
	}
	
	public static bool CheckRecipeAvailability(int recipedItem, int quantity, ref List<RecipeMaterialsPosition> referencedPosition, ref int craftableQuantity) {
		
		referencedPosition = new List<RecipeMaterialsPosition>();
		
		Recipe recipeCheck = CraftSystem.recipeList.FirstOrDefault(x => x.resultItemID == recipedItem);

		if(recipeCheck == null) {
			return false;
		}
		
		bool[] canbecrafted = new bool[recipeCheck.materials.Length];
		int[] craftAmount = new int[recipeCheck.materials.Length];
		
		for(int i = 0; i < recipeCheck.materials.Length; i++) {
			List<InventoryItem> amount = new List<InventoryItem>();
			for(int k = 0; k < 2; k++) {
				InventoryItem[] slotToUse = Slots.InventorySlots;
				SlotsTypes slotType = SlotsTypes.Inventory;
				if(k == 1) {
					slotToUse = Slots.HotBarSlots;
					slotType = SlotsTypes.HotBar;
				}
				InventoryItem[] subAmount = slotToUse.Where(x => x != null && x.id == recipeCheck.materials[i].itemID).ToArray();
				if(subAmount.Length == 0 && k == 0) {
					continue;
				} else if(amount.Count == 0 && subAmount.Length == 0 && k == 1) {
					return false;
				}
				for(int j = 0; j < subAmount.Length; j++) {
					amount.Add(subAmount[j]);
					referencedPosition.Add(new RecipeMaterialsPosition() {slot = System.Array.FindIndex(slotToUse, w => w == subAmount[j]), slottype = slotType});
				}
			}
			craftAmount[i] = amount.Sum(x => x.itemstacksize);
			canbecrafted[i] = craftAmount[i] >= recipeCheck.materials[i].Quantity*quantity;
		}

		craftableQuantity = Mathf.FloorToInt(craftAmount.Min() / recipeCheck.materials[System.Array.FindIndex(craftAmount, x => x == craftAmount.Min())].Quantity);
		return canbecrafted.All(x => x == true);
		
	}
	
	public static bool CheckRecipeAvailability(int recipedItem, int quantity) {
		
		Recipe recipeCheck = CraftSystem.recipeList.FirstOrDefault(x => x.resultItemID == recipedItem);

		if(recipeCheck == null) {
			return false;
		}
		
		bool[] canbecrafted = new bool[recipeCheck.materials.Length];
		
		for(int i = 0; i < recipeCheck.materials.Length; i++) {
			List<InventoryItem> amount = new List<InventoryItem>();
			for(int k = 0; k < 2; k++) {
				InventoryItem[] slotToUse = Slots.InventorySlots;
				if(k == 1) {
					slotToUse = Slots.HotBarSlots;
				}
				InventoryItem[] subAmount = slotToUse.Where(x => x != null && x.id == recipeCheck.materials[i].itemID).ToArray();
				if(subAmount.Length == 0 && k == 0) {
					continue;
				} else if(amount.Count == 0 && subAmount.Length == 0 && k == 1) {
					return false;
				}
				for(int j = 0; j < subAmount.Length; j++) {
					amount.Add(subAmount[j]);
				}
			}
			canbecrafted[i] = amount.Sum(x => x.itemstacksize) >= recipeCheck.materials[i].Quantity*quantity;
		}
		
		return canbecrafted.All(x => x == true);
		
	}

}