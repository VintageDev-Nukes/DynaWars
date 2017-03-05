using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]

public class FPSInputController : MonoBehaviour
{
    private CharacterMotor motor;
	private CharacterController controller;

    // Use this for initialization
    void Awake()
    {
		controller = GetComponent<CharacterController>();
        motor = GetComponent<CharacterMotor>();
    }

	float speed;
	public float rotateSpeed = 2;
	public float speedJump = 10;

	private bool justWalking;

	[HideInInspector]
	public bool ThirdView;

    // Update is called once per frame
    void Update()
    {
		if(ThirdView) {
			speed = motor.movement.maxForwardSpeed;

			Vector3 forward = transform.TransformDirection(Vector3.forward);

			bool IsRotating = false;

			if(Input.GetKey(KeyCode.W)) {
				if(Input.GetAxis("Horizontal") != 0) {
					transform.Rotate(0, Input.GetAxis ("Horizontal") * rotateSpeed, 0);
					float curSpeed = speed * Input.GetAxis("Vertical");
					IsRotating = true;
					controller.SimpleMove(forward * curSpeed);
				}
				if(IsRotating) {
					justWalking = true;
				}
			}

			if(!IsRotating) {

				float curSpeed = 0;

				if(Input.GetKey (KeyCode.W)) {
					/*if(transform.eulerAngles.y == 0)
						transform.eulerAngles = new Vector3(0, 0, 0);*/
					if(!justWalking) {
						transform.eulerAngles = new Vector3(0, 0, 0);
					}
					curSpeed = speed * Input.GetAxis("Vertical");
					//justWalking = false;
				}

				if (Input.GetKey (KeyCode.A)) {
					transform.eulerAngles = new Vector3(0, 270, 0);
					curSpeed = speed * -Input.GetAxis("Horizontal");
					justWalking = false;
				}

				if (Input.GetKey (KeyCode.D)) {
					transform.eulerAngles = new Vector3(0, 90, 0);
					curSpeed = speed * Input.GetAxis("Horizontal");
					justWalking = false;
				}

				if (Input.GetKey (KeyCode.S)) {
					transform.eulerAngles = new Vector3(0, 180, 0);
					curSpeed = speed * -Input.GetAxis("Vertical");
					justWalking = false;
				}

				controller.SimpleMove(forward * curSpeed);
				//controller.SimpleMove(forward * speed);
			}
			
			if (Input.GetKey (KeyCode.Space) && controller.isGrounded) {
				controller.Move (new Vector3(0, speedJump * Time.deltaTime, 0));
			}

		} else {
			//First person

			// Get the input vector from kayboard or analog stick
			Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        	if (directionVector != Vector3.zero)
        	{
            	// Get the length of the directon vector and then normalize it
            	// Dividing by the length is cheaper than normalizing when we already have the length anyway
            	float directionLength = directionVector.magnitude;
            	directionVector = directionVector / directionLength;

           		 // Make sure the length is no bigger than 1
           	 	directionLength = Mathf.Min(1.0f, directionLength);

           		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
            	// This makes it easier to control slow speeds when using analog sticks
           	 	directionLength = directionLength * directionLength;

           	 	// Multiply the normalized direction vector by the modified length
          	 	 directionVector = directionVector * directionLength;
        	}


			// Apply the direction to the CharacterMotor
			motor.inputMoveDirection = transform.rotation * directionVector;
			motor.inputJump = Input.GetButton("Jump");
		}

    }

}