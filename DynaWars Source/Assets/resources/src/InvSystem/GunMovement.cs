using UnityEngine;
using System.Collections;

public class GunMovement : MonoBehaviour {
	
	public float MoveAmount = 1;
	public float MoveSpeed = 2;
	GameObject GUN;
	public float MoveOnX;
	public float MoveOnY;
	Vector3 DefaultPos;
	public Vector3 NewGunPos;

	// Use this for initialization
	void Start () {
		DefaultPos = transform.localPosition;
		DefaultPos = new Vector3 (0.3210213f, -0.1649876f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if(SelectedSlot.CarriedObject != null && !Menus.pauseMenu) {
			GUN = SelectedSlot.CarriedObject;
			MoveOnX = Input.GetAxis("Mouse X") * MoveAmount * Time.deltaTime;
			MoveOnY = Input.GetAxis("Mouse Y") * MoveAmount * Time.deltaTime;
			NewGunPos = new Vector3 ( DefaultPos.x + MoveOnX, DefaultPos.y + MoveOnY, DefaultPos.z);
			GUN.transform.localPosition = Vector3.Lerp(GUN.transform.localPosition, NewGunPos , MoveSpeed * Time.deltaTime);
		}
	}
}
