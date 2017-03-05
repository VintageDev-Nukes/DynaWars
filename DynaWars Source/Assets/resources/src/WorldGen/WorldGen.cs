using UnityEngine;
using System.Collections;
using CoherentNoise;

public class WorldGenerator : MonoBehaviour {

	//Public Variables for the Inspector

	public GameObject PlayerObject;

	public Terrain myterrain;

	/*public int pixWidth;
	public int pixHeight;
	public float xOrg;
	public float yOrg;
	public float scale = 1.0F;
	public float lacunarity = 6.18F;
	public float h2 = 0.69F;
	public float octaves = 8.379F;
	public float offset = 0.75F;
	public float offsetPos = 0.0f;*/

	public float tileSize = 2f;

	public float baseLine = 0.5f;

	public int seed = 2; //(int)Time.time;

	public WaterSimple water;
	public int WaterLevel = 20;

	//Private variables for that File
	/*private Texture2D noiseTex;
	private Color[] pix;*/
	private Terrain[,] _terrainGrid = new Terrain[3,3];
	private Perlin perlin;
	private FractalNoise fractal;

	//private int lastChunkCharged = 1;	

	//Test varaibles
	//float[,] scales = new float[3, 3] {{0.25F, 0.3F, 0.35F}, {0.1F, 0.7F, 0.45F}, {0.6F, 0.55F, 0.4F}};

	// Use this for initialization
	void Start() {

		//renderer.material.mainTexture = noiseTex;

		//GenerateHeights (myterrain);

		//GeneratePerlinNoise();
		//HeightMapFromTexture(myterrain, noiseTex);
		//DebugSaveTexture(noiseTex, "probando");

		Terrain linkedTerrain = gameObject.GetComponent<Terrain>();
		
		_terrainGrid[0,0] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
		_terrainGrid[0,1] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
		_terrainGrid[0,2] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
		_terrainGrid[1,0] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
		_terrainGrid[1,1] = linkedTerrain;
		_terrainGrid[1,2] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
		_terrainGrid[2,0] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
		_terrainGrid[2,1] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
		_terrainGrid[2,2] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
		
		UpdateTerrainPositionsAndNeighbors();

		SetPlayerOnGround(_terrainGrid[1,1], PlayerObject);

		SetTextureOnTerrain(_terrainGrid[1,1]);

	}

