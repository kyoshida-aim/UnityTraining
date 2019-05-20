using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshiteroGenerator : MonoBehaviour {

	public GameObject meshiteroPrefab;
	GameObject canvas;
	public void GenerateMeshitero() {
		Debug.Log("command called");
		this.canvas = GameObject.Find("VerticalCanvas");
		GameObject obj = Instantiate(meshiteroPrefab) as GameObject;
		obj.transform.SetParent(this.canvas.transform);
	}
}
 