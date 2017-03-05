using UnityEngine;
using System.Collections;
using System.Linq;

public class DropScript : MonoBehaviour {

	public Drops drops;

	void Drop() {
		if(drops.ExpToGive > 0) {
			PlayerStats.Exp += (ulong)drops.ExpToGive;
		}
		if(drops.MoneyToDrop > 0) {
			GameObject money = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Items/Money"), transform.position+new Vector3(0, 1, 0), Quaternion.identity);
			money.GetComponent<MoneyScript>().Quantity = Random.Range(drops.MoneyToDrop-Random.Range(1, 10), drops.MoneyToDrop+Random.Range(1, 10));
		}
		if(drops.dropItems.Length > 0) {
			Inv inventory = new Inv();
			for(int i = 0; i < drops.dropItems.Length; i++) {
				inventory.DropItem(Inv.FindItem((int)drops.dropItems[i].itemType), transform.position+new Vector3(0, 1, 0));
			}
		}
	}

}
