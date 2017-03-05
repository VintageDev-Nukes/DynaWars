using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

/// <summary>
/// This is a procedural terrain generator that will create infinite sized terrains based
/// on Simplex Noise. (A slightly faster than perlin implementation of coherent noise).
/// It uses a target transform and generates terrain tiles around that transform, as
/// it moves more terrain tiles are added and removed depending on the settings.
/// @author Quick Fingers
/// </summary>
public class TerrainGenerator : MonoBehaviour {

	public static string mySeed;

    // target to track;
    private Transform target;

    // the 10x10 vertex default plane in Unity
    public Object terrainPlane;

    // (buffer * 2 + 1)^2 will be total number of planes in the scene at any one time
    public int buffer;

    // noise detail scalar. Higher values make spikier noise, values lower than 0.1 give more rolling hills
    public float detailScale;

    // as the noise always returns a value from -1 to 1 create a scalar
    public float heightScale;

    // the planes scale (default unity plane is 10 units by 10 units, any more will make localScale of terrainPlane change)
    public float planeSize = 60f;

	public float baseLine = 0.1f;

	public int seed = 1;

	public bool OnMenu = false;

	private int seedBeta;

    // plane count is the amount of planes in a single row (buffer * 2 + 1)
    private int planeCount;

    // your current position
    private int tileX,tileZ;

    // array of tiles currently on screen
    private Tile[,] terrainTiles;

	public GameObject player;
	public GameObject targetCam;

	public Material[] terrainMat;

    // Use this for initialization
    void Start() {

		if(!OnMenu) {
			target = player.transform;
		} else {
			target = targetCam.transform;
		}

		//Este script no usa las coordenadas del jugador, si no el index del Tile actual (sobre el que el jugador esta)
		//Tiles = Planos

        planeCount = buffer * 2 + 1; //Obtiene el numero de Tiles a generar
        tileX = Mathf.RoundToInt(target.position.x / planeSize); //Get the plane index where the target is placed on based on X-Axis
		tileZ = Mathf.RoundToInt(target.position.z / planeSize); //Get the plane index where the target is placed on based on Z-Axis
	
		if (!OnMenu) {
			seedBeta = new SeedExtension(mySeed).ToInt();
		} else {
			System.Random rnd = new System.Random();
			seedBeta = rnd.Next();
		}

		Generate();

		//LoadLevel function

		//PlayerSystem.LoadLevel(Profile.lastPlayedWorld, player);

	}
    
    public void Generate(float detailScale, float heightScale) {
        this.detailScale = detailScale;
        this.heightScale = heightScale;
        Generate();
    }

    public void Generate() {

        if (terrainTiles != null) {
            foreach (Tile t in terrainTiles) {
                Destroy(t.gameObject);
            }
        }

        terrainTiles = new Tile[planeCount, planeCount];

		//Recorre todos los planos generados

        for (int x = 0; x < planeCount; x++) {
            for (int z = 0; z < planeCount; z++) {
                terrainTiles[x, z] = GenerateTile( tileX - buffer + x, tileZ - buffer + z);
            }
        }

    }

