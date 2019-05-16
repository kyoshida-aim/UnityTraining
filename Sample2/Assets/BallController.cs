using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("called");
		GetComponent<Rigidbody2D>().AddForce(new Vector2(500.0f, 300.0f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
