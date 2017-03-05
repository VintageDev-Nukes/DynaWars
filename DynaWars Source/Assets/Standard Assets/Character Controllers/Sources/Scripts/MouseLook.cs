using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add a rigid body to the capsule
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSWalker script to the capsule

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

	public string targetName;

	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationX = 0F;
	float rotationY = 0F;
	
	Quaternion originalRotation;

	public bool IsFixed = false;

	float x;
	float z;
	float y;

	private Transform raiz;
	private Transform target;

	public float r = 3;

	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody) {
			rigidbody.freezeRotation = true;
		}
		
		originalRotation = transform.localRotation;

		raiz = transform.root;
		
		if(targetName.Equals(System.String.Empty)) {
			
			foreach(Transform child in raiz) 
			{
				if(child.gameObject.GetComponent<Camera>() != null) {
					target = child;
					return;
				}
				foreach(Transform subchild in child) 
				{
					if(subchild.gameObject.GetComponent<Camera>() != null) {
						target = subchild;
						return;
					}
					foreach(Transform subchild2 in subchild) {
						if(subchild2.gameObject.GetComponent<Camera>() != null) {
							target = subchild2;
							return;
						}
					}
				}
			}
			
		} else {
			
			target = FindAllChilds(raiz, targetName);
			
		}
		
	}

	void Update ()
	{

		float x0 = transform.root.position.x;
		float y0 = transform.root.position.y;
		float z0 = transform.root.position.z;

		if (axes == RotationAxes.MouseXAndY)
		{

			if (IsFixed) {

				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
				rotationX = ClampAngle (rotationX, minimumX, maximumX);

				rotationY += Input.GetAxis("Mouse Y") * sensitivityY/15;
				rotationY = Mathf.Clamp(rotationY, -0.5f, 2);
				
				x = x0 + r * Mathf.Cos(rotationX * Mathf.PI / 180);
				y = rotationY;
				z = z0 + r * Mathf.Sin(rotationX * Mathf.PI / 180);
			
				target.position = new Vector3(x, raiz.position.y + y, z);
				target.LookAt(raiz.position);

				//Debug.Log("["+target.name+"] Position to put: "+new Vector3(x, raiz.position.y + y, z)+", setted position:  "+target.position);

			} else {

				// Read the mouse input axis
				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

				rotationX = ClampAngle (rotationX, minimumX, maximumX);
				rotationY = ClampAngle (rotationY, minimumY, maximumY);
			
				Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
				Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
			
				transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			
			}
		}
		else if (axes == RotationAxes.MouseX)
		{

			if (IsFixed) {

				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
				rotationX = ClampAngle (rotationX, minimumX, maximumX);
				
				x = x0 + r * Mathf.Cos(rotationX * Mathf.PI / 180);
				z = z0 + r * Mathf.Sin(rotationX * Mathf.PI / 180);
				
				target.position = new Vector3(x, target.position.y, z);
				target.LookAt(raiz);
				
			} else {

				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
				rotationX = ClampAngle (rotationX, minimumX, maximumX);

				Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
				transform.localRotation = originalRotation * xQuaternion;
			
			}
		}
		else
		{

			if (IsFixed) {

				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = ClampAngle (rotationY, minimumY, maximumY);

				y = y0 + r * Mathf.Cos(rotationY * Mathf.PI / 180);
				z = z0 + r * Mathf.Sin(rotationY * Mathf.PI / 180);

				target.position = new Vector3(target.position.x, y, z);
				target.LookAt(raiz.position);

			} else {
			
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = ClampAngle (rotationY, minimumY, maximumY);
				
				Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
				transform.localRotation = originalRotation * yQuaternion;

			}

		}

	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}

	public static Transform FindAllChilds(Transform parent, string name)
	{
		if (parent.name.Equals(name)) return parent;
		foreach (Transform child in parent)
		{
			Transform result = FindAllChilds(child, name);
			if (result != null) return result;
		}
		return null;
	}

}