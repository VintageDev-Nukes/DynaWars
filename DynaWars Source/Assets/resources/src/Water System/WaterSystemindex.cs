using UnityEngine;
using System.Collections;

public class WaterSystemindex : MonoBehaviour {

	WaterSystem ws;

	GameObject waterObject;

	public WaterSimple water;
	public float WaterLevel = 0;

	private GameObject mainPlayer;

	public float _WL {
		get {return WaterLevel;}
	}

	// Use this for initialization
	void Start () {

		ws = new WaterSystem();

		waterObject = (GameObject)Instantiate(Resources.Load("prefabs/Water4Example"));
		water = waterObject.AddComponent<WaterSimple>();
		water.transform.position = new Vector3(0, WaterLevel, 0);
		water.transform.localScale = new Vector3 (4000, 1, 4000); //Hay que solucionar el error del Agua no infinita



	}
	
	// Update is called once per frame
	void Update () {

		ws.UnderWaterEffects(WaterLevel - 0.905f);

	}

}
