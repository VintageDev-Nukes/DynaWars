using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

	public int id = 0;

	public int stacksize = 1;

	private float droppedTime;

	public bool isDropped;

	void Update() {

		gameObject.SetActive(true);

		isDropped = transform.parent == null;

		if(!isDropped) {
			if(Player.ThirdView && transform.parent.name == "mainCam") {
				gameObject.SetActive(false);
			} else if(!Player.ThirdView && transform.parent.name == "RightHand") {
				gameObject.SetActive(false);
			}
		}

		droppedTime += Time.deltaTime;
		Vector3 plPos = Player.PlayerObj.transform.position;
		float dist = Vector3.Distance(transform.position, plPos);

		if(dist < 1.5f && droppedTime > 0.5f && isDropped) {
			InventoryItem item = Inv.FindItem(id);
			item.itemstacksize = stacksize;
			if(Inv.AddItem(item)) {
				Destroy(this.gameObject);
			}
		}
	}

	void OnMouseDown() {

		InventoryItem item = Inv.FindItem(id);
		item.itemstacksize = stacksize;
		if(Inv.AddItem(item) && isDropped) {
			Destroy (this.gameObject);
		}

	}
}