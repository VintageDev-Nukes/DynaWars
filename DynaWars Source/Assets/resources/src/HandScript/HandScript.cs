using UnityEngine;
using System.Collections;

public enum HandState {None, Open, Close, Punch, Holding, Ok, Finger}
public enum HandPosition {Left, Right}

public class Hands {

	public Hands(Hand left, Hand right) {
		leftHand = left;
		rightHand = right;
	}

	public Hand leftHand;
	public Hand rightHand;

}

public class Hand {
	public HandPosition myPosition;
	public HandState myState;
	public Vector3 handPosition;
	public Vector3 handRotation;
}
