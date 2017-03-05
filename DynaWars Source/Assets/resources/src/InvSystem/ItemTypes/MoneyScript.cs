using UnityEngine;
using System.Collections;

public class MoneyScript : MonoBehaviour {

	public int Quantity = 0;
	private float droppedTime;
	//private float duration = 3;
	private float start;
	//private float t;

	private MoneySystem money;

	void Start() {
		money = GameObject.Find("GameScripts").GetComponent<MoneySystem>();
		//money.pickedTime = Time.time;
	}
	
	void Update() {

		droppedTime += Time.deltaTime;

		Vector3 plPos = Player.PlayerObj.transform.position;
		float dist = (transform.position - plPos).sqrMagnitude;//Vector3.Distance(transform.position, plPos);
		
		if(dist < 3.5f && droppedTime > 0.5f) {
			//droppedTime += Time.deltaTime;
			//t = 0;
			money.ChangeMoney((ulong)Quantity);
			Destroy (this.gameObject);
		}

	}
	
	void OnMouseDown() {

		//droppedTime += Time.deltaTime;
		//t = 0;
		money.ChangeMoney((ulong)Quantity);
		Destroy (this.gameObject);

		//GUIExt guiExt = GameObject.Find("GameScripts").GetComponent<GUIExt>();

		 //guiExt.FadeColorAndRestore(GUIStyles.MoneyStyle.normal.textColor, new Color(1, 0.85f, 0), 2, 1);

		/*GUIStyles.MoneyStyle.normal.textColor = Color.Lerp(new Color(0.5f, 0.5f, 0.5f), new Color(0.85f, 0.85f, 0.85f), 1);
		GameObject.Find("GameScripts").GetComponent<DelaySystem>().Delay(3, () => {
			GUIStyles.MoneyStyle.normal.textColor = Color.Lerp(new Color(0.85f, 0.85f, 0.85f), new Color(0.5f, 0.5f, 0.5f) 1);
		});*/

	}

}
