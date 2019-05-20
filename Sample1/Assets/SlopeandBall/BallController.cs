using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.ThrowBall();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ThrowBall() {
		GetComponent<Rigidbody>().AddForce(new Vector3(0, 200, 500));
	}
}
