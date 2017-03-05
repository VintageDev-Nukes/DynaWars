using UnityEngine;
using System.Collections;

public class Damage {

	private static int _dmgNum;

	public static int DmgNum {
		get {return _dmgNum;}
		set {_dmgNum = value;}
	}

	private static bool _damaged = false;
	private static string _lastCol = "";
	private static float _lastTerCol = 0;
	
	public static bool Damaged {
		get {return _damaged;}
		set {_damaged = value;}
	}

	public static string lastCol {
		get {return _lastCol;}
		set {_lastCol = value;}
	}

	public static float lastTerCol {
		get {return _lastTerCol;}
		set {_lastTerCol = value;}
	}

	public static void DisplayDmg(Transform mainobj, float dmg, bool critic = false) {
		Color color = Color.yellow;
		GameObject dmgtxt = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/DmgText"));
		if(mainobj.tag == "Player") {
			color = new Color(1, 0.5f, 0.5f);
		}
		if(critic) {
			color = new Color(1, 0.5f, 0);
			Vector3 actualScale = dmgtxt.transform.localScale;
			dmgtxt.transform.localScale = new Vector3(actualScale.x+0.05f, actualScale.y+0.05f, actualScale.z);
		}
		Vector3 org = mainobj.transform.position;
		dmgtxt.transform.position = org;
		//dmgtxt.transform.position = new Vector3(org.x+Random.Range(-3, 3), org.y, org.z+Random.Range(-3, 3));
		dmgtxt.transform.name = "DmgTxt"+Damage.DmgNum;
		Damage.DmgNum++;
		//dmgtxt.transform.LookAt(Player.PlayerObj.transform);
		TextMesh Mdmgtxt = dmgtxt.GetComponent<TextMesh>();
		Mdmgtxt.text = ((int)dmg).ToString();
		Mdmgtxt.color = color;
		GameObject.Destroy(dmgtxt, 3.5f);
	}

}

public class DamageSounds {

	public static IEnumerator HurtSound(Transform obj) {
		AudioClip hurt = Resources.Load<AudioClip>("sounds/hurt");
		AudioManager.Play(hurt, obj.position, 0.5f, 1+(Random.Range(-10, 10)/10));
		yield return new WaitForSeconds(hurt.length);
	} 
	
	public static IEnumerator DamageSound(Transform obj) {
		AudioClip damage = Resources.Load<AudioClip>("sounds/punch");
		AudioManager.Play(damage, obj.position);
		yield return new WaitForSeconds(damage.length);
	}

	public static IEnumerator FallDmgSound(Transform obj) {
		AudioClip falldmg = Resources.Load<AudioClip> ("sounds/fall");
		AudioManager.Play(falldmg, obj.position);
		yield return new WaitForSeconds(falldmg.length);
	}
	
}

public class DamageSystem : MonoBehaviour {

	public float defaultKnockBack = 200, mass = 3, velocity = 0; // defines the character mass

	Vector3 impact = Vector3.zero;