	public void GenerateHeights(Terrain terrain)
	{
		var Pink = seed;
		var n = new CoherentNoise.Generation.Fractal.PinkNoise(Pink) ; //213321
		
		float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		
		for (int i = 0; i < terrain.terrainData.heightmapWidth; i++) //terrain.terrainData.heightmapWidth * lastChunkCharged
		{
			for (int k = 0; k < terrain.terrainData.heightmapHeight; k++) //terrain.terrainData.heightmapHeight * lastChunkCharged
			{
				//var x = n.GetValue(((float)i / ((float)terrain.terrainData.heightmapWidth * lastChunkCharged)) * tileSize, ((float)k / ((float)terrain.terrainData.heightmapHeight * lastChunkCharged)) * tileSize, 0);
				//var x = n.GetValue((((float)terrain.terrainData.heightmapWidth * (float)lastChunkCharged + (float)i) / (float)terrain.terrainData.heightmapWidth) * tileSize, (((float)terrain.terrainData.heightmapHeight * (float)lastChunkCharged + (float)k) / (float)terrain.terrainData.heightmapHeight) * tileSize, 0);
				var x = n.GetValue(((float)i / (float)terrain.terrainData.heightmapWidth) * tileSize, ((float)k / (float)terrain.terrainData.heightmapHeight) * tileSize, 0);
				//Debug.Log (lastChunkCharged);
				//var x = n.GetValue(((float)i / (terrain.transform.position.x + 1)) * tileSize, ((float)k / (terrain.transform.position.y + 1)) * tileSize, 0);
				heights[i, k] = x/5.0f + baseLine; //5.0f = Smooth
			}
		}
		
		terrain.terrainData.SetHeights(0,0, heights);

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
	
	private void SetPlayerOnGround(Terrain terrain, GameObject _Player)
	{
		int x = (int)PlayerObject.transform.position.x;
		int z = (int)PlayerObject.transform.position.z;
		float currentHeight = terrain.terrainData.GetHeight(x, z);
		_Player.transform.position = new Vector3 (x, currentHeight + 100, z);;
	}

	private void UpdateTerrainPositionsAndNeighbors()
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

		for (int x = 0; x < 3; x++) {
			for (int z = 0; z < 3; z++) {
				//noiseTex = new Texture2D(pixWidth, pixHeight);
				//pix = new Color[noiseTex.width * noiseTex.height];
				
				//GeneratePerlinNoise(); //scales[x,y]
				//HeightMapFromTexture(_terrainGrid[x,y], noiseTex);
				
				//int PinkPendiente = (int)Mathf.Ceil(_terrainGrid[x,z].transform.position.x/_terrainGrid[x,z].transform.position.z);
				
				GenerateHeights(_terrainGrid[x,z]);
				//lastChunkCharged += 1;
				
			}
		}
		
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

	void UpdatePlayerAndTerrain() {

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
			}
			_terrainGrid = newTerrainGrid;
			UpdateTerrainPositionsAndNeighbors();
		}

	}

	// Update is called once per frame
	void Update ()
	{
		UpdatePlayerAndTerrain();
		InfiniteWater();
	}

	/*void GeneratePerlinNoise() 
	{

		perlin = new Perlin();
		fractal = new FractalNoise(h2, lacunarity, octaves, perlin);
		noiseTex = new Texture2D(pixWidth, pixHeight);
		pix = new Color[noiseTex.width * noiseTex.height];

		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				//float xCoord = xOrg + x / noiseTex.width * scale;
				//float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = fractal.HybridMultifractal(x * scale + Time.time, y * scale + Time.time, offset);
				//float sample = perlin.Noise(noiseTex.width + 0.1f, noiseTex.height + 0.1f) * scale; //Mathf.PerlinNoise(xCoord, yCoord);
				pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
				x++;
			}
			y++;
		}
		noiseTex.SetPixels(pix);
		noiseTex.Apply();

	}

	void DebugSaveTexture(Texture2D texture, string name) 
	{

		byte[] data = texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(Application.dataPath + "/../"+name+".png", data);

	}

	void HeightMapFromTexture(Terrain terrain, Texture2D texture) 
	{

		int w = texture.width;
		int h = texture.height;
		int w2 = terrain.terrainData.heightmapWidth;
		float[,] textureData = terrain.terrainData.GetHeights(0, 0, w2, w2);
		Color[] mapColors = texture.GetPixels();
		Color[] map = new Color[w2 * w2];
		
		if (w2 != w || h != w) {
			// Resize using nearest-neighbor scaling if texture has no filtering
			if (texture.filterMode == FilterMode.Point) {
				float dx = w/w2; //float.Parse(w)/w2;
				float dy = h/w2; //float.Parse(h)/w2;
				for (int y = 0; y < w2; y++) {
					if (y%20 == 0) {
						UnityEditor.EditorUtility.DisplayProgressBar("Resize", "Calculating texture", Mathf.InverseLerp(0.0f, w2, y));
					}
					int thisY = (int)dy*y*w; //int.Parse(dy*y)*w;
					int yw = y*w2;
					for (int x = 0; x < w2; x++) {
						map[yw + x] = mapColors[thisY + (int)dx*x];
					}
				}
			}
			// Otherwise resize using bilinear filtering
			else {
				float ratioX = 1.0f/ w2/(w-1); //(float.Parse(w2)/(w-1));
				float ratioY = 1.0f/ w2/(h-1); //(float.Parse(w2)/(h-1));
				for (int y = 0; y < w2; y++) {
					if (y%20 == 0) {
						UnityEditor.EditorUtility.DisplayProgressBar("Resize", "Calculating texture", Mathf.InverseLerp(0.0f, w2, y));
					}
					float yy = Mathf.Floor(y*ratioY);
					float y1 = yy*w;
					float y2 = (yy+1)*w;
					float yw = y*w2;
					for (int x = 0; x < w2; x++) {
						float xx = Mathf.Floor(x*ratioX);
						
						Color bl = mapColors[(int)y1 + (int)xx];
						Color br = mapColors[(int)y1 + (int)xx+1]; 
						Color tl = mapColors[(int)y2 + (int)xx];
						Color tr = mapColors[(int)y2 + (int)xx+1];
						
						float xLerp = x*ratioX-xx;
						map[(int)yw + x] = Color.Lerp(Color.Lerp(bl, br, xLerp), Color.Lerp(tl, tr, xLerp), y*(int)ratioY-(int)yy);
					}
				}
			}
			UnityEditor.EditorUtility.ClearProgressBar();
		}
		else {
			// Use original if no resize is needed
			map = mapColors;
		}
		
		// Assign texture data to texture
		for (int y = 0; y < w2; y++) {
			for (int x = 0; x < w2; x++) {
				textureData[y,x] = map[y*w2+x].grayscale;
			}
		}

		terrain.terrainData.SetHeights(0, 0, textureData);
	}*/
	
}
