using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectExtension {

	public static GameObject[] FindGameObjectswithName(string name, int startIndex = 0, int length = 0) {

		List<GameObject> tempGos = new List<GameObject>();

		foreach(GameObject go in GameObject.FindObjectsOfType<GameObject>()) {

			string tempName = name;

			if(startIndex == 0 && length != 0) {
				if(length != 0) {
					tempName = name.Substring(startIndex, length);
				} else {
					tempName = name.Substring(startIndex);
				}
			}


			if(go.transform.name == tempName) {

				tempGos.Add(go);

			}

		}

		GameObject[] gos = tempGos.ToArray();

		return gos;

	}

}
