using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delegatetest : MonoBehaviour {

	delegate void SomeDelegate(int a);
	
	// Use this for initialization
	void Start () {
		SomeDelegate a = DoSomething;
		a(256);
	}
	
	void DoSomething(int n)
	{
		Debug.Log(n.ToString() + "が呼ばれました");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
