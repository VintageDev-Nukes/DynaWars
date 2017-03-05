using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class AIScript : MonoBehaviour
{
    #region variables
    #region cached variables
    CharacterController _controller;
    Transform player;
	Transform _eyes;
    #endregion
    #region movement variables
    public float speed = 5;
    float gravity = 100;
    Vector3 moveDirection;
    float maxRotSpeed = 200.0f;
    float minTime = 0.1f;
    float velocity;
    float range;
    float attackRange;
    bool isCorouting;
    #endregion
    #region waypoint variables
    int index;
    public string strTag;
    Dictionary<int, Transform> waypoint = new Dictionary<int, Transform>();
    #endregion
    #region delegate variable
    delegate void DelFunc();
    delegate IEnumerator DelEnum();
    DelFunc delFunc;
    DelEnum delEnum;
    bool del;
    #endregion
    #endregion
    bool seenAround;
    int layerMask = 1 << 8;

	static bool isAttacking = false;
	static float lastAttack;

	public float damage = 10;

	public string idleAnim = "idle";
	public string victoryAnim = "victory";
	public string walkAnim = "walk";
	public string attackOwn = "attackOwn";
	public string attack = "charge";
	
	public float life = 80;

	//This is the range where the mob deletes
	public float deleteRange = 50;

	//The probability of make a critic hit
	public float criticProb = 10;

	public Vector3 playerpos, vVelocity, lastPosition, curPosition;
	public float _velocity;

	private bool noPlayer;

    void Start()
    {

		Entity newEnt = new Entity();

		newEnt.life = life;

		transform.name = "Mob"+Entity.MobNum;
		gameObject.layer = 13;

		Entity.MobNum++;

		Physics.IgnoreLayerCollision(13, 12);
		Physics.IgnoreLayerCollision(12, 14);
		Physics.IgnoreLayerCollision(12, 12);
		Physics.IgnoreLayerCollision(12, 11);

		EntityLib.Entities.Add(transform.name, newEnt);

        _controller = GetComponent<CharacterController>();
		_eyes = transform.Find ("Eyes");
		try {
        	player = GameObject.Find("Player").transform;
		} catch {
			Debug.Log("No player on scene");
			noPlayer = true;
			return;
		}
        if (string.IsNullOrEmpty(strTag)) 
            Debug.LogError("No waypoint tag given");
        
        index = 0;
        range = 2.5f; attackRange = 200f;

		Transform spawn = GameObject.Find(strTag).transform;
		for(int i = 0; i < spawn.childCount; i++)
        {
			Transform wp = spawn.GetChild(i);
            WaypointScript script = wp.GetComponent<WaypointScript>();
            waypoint.Add(script.index, wp);
        }
		//animation[victoryAnim].wrapMode = WrapMode.Once;

        delFunc = this.Walk;
        delEnum = null;
        del = true;
        
        isCorouting = false;
        seenAround = false;
        layerMask = ~layerMask;
    }

    void Update()
    {
		if(noPlayer) {
			return;
		}
		if(isAttacking && Time.time > lastAttack+animation[attack].length && Vector3.Distance(player.position, transform.position) < 1.75f && !PlayerStats.Killed) {
			attacking();
		}
        if (AIFunction() && isCorouting)
        {
            StopAllCoroutines();
            del = true;
        }
        if (del)
            delFunc();
        else if (!isCorouting)
        {
            isCorouting = true;
            StartCoroutine(delEnum());
        }
		if(life <= 0) {
			Die();
		}

		if((player.position - transform.position).magnitude > deleteRange) {
			GameObject spawnAttached = GameObject.Find(strTag);
			Destroy(gameObject);
			Destroy(spawnAttached);
		}

    }

	void Die() {

		//Drop everything
		GetComponent<DropScript>().BroadcastMessage("Drop");

		//Make the mob a ragdoll
		Physics.IgnoreLayerCollision(12, 8);

		RagdollBuilder ragdollbuild = gameObject.AddComponent<RagdollBuilder>();

		RagdollExt.PlayerToRagdoll(ragdollbuild, gameObject).CreateAll();
		GameObject spawnAttached = GameObject.Find(strTag);
		Destroy(gameObject, 15);
		Destroy(spawnAttached, 15);
		enabled = false;
	}

    #region movement functions
    void Move(Transform target)
    {
        //Movements
        moveDirection = transform.forward;
        moveDirection *= speed;
        moveDirection.y -= gravity * Time.deltaTime;
        _controller.Move(moveDirection * Time.deltaTime);
        //Rotation
        var newRotation = Quaternion.LookRotation(target.position - transform.position).eulerAngles;
        var angles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(angles.x,
            Mathf.SmoothDampAngle(angles.y, newRotation.y, ref velocity, minTime, maxRotSpeed), angles.z);
    }

    void NextIndex()
    {
        if (++index == waypoint.Count) index = 0;
    }

    void Walk()
    {
        if ((transform.position - waypoint[index].position).sqrMagnitude > range)
        {
            Move(waypoint[index]);
            animation.CrossFade(walkAnim);
        }
        else
        {
            switch (index)
            {
                /*case 0:
                    del = false;
                    isCorouting = false;
                    delEnum = this.Victory;
                    break;*/
                case 0:
                    del = false;
                    isCorouting = false;
                    delEnum = this.Wait;
                    break;
                default:
                    NextIndex(); break;
            }
        }
    }

    void Attack()
    {
		bool final = true;
        if ((transform.position - player.position).sqrMagnitude > range)
        {
            Move(player);
            animation.CrossFade(attack);
			isAttacking = true;
			final = false;
        }
        else
        {
            animation.CrossFade(attackOwn);
			isAttacking = false;
			final = false;
        }
		if(final) {
			isAttacking = false;
		}
    }

	void attacking() {
		int dmg = Random.Range((int)damage-Random.Range(1, 5), (int)damage+Random.Range(1, 5));
		bool critic = false;
		if(RandomExt.PRand(criticProb)) {
			critic = true;
			dmg *= (Random.Range(15, 25)/10);
		}
		player.GetComponent<DamageSystem>().MakeDamage(dmg, critic);
		lastAttack = Time.time;
	}
	
	#endregion
   
    #region animation functions
    /*IEnumerator Victory()
    {
		if (!animation.IsPlaying(victoryAnim)) animation.CrossFade(victoryAnim);
		yield return new WaitForSeconds(animation[victoryAnim].length);
        NextIndex();
        del = true;
    }*/
    
    IEnumerator Wait()
    {
        animation.CrossFade(idleAnim);
        yield return new WaitForSeconds((float)Random.Range(2, 10));
        NextIndex();
        del = true;
    }
    #endregion
    
    #region AI function
    bool AIFunction(){
        Vector3 direction = player.position - transform.position;
        if (direction.sqrMagnitude < attackRange){
            if (seenAround){
                delFunc = this.Attack;
                return true;
            } else{
                if (Vector3.Dot(direction.normalized, transform.forward) > 0 &&
                    !Physics.Linecast(_eyes.position, player.position, layerMask)){
	                    delFunc = this.Attack;
	                    seenAround = true;
	                    return true;
                }
                return false;
            }
        }else{
            delFunc = this.Walk;
            seenAround = false;
            return false;
        }
    }
    #endregion

	void FixedUpdate() {
		if(noPlayer) {
			return;
		}
		curPosition = player.transform.position;
		vVelocity = (lastPosition - curPosition) / Time.deltaTime;
		_velocity = (lastPosition - curPosition).magnitude / Time.deltaTime;
		lastPosition = player.transform.position;
		EntityLib.Entities[transform.name].velocity = _velocity;
	}
}