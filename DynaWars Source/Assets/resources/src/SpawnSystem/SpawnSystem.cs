using UnityEngine;
using System.Collections;

public class SpawnSystem {

	private static int _spawnNum;

	public static int SpawnNum {
		get {return _spawnNum;}
		set {_spawnNum = value;}
	}

	private static GameObject[] waypoints;
	private static TerrainGenerator tg;
	public static bool spawncreated = false;

	public static void CreateSpawn(int WaypointsNum, Mob mob) {

		//"Waypoint_"+GameObjectExtension.FindGameObjectswithName("Waypoint", 0, 8).Length

		//Create the spawn point
		GameObject spawnPoint = new GameObject();
		tg = GameObject.Find("GameScripts").GetComponent<TerrainGenerator>();

		spawnPoint.name = "Spawn"+SpawnSystem.SpawnNum;
		spawnPoint.tag = "Spawn";

		SpawnSystem.SpawnNum++;

		//I have to set a position near the player

		GameObject player = Player.PlayerObj;

		Vector3 playerPos = player.transform.position;

		float randX = playerPos.x + Random.Range(-10, 10);
		float randZ = playerPos.z + Random.Range(-10, 10);

		//Debug.Log("Spawn pos: ("+randX+", 0, "+randZ+")");

		spawnPoint.transform.position = new Vector3(randX, tg.GetHeights(randX, randZ)+1, randZ);

		Vector3 spawnPointpos = spawnPoint.transform.position;

		//Now we have to create all the waypoints

		for(int i = 0; i < WaypointsNum; i++) {
			/*float RandXp = spawnPointpos.x + Random.Range(-10, 10);
			float RandZp = spawnPointpos.z + Random.Range(-10, 10);
			float RandYp = tg.GetHeights(RandXp, RandZp);*/
			int localRandXp = Ranges.RandomValueFromRanges(new Range(-30,-10), new Range(10,30));
			int localRandZp = Ranges.RandomValueFromRanges(new Range(-30,-10), new Range(10,30));
			float RandXp = spawnPointpos.x + localRandXp;
			float RandZp = spawnPointpos.z + localRandZp;
			float RandYp = tg.GetHeights(RandXp, RandZp);
			float localRandYp = RandYp - spawnPointpos.y;
			CreateWaypoint(spawnPoint, new Vector3(localRandXp, localRandYp+1, localRandZp), i);
		}

		//Spawn it!

		Spawn(spawnPoint.name, mob);

	}

	public static void CreateWaypoint(GameObject spawn, Vector3 wpPos, int index) {

		GameObject newSpawnPoint = new GameObject();

		newSpawnPoint.transform.parent = spawn.transform;
		newSpawnPoint.transform.localPosition = wpPos;
		newSpawnPoint.AddComponent<WaypointScript>();
		newSpawnPoint.GetComponent<WaypointScript>().index = index;

	}

	/*public static void Spawn(MobsSystem.Mobs mob, string SpawnName) {

	}*/

	public static void Spawn(string SpawnName, Mob mob) { //I have to create another parameter for say to the script what prefab I have to spawn

		//INDIE_CharPack/unDead/Male/ZombieGuy/preZomGuy

		GameObject enemy = (GameObject)GameObject.Instantiate(mob.mobGameObject, GameObject.Find(SpawnName).transform.position, Quaternion.identity);
		enemy.GetComponent<AIScript>().strTag = SpawnName;
		
	}

	public static void ChangeSpawn(string SpawnName) {

	}

}

[System.Serializable]
public class Mob {
	public GameObject mobGameObject;
}
