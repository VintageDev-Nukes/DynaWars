using UnityEngine;
using System.Collections;
using WorldGeneration;

public class WorldGeneratorindex : MonoBehaviour {
	
	public GameObject PlayerObject;

	public Terrain myterrain;

	public float tileSize = 2f;
	public float baseLine = 0.5f;
	public int seed = 2; //(int)Time.time;
	
	public WaterSimple water;
	public int WaterLevel = 20;

	WorldGen wg;

	// Use this for initialization
	void Start () {

		wg = new WorldGen ();

		wg.SetValues(PlayerObject, tileSize, baseLine, seed, water, WaterLevel, myterrain);

		wg.OnStart();

		wg.UpdateTerrainPositionsAndNeighbors();
		
		wg.SetPlayerOnGround(myterrain, PlayerObject);
		
		wg.SetTextureOnTerrain(myterrain);

	}
	
	// Update is called once per frame
	void Update () {

		wg.OnUpdate();

	}
}
