var target : Transform; 

var distanceMin = 10.0; 

var distanceMax = 15.0; 

var distanceInitial = 12.5; 

var scrollSpeed = 1.0; 

 

var xSpeed = 250.0; 

var ySpeed = 120.0; 

 

var yMinLimit = -20; 

var yMaxLimit = 80; 

 

private var x = 0.0; 

private var y = 0.0; 

private var distanceCurrent = 0.0; 

 

@script AddComponentMenu ("Camera-Control/Key Mouse Orbit") 

 

function Start () { 

    calibrateAccelerometer();

   

   var angles = transform.eulerAngles; 

    x = angles.y; 

    y = angles.x; 

 

   distanceCurrent = distanceInitial; 

 

   // Make the rigid body not change rotation 

      if (rigidbody) 

      rigidbody.freezeRotation = true; 

} 

 

function LateUpdate () { 

    if (target) { 

        x -= getAccelerometer(Input.acceleration).y * xSpeed * Time.deltaTime; 

        y += getAccelerometer(Input.acceleration).x * ySpeed * Time.deltaTime; 

       distanceCurrent -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed; 

 

    if (Input.touchCount > 1) {

        var touch : Touch  = Input.GetTouch(0);

        var touch2 : Touch = Input.GetTouch(1); 

 

        // Find out how the touches have moved relative to eachother: 

        var curDist: Vector2 = touch.position - touch2.position; 

        var prevDist : Vector2 = (touch.position - touch.deltaPosition) - (touch2.position - touch2.deltaPosition); 

 

        distanceCurrent -= (curDist.magnitude - prevDist.magnitude)/2; 

    }

      

      distanceCurrent = Mathf.Clamp(distanceCurrent, distanceMin, distanceMax); 

       y = ClampAngle(y, yMinLimit, yMaxLimit); 

              

        var rotation = Quaternion.Euler(y, x, 0); 

        var position = rotation * Vector3(0.0, 0.0, -distanceCurrent) + target.position; 

        

        transform.rotation = rotation; 

        transform.position = position; 

    } 

} 

 

var calibrationMatrix : Matrix4x4;

 

static function ClampAngle (angle : float, min : float, max : float) { 

   if (angle < -360) 

      angle += 360; 

   if (angle > 360) 

      angle -= 360; 

   return Mathf.Clamp (angle, min, max); 

}

 

function calibrateAccelerometer(){ 

   var wantedDeadZone : Vector3  = Input.acceleration; 

   var rotateQuaternion : Quaternion  = Quaternion.FromToRotation(new Vector3(0, 0, -1), wantedDeadZone); 

       

   //create identity matrix ... rotate our matrix to match up with down vec 

   var matrix : Matrix4x4 = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1, 1, 1)); 

 

   //get the inverse of the matrix 

   this.calibrationMatrix = matrix.inverse; 

} 

    

//Whenever you need an accelerator value from the user 

//call this function to get the 'calibrated' value 

function getAccelerometer(accelerator : Vector3 ) : Vector3 { 

   var accel : Vector3  = this.calibrationMatrix.MultiplyVector(accelerator); 

   return accel; 

}