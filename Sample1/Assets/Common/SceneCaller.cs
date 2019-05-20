using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCaller : MonoBehaviour {

	public void callMeshitero() {
		SceneManager.LoadScene("Meshitero");
	}

	public void callSlopeandBall() {
		SceneManager.LoadScene("SlopeandBall");
	}

	public void callLayerTest() {
		SceneManager.LoadScene("LayerTest");
	}
}
