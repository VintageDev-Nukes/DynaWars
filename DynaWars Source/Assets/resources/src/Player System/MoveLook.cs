using UnityEngine;
using System.Collections;

public class MoveLook : MonoBehaviour {
	
	public float lookSpeed = 10;
	private Vector3 curLoc;
	private Vector3 prevLoc;
	
	void Update () 
	{
		InputListen();
		transform.rotation = Quaternion.Lerp (transform.rotation,  Quaternion.LookRotation(transform.position - prevLoc), Time.fixedDeltaTime * lookSpeed);
	}
	
	private void InputListen()
	{
		prevLoc = curLoc;
		curLoc = transform.position;
		
		if(Input.GetKey(KeyCode.A))
			curLoc.x -= 1 * Time.fixedDeltaTime;
		if(Input.GetKey(KeyCode.D))
			curLoc.x += 1 * Time.fixedDeltaTime;
		if(Input.GetKey(KeyCode.W))
			curLoc.z += 1 * Time.fixedDeltaTime;
		if(Input.GetKey(KeyCode.S))
			curLoc.z -= 1 * Time.fixedDeltaTime;
		
		transform.position = curLoc;
		
	}
}