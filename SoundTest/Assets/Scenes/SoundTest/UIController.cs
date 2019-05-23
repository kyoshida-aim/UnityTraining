using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject resumeButton;
	public GameObject pauseButton;
	public GameObject seButton;
	public GameObject bgmButton;

	public void Pause() {
		resumeButton.SetActive(!resumeButton.activeInHierarchy);
		pauseButton.SetActive(!pauseButton.activeInHierarchy);
		seButton.SetActive(!seButton.activeInHierarchy);
		bgmButton.SetActive(!bgmButton.activeInHierarchy);
	}
}
