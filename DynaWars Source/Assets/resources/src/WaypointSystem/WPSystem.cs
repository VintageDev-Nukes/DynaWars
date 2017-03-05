using UnityEngine;
using System.Collections;
using System.Linq;

public class WPSystem : MonoBehaviour {

	GameObject[] allWaypoints;
	//GameObject monologueText;

	/*float angle = 0;

	void OnGUI() {
		angle = GUI.HorizontalSlider(new Rect(300, 200, 500, 30), angle, 0, 360);
		Debug.Log(angle);
	}*/

	// Update is called once per frame
	void FixedUpdate () {

		float distAct = 1;

		allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

		for(int i = 0; i < allWaypoints.Length; i++) {

			allWaypoints[i].transform.LookAt(Player.PlayerObj.transform);

			float rotation = 90-allWaypoints[i].transform.eulerAngles.y;

			float x0 = allWaypoints[i].transform.position.x;
			float z0 = allWaypoints[i].transform.position.z;

			float r = Vector3.Distance(allWaypoints[i].transform.position, Player.PlayerObj.transform.position);
			float safeDistance = 5;
			
			if(r < 5) {
				distAct = 0;
			}

			float x = x0 + distAct*((r - safeDistance) * Mathf.Cos(rotation * Mathf.PI / 180));
			float z = z0 + distAct*((r - safeDistance) * Mathf.Sin(rotation * Mathf.PI / 180));
			
			allWaypoints[i].transform.FindChild("WaypointText").position = new Vector3(x, Player.PlayerObj.transform.position.y, z);
			
			string text = allWaypoints[i].GetComponent<WaypointIndexer>().WaypointName + " ["+r.ToString("F2")+"m]";
			
			allWaypoints[i].transform.FindChild("WaypointText").FindChild("New Text").GetComponent<TextMesh>().text = text;

			allWaypoints[i].transform.FindChild("WaypointText").LookAt(Player.PlayerObj.transform);

			allWaypoints[i].transform.FindChild("WaypointText").eulerAngles = new Vector3(0, allWaypoints[i].transform.FindChild("WaypointText").eulerAngles.y-180, 0);

		}

	}
}
