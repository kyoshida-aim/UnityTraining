using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {

	public AudioClip se1;
	public AudioClip se2;

	public AudioSource se_aud;
	public AudioSource bgm_aud;

	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;

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

	public void Pause() {
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		Lowpass();
	}

	void Lowpass() {
		if (Time.timeScale == 0) {
			paused.TransitionTo(.01f);
		} else {
			unpaused.TransitionTo(.01f);
		}
	}
}
