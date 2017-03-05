using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum NPCState {Unblocked, Blocked}

public class NPC {
	public NPC(string name, string description, int lvl, int[] items) {
		this.id = NPCSystem.npcs.ToArray().Length+1;
		this.name = name;
		this.description = description;
		this.lvlRequired = lvl;
		this.itemSellID = items;
		this.state = (PlayerStats.Lvl >= lvl) ? NPCState.Unblocked : NPCState.Blocked;
	}
	public int id;
	public string name;
	public string description;
	public int lvlRequired;
	public int[] itemSellID;
	public NPCState state = NPCState.Unblocked;
}

public class NPCSystem {

	public static List<NPC> npcs = new List<NPC>();

	private static List<NPC> _uNPCs;

	public static List<NPC> unblockedNPCs {
		get {
			if(_uNPCs == null) {
				_uNPCs = new List<NPC>();
				_uNPCs = npcs.Where(x => x.state == NPCState.Unblocked).ToList();
			}
			return _uNPCs;
		}
	}

	public static void Load() {

		NPC npcList = new NPC("Artillero", "Vende armas de fuego", 0, new int[] {1,2,3,4,5,6,7,8,9,10,11});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Herrero", "Vende y mejora armaduras y armas", 0, new int[] {1,2,3});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Mecánico", "Arregla y vende piezas de coche", 10, new int[] {1,2,3});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Cocinero", "Vende alimentos procesados", 0, new int[] {1,2,3});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Granjero", "Vende productos del campo", 0, new int[] {1,2,3});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Doctor", "Cura y vende medicamentos", 0, new int[] {1,2,3});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Mago", "Vende y mejora armas mágicas", 10, new int[] {1,2,3});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Mercader", "Vende productos básicos", 0, new int[] {1,2,3});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Minero", "Recolecta y vende materiales", 0, new int[] {1,2,3});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Estilista", "Vende objetos de apariencia", 5, new int[] {1,2,3});
		
		npcs.Add(npcList);
		
		npcList = new NPC("Militar", "Vende objetos de defensa personal", 15, new int[] {1,2,3});
		
		npcs.Add(npcList);

		npcList = new NPC("Alquimista", "Vende aleaciones", 20, new int[] {1,2,3});
		
		npcs.Add(npcList);

	}

	public static void NPCCheck() {
		for(int i = 0; i < npcs.Count; i++) {
			if(npcs[i].state == NPCState.Blocked) {
				npcs[i].state = (PlayerStats.Lvl >= npcs[i].lvlRequired) ? NPCState.Unblocked : NPCState.Blocked;
			}
		}
		_uNPCs = null;
	}

}
