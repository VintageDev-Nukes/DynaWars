using UnityEngine;
using System.Collections;

public class WaypointRenderer : MonoBehaviour {

	public bool waypointEnabled = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.FindChild("New Text").renderer.enabled = waypointEnabled;
		transform.FindChild("New Text").FindChild("Background").renderer.enabled = waypointEnabled;
		transform.FindChild("Point").renderer.enabled = waypointEnabled;
	}
}
