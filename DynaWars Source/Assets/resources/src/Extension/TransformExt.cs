using UnityEngine;
using System.Collections;

public class TransformExt {

	public static Transform FindAllChilds(Transform parent, string name)
	{
		if (parent.name.Equals(name)) return parent;
		foreach (Transform child in parent)
		{
			Transform result = FindAllChilds(child, name);
			if (result != null) return result;
		}
		return null;
	}

}
