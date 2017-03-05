using UnityEngine;
using System.Collections;

public class LangSystem : MonoBehaviour {

	public Languages langs;

	// Use this for initialization
	void Start () {
		LangMotor.Load(ref langs);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
