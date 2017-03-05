using UnityEngine;
using System.Collections;

public class PunchScript : MonoBehaviour {

	public float damage = 10;
	public float criticProb = 50;
	public float delaybetweenpunchs = 0.5f;

	float t;

	void Update() {
		RaycastHit hit;
		if(Input.GetMouseButtonDown(0) && t > delaybetweenpunchs) {
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2.5f)) {
				if(hit.transform.GetComponent<DamageSystem>() != null) {

					int dmg = Random.Range((int)damage-Random.Range(1, 5), (int)damage+Random.Range(1, 5));

					bool critic = false;
					
					if(RandomExt.PRand(criticProb)) {
						critic = true;
						dmg *= (Random.Range(15, 25)/10);
					}

					hit.transform.GetComponent<DamageSystem>().MakeDamage(dmg, critic);

				}
			}
			t = 0;
		}
		t += Time.deltaTime;
	}

}