	// Update is called once per frame
	void Update () {

		//Si el tag del objeto al que este script esta attacheado es el jugador entonces...
		if(transform.tag == "Player") {

			//si no esta tocando el suelo y la ultima lectura dice que no esta cayendo...
			if(!transform.GetComponent<CharacterController>().isGrounded && !PlayerStats.IsFalling) {
				//Se guarda la posicion de caida
				PlayerStats.FallPoint = transform.position;
				//Y se activa esta bool para que la proxima vez no se lea y directamente se pase al siguiente bloque...
				PlayerStats.IsFalling = true;
			}

			//Esta velocidad siempre se leera y cuando la segunda condicion se cumpla entonces dejara de leerse asi bloqueando el resultado y evitanto que de 0
			if(PlayerStats.Velocity > 0) { //For avoid errors
				velocity = PlayerStats.Velocity;
			}

			//Este es el siguiente bloque, aqui el jugador ya ha golpeado el suelo, pero el script detecta que el jugador sigue cayendo, por tanto hay que aplicar el daño de impacto
			if(transform.GetComponent<CharacterController>().isGrounded && PlayerStats.IsFalling) {

				//Por tanto ya se empieza a guardar las coordenadas del punto B (la distancia de caida)
				PlayerStats.FallHeight = transform.position;

				//Aqui esta la distancia caida en el eje y
				float distanceFalled = PlayerStats.FallPoint.y-PlayerStats.FallHeight.y;

				//Si es mayor de 5 y la velocidad que llevaba el jugador antes del impacto es mayor de x, aplicar un daño 
				if(distanceFalled > 5 && velocity > 10) {
					float tmplife = PlayerStats.Health;
					float dmg = 0;
					if(!PlayerStats.invincible) {
						dmg = Mathf.Pow((distanceFalled-5)/4, 2)*velocity; // *difficulty //La formula
					}
					PlayerStats.Health -= dmg; //Restar vida
					StartCoroutine(DamageSounds.FallDmgSound(Player.PlayerObj.transform)); //Reproducir sonido de daño en caida
					if(tmplife > 0) {
						Damage.DisplayDmg(transform, dmg); //Mostrar texto
					}
				}

				//Ya se ha dejado de caer por tanto volver al inicio
				PlayerStats.IsFalling = false;
				
			}
			
		} else { //Lo mismo para un mob, solo que en vez de usar la variable playerstats.health, se usa el AIScript.
			
			if(!transform.GetComponent<CharacterController>().isGrounded && !EntityLib.Entities[transform.name].isFalling) {
				if(!EntityLib.Entities[transform.name].isFalling) {
					EntityLib.Entities[transform.name].fallPoint = transform.position;
					EntityLib.Entities[transform.name].isFalling = true;
				}
			}

			if(EntityLib.Entities[transform.name].velocity > 0) {
				velocity = EntityLib.Entities[transform.name].velocity;
			}
			
			if(transform.GetComponent<CharacterController>().isGrounded && EntityLib.Entities[transform.name].isFalling) {
				EntityLib.Entities[transform.name].fallHeight = transform.position;
				float distanceFalled = EntityLib.Entities[transform.name].fallPoint.y-EntityLib.Entities[transform.name].fallHeight.y;
				if(distanceFalled > 5 && velocity > 10) {
					float dmg = Mathf.Pow((distanceFalled-5)/4, 2)*velocity; // /difficulty
					float tmplife = 0;
					if(transform.GetComponent<AIScript>() != null) {
						tmplife = transform.GetComponent<AIScript>().life;
						transform.GetComponent<AIScript>().life -= dmg;
						StartCoroutine(DamageSounds.FallDmgSound(transform));
					}
					if(tmplife > 0) {
						Damage.DisplayDmg(transform, dmg);
					}
				}
				
				EntityLib.Entities[transform.name].isFalling = false;
				
			}
			
		}
		
	}

	public void MakeDamage(int dmg, bool critic = false, Vector3? varVector = null, float? varPushBack = null, Object varhit = null) {

		Vector3 direction = Vector3.zero;

		float pushBack = defaultKnockBack;

		Transform hit = transform;

		if(varVector != null) {
			direction = (Vector3)varVector;
		}

		if(varPushBack != null) {
			pushBack = (float)varPushBack;
		}

		if(varhit != null) {
			hit = (Transform)varhit;
		}

		if(hit.tag == "Player") {
			PlayerStats.Health -= dmg;
			StartCoroutine(DamageSounds.HurtSound(hit));
			Damage.DisplayDmg(transform, dmg, critic);
		} else {
			if(hit.GetComponent<AIScript>() != null) {
				hit.GetComponent<AIScript>().life -= dmg;
				StartCoroutine(DamageSounds.DamageSound(hit));
				Damage.DisplayDmg(transform, dmg, critic);
			} else {
				return;
			}
		}

		if(varPushBack != null && varVector != null) {
			StartCoroutine(KnockBack(hit, direction, pushBack));
		}

	}

	public IEnumerator KnockBack(Transform obj, Vector3 dir, float force) { //Pushback

		float t = 0;

		CharacterController character = obj.GetComponent<CharacterController>();

		while(t < 1) {

			yield return null;

			// apply the impact force:
			if (impact.magnitude > 0.2) character.Move(impact * Time.deltaTime);
			// consumes the impact energy each cycle:
			impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);

			dir.Normalize();
			if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
			impact += dir.normalized * force / mass;

			t += Time.deltaTime;

		}
	}

}