using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	Vector3 rot;
	Vector3 bulletrot;
	//bool collisioned = false;
	public float damage = 8;
	public float speed = 100;
	public Material[] decalTex;

	[HideInInspector]
	public float criticProb = 0;
	public float knockBack = 0.01f;

	private float duration = 10;//Vida de la bala en segundos
	
	// Use this for initialization
	/*void Start () {

		Physics.IgnoreLayerCollision(11, 11);
		Physics.IgnoreLayerCollision(12, 11);

		rigidbody.detectCollisions = true;
		rigidbody.velocity = gameObject.transform.forward * speed;
		//StartCoroutine(activeCol());
		//StartCoroutine(sendReport());

		randSpeed = Random.Range(150, 300);

		//rot = GameObject.Find ("Player").transform.FindChild ("mainCam").transform.eulerAngles;
		//bulletrot = new Vector3(0, rot.y, rot.x);
		//transform.position = GameObject.Find("Player").transform.FindChild("mainCam").transform.position;
		//transform.eulerAngles = gameObject.transform.forward;
		//Physics.IgnoreCollision(GameObject.Find("Player").transform.FindChild("mainCam").transform.FindChild("HoldSystem").collider, collider1);
		//Physics.IgnoreCollision(GameObject.Find("Player").transform.FindChild("mainCam").transform.FindChild("HoldSystem").collider, collider2);
		//Debug.Log( transform.position );
		//StartCoroutine(activeCol());
	} 

	void Update() {
		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.forward, out hit, 10) && !collisioned && Vector3.Dot(rigidbody.velocity,transform.forward) >= randSpeed){ //when we left click and our raycast hits something

			GameObject decal = (GameObject)Resources.Load("Prefabs/Decal");
			Instantiate(decal, new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z), Quaternion.FromToRotation(Vector3.up, hit.normal)); //then we'll instantiate a random bullet hole texture from our array and apply it where we click and adjust

			decal.GetComponent<MeshRenderer>().material = decalTex[Random.Range(0, decalTex.Length)];

			float newRandSpeed = randSpeed / Random.Range(2, 5);

			if(newRandSpeed >= 25) {
				rigidbody.velocity = gameObject.transform.forward * newRandSpeed;
			} else {
				Destroy(this.gameObject);
			}
			
		}
	}

	void FixedUpdate() {

		if (!collisioned) {
			Vector3 finalSpeed = gameObject.transform.forward * (speed + GunStats.fireSpeed);
			rigidbody.AddForce(finalSpeed);
		} else {
			rigidbody.useGravity = true;
			collisioned = true;
			//transform.GetComponent<Damager>().enabled = false;
			Destroy (this.gameObject, 3);
		}
		if (!collisioned) {
			Destroy (this.gameObject, 10);
		}

		//Debug.Log(Vector3.Dot(rigidbody.velocity,transform.forward));

	}

	IEnumerator activeCol() {

		yield return new WaitForSeconds(0.1f);

		string bulletType = transform.name;

		if(bulletType.Substring(0, 4) == "bala") {
			transform.GetComponent<BoxCollider>().enabled = true;
		} else if(bulletType.Substring(0, 4) == "plom") {
			transform.GetComponent<SphereCollider>().enabled = true;
		}
	}

	IEnumerator sendReport() {
		
		yield return new WaitForSeconds(9.9f);
		
		Debug.Log(transform.position);

	}

	void OnCollisionEnter(Collision collision) {
		if (!collisioned) {

			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero; 
			transform.GetComponent<TrailRenderer>().enabled = false;

			/*if(Vector3.Dot(rigidbody.velocity,transform.forward) >= Random.Range(150, 300)) {
				Instantiate((GameObject)Resources.Load("Prefabs/Decal"), transform.position, Quaternion.FromToRotation(Vector3.up, transform.position));
				Destroy(this.gameObject);
			}

		}

		int dmg = Random.Range((int)damage-Random.Range(1, 5), (int)damage+Random.Range(1, 5));
		bool critic = false;
		if(RandomExt.PRand(criticProb)) {
			critic = true;
			dmg *= (Random.Range(15, 25)/10);
		}
		collision.transform.GetComponent<DamageSystem>().MakeDamage(dmg, collision.transform, critic);
		
		collisioned = true;

		/*if (collision.gameObject.name == "Player") {
			transform.GetComponent<BoxCollider> ().isTrigger = true;
		} else {
			transform.GetComponent<BoxCollider>().isTrigger = false;
		}
		//Debug.Log(collision.gameObject.name);
	}*/
	
	//public float speed = 1.0;//Velocidad de vuelo de la bala
	//public string TargetTag = "Player";//Objetivo de la bala
	//public int damage = 10;//Daño de la bala
	//public LayerMask mask;//Filtro para decir que mascara detectara y cual no
	
	private Vector3 newPos = Vector3.zero;
	private Vector3 oldPos = Vector3.zero;
	private bool hasHit = false;
	private Vector3 direction = Vector3.zero;

	void Start(){
		newPos = transform.position; 
		oldPos = newPos;
		Destroy(this.gameObject, duration);//Iniciamos la cuenta atras del objeto
	}
	
	void Update(){

		if (hasHit){
			Destroy(this.gameObject);
			return;
		}	
		
		newPos += (transform.forward*speed + direction) * Time.deltaTime;
		
		Vector3 dir = newPos - oldPos;
		float dist = dir.magnitude;
		
		dir /= dist;
		
		/*if (dist > 0){
			if(Physics.Raycast(oldPos, dir, out hit, dist, layerMask)){
				if(hit.collider.tag==TargetTag){
					hit.collider.gameObject.BroadcastMessage("SetDamage", damage, SendMessageOptions.RequireReceiver);
				}
			}
		}	*/

		//Tomamos la distancia de la posicion nueva y la vieja
		RaycastHit hit;
		
		if(Physics.Raycast(oldPos, dir, out hit, 1, Physics.DefaultRaycastLayers) && dist > 0){//Si toco algo... //transform.position, Vector3.forward, out hit, 1
			if(hit.transform.GetComponent<AIScript>() != null){//Y ese algo es nuestro objetivo...

				//Debug.Log(hit.transform.name);

				//Le mandamos un mensaje diciendole que le dimos
				int dmg = Random.Range((int)damage-Random.Range(1, 5), (int)damage+Random.Range(1, 5));

				bool critic = false;

				if(RandomExt.PRand(criticProb)) {
					critic = true;
					dmg *= (Random.Range(15, 25)/10);
				}

				hit.transform.GetComponent<DamageSystem>().MakeDamage(dmg, critic, dir, knockBack, null);

				GameObject decal = (GameObject)Resources.Load("Prefabs/Decal");
				Instantiate(decal, new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z), Quaternion.FromToRotation(Vector3.up, hit.normal)); //then we'll instantiate a random bullet hole texture from our array and apply it where we click and adjust
				
				decal.GetComponent<MeshRenderer>().material = decalTex[Random.Range(0, decalTex.Length)];
				
				Destroy(this.gameObject);
				//Le avisamos a la bala que le dimos a algo
			} else {

				float RandSpeed = speed / Random.Range(2, 5);
				
				if(RandSpeed >= speed/4) {
					speed = RandSpeed;
					return;
				} else {

					GameObject decal = (GameObject)Resources.Load("Prefabs/Decal");
					Instantiate(decal, new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z), Quaternion.FromToRotation(Vector3.up, hit.normal)); //then we'll instantiate a random bullet hole texture from our array and apply it where we click and adjust
					
					decal.GetComponent<MeshRenderer>().material = decalTex[Random.Range(0, decalTex.Length)];
					
					Destroy(this.gameObject);
				}
			}
			hasHit = true;
		}

		oldPos = transform.position;  
		transform.position = newPos;

		//transform.Translate(Vector3.forward * Time.deltaTime * speed);
		//transform.TransformDirection(Vector3.forward * speed);
		
	}



}// end of class BulletScript