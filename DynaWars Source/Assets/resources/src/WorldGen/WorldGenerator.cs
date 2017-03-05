using UnityEngine;
using System.Collections;
using CoherentNoise;

namespace WorldGeneration
{

	public class WorldGen
	{
		
		//Public Variables for the Inspector
		
		GameObject PlayerObject;
		
		float tileSize; //(float)GameObject.FindWithTag("tileSize");
		float baseLine;
		int seed; //(int)Time.time;
		
		WaterSimple water;
		int WaterLevel;
		
		Terrain linkedTerrain;
		
		//Private variables for that File
		Terrain[,] _terrainGrid = new Terrain[3,3];

		private int lastChunkCharged = 1;
		
		//#######################################
		//##                                   ##
		//##  Infinite World System Variables  ##
		//##                                   ##
		//#######################################
		
		public void SetValues(GameObject PlayerObject2, float tileSize2, float baseLine2, int seed2, WaterSimple water2, int WaterLevel2, Terrain linkedTerrain2) 
		{
			PlayerObject = PlayerObject2;
			tileSize = tileSize2;
			baseLine = baseLine2;
			seed = seed2;
			water = water2;
			WaterLevel = WaterLevel2;
			linkedTerrain = linkedTerrain2;
		}
		
		public void OnStart() 
		{
			
			_terrainGrid [0,0] = Terrain.CreateTerrainGameObject (linkedTerrain.terrainData).GetComponent<Terrain> ();
			_terrainGrid [0,1] = Terrain.CreateTerrainGameObject (linkedTerrain.terrainData).GetComponent<Terrain> ();
			_terrainGrid [0,2] = Terrain.CreateTerrainGameObject (linkedTerrain.terrainData).GetComponent<Terrain> ();
			_terrainGrid [1,0] = Terrain.CreateTerrainGameObject (linkedTerrain.terrainData).GetComponent<Terrain> ();
			_terrainGrid [1,1] = linkedTerrain;
			_terrainGrid [1,2] = Terrain.CreateTerrainGameObject (linkedTerrain.terrainData).GetComponent<Terrain> ();
			_terrainGrid [2,0] = Terrain.CreateTerrainGameObject (linkedTerrain.terrainData).GetComponent<Terrain> ();
			_terrainGrid [2,1] = Terrain.CreateTerrainGameObject (linkedTerrain.terrainData).GetComponent<Terrain> ();
			_terrainGrid [2,2] = Terrain.CreateTerrainGameObject (linkedTerrain.terrainData).GetComponent<Terrain> ();
			
			for (int x = 0; x < 3; x++) {
				for (int z = 0; z < 3; z++) {
					
					GenerateHeights(_terrainGrid[x,z], lastChunkCharged + z);
					//GenerateHeights(_terrainGrid[x,z]);
					
				}
			}

			lastChunkCharged += 3;
			
			/*UpdateTerrainPositionsAndNeighbors();
			
			SetPlayerOnGround(_terrainGrid[1,1], PlayerObject);
			
			SetTextureOnTerrain(_terrainGrid[1,1]);*/
			
		}
		
		public void OnUpdate () 
		{
			
			UpdatePlayerAndTerrain();
			InfiniteWater();
			
		}
		
		void GenerateHeights(Terrain terrain, int last)
		{

			var Pink = seed;
			var n = new CoherentNoise.Generation.Fractal.PinkNoise(Pink); //213321
			
			float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
			
			for (int i = 0; i < terrain.terrainData.heightmapWidth; i++) //terrain.terrainData.heightmapWidth * lastChunkCharged
			{
				for (int k = 0; k < terrain.terrainData.heightmapHeight; k++) //terrain.terrainData.heightmapHeight * lastChunkCharged
				{
					//var x = n.GetValue((((float)i + terrain.terrainData.heightmapWidth * last) / ((float)terrain.terrainData.heightmapWidth * last)) * tileSize, (((float)k + terrain.terrainData.heightmapHeight * last) / ((float)terrain.terrainData.heightmapHeight * last)) * tileSize, 0);
					var x = n.GetValue((((float)i + terrain.terrainData.heightmapWidth * last) / ((float)terrain.terrainData.heightmapWidth * last)) * tileSize, (((float)k + terrain.terrainData.heightmapHeight * last) / ((float)terrain.terrainData.heightmapHeight * last)) * tileSize, 0);
					heights[i, k] = x / 5 + baseLine; //5.0f = Smooth
					//Debug.Log(x/5+baseLine);
				}
			}
			
			terrain.terrainData.SetHeights(0,0, heights);
			
		}

