using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WaypointState {Shown, Hidden, Deleted}

public class WaypointLib {

	public static List<Waypoint> waypointList;

}

public class Waypoint {
	public Vector_3 pos;
	public string name;
	public Color color;
	public WaypointState state;
}