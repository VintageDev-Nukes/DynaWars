using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnSystemindex : MonoBehaviour {

	public Mob[] allMobs;

	GameObject[] waypoints;
	TerrainGenerator tg;

	// Use this for initialization
	void Start() {

	}
	
	// Update is called once per frame
	void FixedUpdate() {
		float currentHour = GameObject.Find("GameScripts").transform.FindChild("TOD").GetComponent<TOD>().slider;
		if(GameObject.FindGameObjectsWithTag("Mob0").Length+GameObject.FindGameObjectsWithTag("Mob1").Length < 15 && RandomExt.PRand(1) && (currentHour > 0.875f || currentHour < 0.3125f)) {
			SpawnSystem.CreateSpawn(Random.Range(4, 10), allMobs[Random.Range(0, allMobs.Length)]); //This will spawn a mob
		}
	}

}