		/*void GenerateHeights(Terrain terrain)
		{
			var Pink = seed;
			var n = new CoherentNoise.Generation.Fractal.PinkNoise(Pink);
			
			float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
			
			for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
			{
				for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
				{
					var x = n.GetValue(((float)i / (float)terrain.terrainData.heightmapWidth) * tileSize, ((float)k / (float)terrain.terrainData.heightmapHeight) * tileSize, 0);
					//var x = n.GetValue((float)i , (float)k, 0);
					heights[i, k] = x/5.0f + baseLine;
				}
			}
			
			terrain.terrainData.SetHeights(0,0, heights);
			
		}*/
		
		public void SetPlayerOnGround(Terrain terrain, GameObject _Player)
		{
			int x = (int)PlayerObject.transform.position.x;
			int z = (int)PlayerObject.transform.position.z;
			float currentHeight = terrain.terrainData.GetHeight(x, z);
			_Player.transform.position = new Vector3 (x, currentHeight + 100, z);;
		}
		
		public void UpdateTerrainPositionsAndNeighbors()
		{
			
			_terrainGrid[0,0].transform.position = new Vector3(
				_terrainGrid[1,1].transform.position.x - _terrainGrid[1,1].terrainData.size.x,
				_terrainGrid[1,1].transform.position.y,
				_terrainGrid[1,1].transform.position.z + _terrainGrid[1,1].terrainData.size.z);
			_terrainGrid[0,1].transform.position = new Vector3(
				_terrainGrid[1,1].transform.position.x - _terrainGrid[1,1].terrainData.size.x,
				_terrainGrid[1,1].transform.position.y,
				_terrainGrid[1,1].transform.position.z);
			_terrainGrid[0,2].transform.position = new Vector3(
				_terrainGrid[1,1].transform.position.x - _terrainGrid[1,1].terrainData.size.x,
				_terrainGrid[1,1].transform.position.y,
				_terrainGrid[1,1].transform.position.z - _terrainGrid[1,1].terrainData.size.z);
			
			_terrainGrid[1,0].transform.position = new Vector3(
				_terrainGrid[1,1].transform.position.x,
				_terrainGrid[1,1].transform.position.y,
				_terrainGrid[1,1].transform.position.z + _terrainGrid[1,1].terrainData.size.z);
			_terrainGrid[1,2].transform.position = new Vector3(
				_terrainGrid[1,1].transform.position.x,
				_terrainGrid[1,1].transform.position.y,
				_terrainGrid[1,1].transform.position.z - _terrainGrid[1,1].terrainData.size.z);
			
			_terrainGrid[2,0].transform.position = new Vector3(
				_terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].terrainData.size.x,
				_terrainGrid[1,1].transform.position.y,
				_terrainGrid[1,1].transform.position.z + _terrainGrid[1,1].terrainData.size.z);
			_terrainGrid[2,1].transform.position = new Vector3(
				_terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].terrainData.size.x,
				_terrainGrid[1,1].transform.position.y,
				_terrainGrid[1,1].transform.position.z);
			_terrainGrid[2,2].transform.position = new Vector3(
				_terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].terrainData.size.x,
				_terrainGrid[1,1].transform.position.y,
				_terrainGrid[1,1].transform.position.z - _terrainGrid[1,1].terrainData.size.z);
			
			
			_terrainGrid[0,0].SetNeighbors(             null,              null, _terrainGrid[1,0], _terrainGrid[0,1]);
			_terrainGrid[0,1].SetNeighbors(             null, _terrainGrid[0,0], _terrainGrid[1,1], _terrainGrid[0,2]);
			_terrainGrid[0,2].SetNeighbors(             null, _terrainGrid[0,1], _terrainGrid[1,2],              null);
			_terrainGrid[1,0].SetNeighbors(_terrainGrid[0,0],              null, _terrainGrid[2,0], _terrainGrid[1,1]);
			_terrainGrid[1,1].SetNeighbors(_terrainGrid[0,1], _terrainGrid[1,0], _terrainGrid[2,1], _terrainGrid[1,2]);
			_terrainGrid[1,2].SetNeighbors(_terrainGrid[0,2], _terrainGrid[1,1], _terrainGrid[2,2],              null);
			_terrainGrid[2,0].SetNeighbors(_terrainGrid[1,0],              null,              null, _terrainGrid[2,1]);
			_terrainGrid[2,1].SetNeighbors(_terrainGrid[1,1], _terrainGrid[2,0],              null, _terrainGrid[2,2]);
			_terrainGrid[2,2].SetNeighbors(_terrainGrid[1,2], _terrainGrid[2,1],              null,              null);
			
		}
		
		public void UpdatePlayerAndTerrain() 
		{
			
			Vector3 playerPosition = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y, PlayerObject.transform.position.z);
			Terrain playerTerrain = null;
			int xOffset = 0;
			int yOffset = 0;
			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					if ((playerPosition.x >= _terrainGrid[x,y].transform.position.x) &&
					    (playerPosition.x <= (_terrainGrid[x,y].transform.position.x + _terrainGrid[x,y].terrainData.size.x)) &&
					    (playerPosition.z >= _terrainGrid[x,y].transform.position.z) &&
					    (playerPosition.z <= (_terrainGrid[x,y].transform.position.z + _terrainGrid[x,y].terrainData.size.z)))
					{
						playerTerrain = _terrainGrid[x,y];
						xOffset = 1 - x;
						yOffset = 1 - y;
						break;
					}
				}
				if (playerTerrain != null)
					break;
			}
			
			if (playerTerrain != _terrainGrid[1,1])
			{
				Terrain[,] newTerrainGrid = new Terrain[3,3];
				for (int x = 0; x < 3; x++)
					for (int y = 0; y < 3; y++)
				{
					int newX = x + xOffset;
					if (newX < 0)
						newX = 2;
					else if (newX > 2)
						newX = 0;
					int newY = y + yOffset;
					if (newY < 0)
						newY = 2;
					else if (newY > 2)
						newY = 0;
					newTerrainGrid[newX, newY] = _terrainGrid[x,y];
					GenerateHeights(newTerrainGrid[x,y], lastChunkCharged + y);
					//GenerateHeights(newTerrainGrid[x,y]);
				}
				lastChunkCharged += 3;
				_terrainGrid = newTerrainGrid;
				UpdateTerrainPositionsAndNeighbors();
			}
			
		}
		
		public void InfiniteWater() {
			water.transform.position = new Vector3(PlayerObject.transform.position.x, WaterLevel, PlayerObject.transform.position.z);
		}
		
		public void SetTextureOnTerrain(Terrain v_td)
		{
			SplatPrototype[] va_sp = new SplatPrototype[2];
			
			// create the splat types here
			// ?? do we also need to set va_sp[0].tileOffset and .tileSize
			va_sp[0] = new SplatPrototype();
			va_sp[0].texture = (Texture2D)Resources.Load("textures/Terrain Textures/GoodDirt");
			va_sp[1] = new SplatPrototype();
			va_sp[1].texture = (Texture2D)Resources.Load("textures/Terrain Textures/Grass (Hill)");
			/*va_sp[2] = new SplatPrototype();
		va_sp[2].texture = (Texture2D)Resources.Load("textures/Terrain Textures/Cliff (Layered Rock)");
		va_sp[3] = new SplatPrototype();
		va_sp[3].texture = (Texture2D)Resources.Load("textures/Terrain Textures/Grass&Rock");*/
			v_td.terrainData.splatPrototypes = va_sp;
			
			int v_td_alphaMapResolution = v_td.terrainData.alphamapResolution/2;
			
			float[, ,] va_alphamaps = new float[v_td_alphaMapResolution, v_td_alphaMapResolution, va_sp.Length];
			va_alphamaps = v_td.terrainData.GetAlphamaps(0, 0, v_td_alphaMapResolution, v_td_alphaMapResolution);
			// set the actual textures in each tile here.
			// first two indexes are coordinates, the last is the alpha blend of this particular layer.
			
			for (int ti = 0; ti < v_td.terrainData.heightmapWidth; ti++) {
				for (int tj = 0; tj < v_td.terrainData.heightmapHeight; tj++) {
					for (int v_tex = 0; v_tex < va_sp.Length; v_tex++) { 
						// set the alphamaps
						va_alphamaps[ti,tj,v_tex] = 0.5f; //Random.Range(0.0f, 1); // just randomly blend for now
					}
				}
			}
			v_td.terrainData.SetAlphamaps(0, 0, va_alphamaps);
		}

	}
}