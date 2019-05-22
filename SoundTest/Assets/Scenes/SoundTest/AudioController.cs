using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public AudioClip se1;
	public AudioClip se2;
	public AudioSource se_aud;
	public AudioSource bgm_aud;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick_SE1Play() {
		this.se_aud.PlayOneShot(this.se1);
	}

	public void OnClick_SE2Play() {
		this.se_aud.PlayOneShot(this.se2);
	}

	public void OnClick_BGMPlay() {
		this.bgm_aud.Play();
	}

	public void OnClick_BGMStop() {
		this.bgm_aud.Pause();
	}
}