    // Given a world tile x and tile z this generates a Tile object.
    private Tile GenerateTile(int x, int z) {

		var Pink = seedBeta;
		var n = new CoherentNoise.Generation.Fractal.PinkNoise(Pink); //213321

        GameObject plane =
                    (GameObject)Instantiate(terrainPlane, new Vector3(x * planeSize, 0, z * planeSize), Quaternion.identity);
        plane.transform.localScale = new Vector3(planeSize * 0.1f, 1, planeSize * 0.1f);
        plane.transform.parent = transform;

		plane.GetComponent<MeshRenderer>().material = terrainMat[Random.Range(0, terrainMat.Length)];

		plane.name = "Chunk_"+x+","+z;
		plane.tag = "Terrain";
		plane.layer = 14;

        // Get the planes vertices
        Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // alter vertex Y position depending on simplex noise)
        for (int v = 0; v < vertices.Length; v++) {
            // generate the height for current vertex
            Vector3 vertexPosition = plane.transform.position + vertices[v] * planeSize / 10f;
            //float height = SimplexNoise.Noise(vertexPosition.x * detailScale, vertexPosition.z * detailScale);
			float height = n.GetValue(vertexPosition.x * detailScale, vertexPosition.z * detailScale, 0);
			// scale it with the heightScale field
            vertices[v].y = height * heightScale + baseLine;

        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        plane.AddComponent<MeshCollider>();

        Tile tile = new Tile();
        tile.gameObject = plane;
        tile.tileX = x;
        tile.tileZ = z;

        return tile;
    }

	public float GetHeights(float x, float z) {

		GameObject plane = GameObject.Find("GameScripts").transform.FindChild("Chunk_"+Mathf.Round(x/planeSize)+","+Mathf.Round(z/planeSize)).gameObject;

		// Get the planes vertices
		Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;

		Vector3 vectorToSearch = plane.GetComponent<MeshFilter>().transform.InverseTransformPoint(new Vector3 (x, 0, z));

		int vertexNum = 0;

		for(int i = 0; i < vertices.Length; i++) {

			if(Mathf.Round(vertices[i].x) == Mathf.Round(vectorToSearch.x) && Mathf.Round(vertices[i].z) == Mathf.Round(vectorToSearch.z)) {
				vertexNum = i;
				break;
			}
		}

		return vertices[vertexNum].y;
	}


    // this function could possibly be optimized, but is a proof of concept.
    // by recording the changeX and changeZ (as -1 or 1) we can
    // remove the un needed cells and re generate the grid array with the
    // correct tiles.
    private void Cull(int changeX, int changeZ) {
        int i, j;
        Tile[] newTiles = new Tile[planeCount];
        Tile[,] newTerrainTiles = new Tile[planeCount, planeCount];

        // firstly remove the old tile gameObjects and null them from the array.
        // populate a temporary array with the newly made tiles.
        if (changeX != 0) {
            for (i = 0; i < planeCount; i++) {
                Destroy(terrainTiles[buffer - buffer * changeX, i].gameObject);
                terrainTiles[buffer - buffer * changeX, i] = null;
                newTiles[i] = GenerateTile(tileX + buffer * changeX + changeX, tileZ - buffer + i);
            }
        }
        if (changeZ != 0) {
            for (i = 0; i < planeCount; i++) {
                Destroy(terrainTiles[i, buffer - buffer * changeZ].gameObject);
                terrainTiles[i, buffer - buffer * changeZ] = null;
                newTiles[i] = GenerateTile(tileX - buffer + i, tileZ + buffer * changeZ + changeZ);
            }
        }

        // make a copy of the old terrainTiles to reference when creating the new tile map.
        Array.Copy(terrainTiles, newTerrainTiles, planeCount * planeCount);

        // go through the current tiles on screen (minus the ones we just deleted, and reapply there 
        // new position in the newTerrainTiles array.
        for (i = 0; i < planeCount; i++) {
            for (j = 0; j < planeCount; j++) {
                Tile t = terrainTiles[i, j];
                if (t != null) newTerrainTiles[-tileX - changeX + buffer + t.tileX, -tileZ - changeZ + buffer + t.tileZ] = t;
            }
        }

        // add the newly created tiles to this new array.
        for (i = 0; i < newTiles.Length; i++) {
            Tile t = newTiles[i];
            newTerrainTiles[-tileX - changeX + buffer + t.tileX, -tileZ - changeZ + buffer + t.tileZ] = t;
        }

        // set the current map to the new array.
        terrainTiles = newTerrainTiles;
    }

    

    // Update is called once per frame
    void Update() {
		try {
	        int newTileX = Mathf.RoundToInt(target.position.x / planeSize);
	        int newTileZ = Mathf.RoundToInt(target.position.z / planeSize);

	        if (newTileX != tileX) {
	            Cull(newTileX - tileX, 0);
	            tileX = newTileX;
	        }

	        if (newTileZ != tileZ) {
	            Cull(0, newTileZ - tileZ);
	            tileZ = newTileZ;
	        }
		} catch 
		{
			tileX = Mathf.RoundToInt(target.position.x / planeSize);
			tileZ = Mathf.RoundToInt(target.position.z / planeSize); 
			Generate();
		}
    }

    
}

public class Tile {
    public GameObject gameObject;
    public int tileX;
    public int tileZ;
}