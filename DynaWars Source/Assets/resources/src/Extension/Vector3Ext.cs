using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vector_3 {

	public float x {get; set;}
	public float y {get; set;}
	public float z {get; set;}

	public Vector_3() {}

	public Vector_3(float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector_3(Vector3 v) {
		this.x = v.x;
		this.y = v.y;
		this.z = v.z;
	}

	public Vector3 GetVector3() {
		return new Vector3(x, y, z);
	}

	public static Vector3 Revert(Vector_3 v) {
		return new Vector3(v.x, v.y, v.z);
	}

}

public class Vector3Ext {

	public static IEnumerator MoveFromTo(Transform obj, Vector3 pointA, Vector3 pointB, float time) {
		if (obj.position != pointB) {               // Do nothing if already moving
			float t = 0f;
			while (t < 1.0f) {
				t += Time.deltaTime / time; // Sweeps from 0 to 1 in time seconds
				obj.position = Vector3.Lerp(pointA, pointB, t); // Set position proportional to t
				yield return 0;    // Leave the routine and return here in the next frame
			}
		}

	}

	public static float AngleOffAroundAxis (Vector3 v, Vector3 forward, Vector3 axis  ){
		Vector3 right = Vector3.Cross(axis, forward);
		forward = Vector3.Cross(right, axis);
		Vector2 v2 = new Vector2(Vector3.Dot(v, forward), Vector3.Dot(v, right));
		v2.Normalize();
		return Mathf.Atan2(v2.y, v2.x);
	}

	public static float AngleDir(Vector2 A, Vector2 B)
	{
		return -A.x * B.y + A.y * B.x;	
	}

}
