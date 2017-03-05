using System;
using System.Collections;
using UnityEngine;

public class DelaySystem : MonoBehaviour {

	public void Delay( float waitTime, Action act )
	{
		StartCoroutine( DelayImpl( waitTime, act ) );
	}
	
	public IEnumerator DelayImpl( float waitTime, Action act )
	{
		yield return new WaitForSeconds( waitTime );
		act();
	}

}
