using UnityEngine;
using System.Collections;
using System.Linq;

public class CraftingStation : MonoBehaviour {

	public int findRange = 3;
	public Craftings myCraft;

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(CraftSystem.craftSList != null && CraftSystem.craftSList.Count > 0) {
			if((player.transform.position-transform.position).magnitude < findRange) {
				CraftingStations attCraft = CraftSystem.craftSList.FirstOrDefault(x => x.myCraft == myCraft);
				player.GetComponent<PCraftScript>().nearCraftingStation.Add(attCraft);
			} else {
				CraftingStations attCraft = CraftSystem.craftSList.FirstOrDefault(x => x.myCraft == myCraft);
				if(player.GetComponent<PCraftScript>().nearCraftingStation.Contains(attCraft)) {
					player.GetComponent<PCraftScript>().nearCraftingStation.Remove(attCraft);
				}
			}
		}
	}

}
