using UnityEngine;
using System.Collections;

public class HandSystem : MonoBehaviour {

	public Transform leftHand;
	public Transform rightHand;

	[HideInInspector]
	public Hands currentHand;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		leftHand.position = currentHand.leftHand.handPosition;
		rightHand.position = currentHand.rightHand.handPosition;
		leftHand.eulerAngles = currentHand.leftHand.handRotation;
		rightHand.eulerAngles = currentHand.rightHand.handRotation;
	}
}
