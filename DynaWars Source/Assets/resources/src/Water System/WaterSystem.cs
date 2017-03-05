using UnityEngine;
using System.Collections;

public class WaterSystem {

	private bool isUnderwater = false;
	private GameObject mainPlayer;

	CharacterMotor chMotor;

	public void InfiniteWater(GameObject water, GameObject player) 
	{

		water.transform.localScale = new Vector3 (player.transform.position.x, 1, player.transform.position.z);

	}

	public void UnderWaterEffects(float waterLevel) 
	{

		/*if (!Player.ThirdView) {
			mainPlayer = Player.FPlayerObject;		
		} else {
			mainPlayer = Player.TPlayerObject;
		}*/

		if(!PlayerStats.Killed) {

			mainPlayer = Player.TPlayerObject;

			isUnderwater = mainPlayer.transform.position.y < waterLevel;
			if (isUnderwater) SetUnderwater ();
			if (!isUnderwater) SetNormal ();

		}

		//Esto tiene que ser deshabilitado

		/*if ((player.transform.position.y < waterLevel) != isUnderwater) 
		{
			isUnderwater = player.transform.position.y < waterLevel;
			if (isUnderwater) SetUnderwater (player);
			if (!isUnderwater) SetNormal (player);
		}*/
		
		/*if(isUnderwater && Input.GetKeyDown(KeyCode.Space))
		{
			player.constantForce.relativeForce = new Vector3(0, -1, 0);
		} else
		{
			player.constantForce.relativeForce = new Vector3(0, 0, 0);
		}
		
		if(isUnderwater && Input.GetKeyDown(KeyCode.R))
		{
			player.constantForce.relativeForce = new Vector3(0, 1, 0);
		}*/


	}

	void SetNormal () 
	{

		mainPlayer.GetComponentInChildren<Transform> ().Find("mainCam").GetComponent<BlurEffect>().enabled = false;
		
		chMotor = mainPlayer.GetComponent<CharacterMotor>();	
		chMotor.movement.maxForwardSpeed = 5;

		/*if (!Player.ThirdView) {
			mainPlayer = Player.FPlayerObject;		
		} else {
			mainPlayer = Player.TPlayerObject;
		}

		if(Player.ThirdView) {
		
			mainPlayer.GetComponentInChildren<Transform> ().Find("mainCam").GetComponent<BlurEffect>().enabled = false;
	
			chMotor = mainPlayer.GetComponent<CharacterMotor>();	
			chMotor.movement.maxForwardSpeed = 5;

		} else {

			mainPlayer.GetComponentInChildren<Transform> ().Find("Weapon Camera").GetComponent<BlurEffect>().enabled = false;

		}*/

	}

	//CharacterController chMotor;
	
	void SetUnderwater () 
	{

		mainPlayer.GetComponentInChildren<Transform>().Find("mainCam").GetComponent<BlurEffect>().enabled = true;
		
		chMotor = mainPlayer.GetComponent<CharacterMotor>();	
		chMotor.movement.maxForwardSpeed = 3;

		/*if (!Player.ThirdView) {
			mainPlayer = Player.FPlayerObject;		
		} else {
			mainPlayer = Player.TPlayerObject;
		}

		if(Player.ThirdView) {

			mainPlayer.GetComponentInChildren<Transform> ().Find("mainCam").GetComponent<BlurEffect>().enabled = true;

			chMotor = mainPlayer.GetComponent<CharacterMotor>();	
			chMotor.movement.maxForwardSpeed = 3;

		} else {
			
			mainPlayer.GetComponentInChildren<Transform> ().Find("Weapon Camera").GetComponent<BlurEffect>().enabled = true;
			
		}*/

		/*
		#################################
		##                             ##
		##     EDIT PLAYER MOVEMENT    ##
		##                             ##
		#################################
		 */

		//chMotor.movement.gravity = 3;
		//chMotor.jumping.baseHeight = 10;


		
	}

}
