using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//Ignorar coliciones entre capas (aunque esto sobra)
		Physics.IgnoreLayerCollision(15, 8);
		Physics.IgnoreLayerCollision(15, 9);
		Physics.IgnoreLayerCollision(15, 10);
		Physics.IgnoreLayerCollision(15, 11);
		Physics.IgnoreLayerCollision(15, 12);
		Physics.IgnoreLayerCollision(15, 13);
		Physics.IgnoreLayerCollision(15, 14);
		Physics.IgnoreLayerCollision(15, 15);

		Vector3 curPos = transform.position; //Actual posicion
		Vector3 newPos = new Vector3(curPos.x, curPos.y+(Random.Range(15, 50)/10), curPos.z); //Nueva posicion
		transform.LookAt(Player.PlayerObj.transform.position); //Mirar hacia el jugador
		transform.rotation *= Quaternion.Euler(0,180,0); //Rotar 180 grados para que se vea de la forma correcta
		StartCoroutine(Vector3Ext.MoveFromTo(transform, curPos, newPos, 0.5f)); //Mover hacia una coordenada
	}

}
