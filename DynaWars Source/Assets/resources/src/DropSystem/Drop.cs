using UnityEngine;
using System.Collections;

[System.Serializable]
public class Drops {
	public DropItem[] dropItems;
	public int MoneyToDrop;
	public int ExpToGive;
}

[System.Serializable]
public class DropItem {
	public ItemList itemType;
}
