using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCraftScript : MonoBehaviour {

	public List<CraftingStations> nearCraftingStation = new List<CraftingStations>();

	bool isnotset;

	public void DefaultCrafting() {
		nearCraftingStation.Add(CraftSystem.craftSList.FirstOrDefault(x => x.myCraft == Craftings.Hand));
	}

	public void Update() {
		if(CraftSystem.craftSList != null && CraftSystem.craftSList.Count > 0 && !isnotset) {
			DefaultCrafting();
			isnotset = true;
		}
	}

}
