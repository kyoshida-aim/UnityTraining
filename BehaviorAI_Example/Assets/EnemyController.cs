using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {


	public NavMeshAgent agent;
	public GameObject target;

	string state = "patrol";
	float span = 3.0f;
	float delta = 0;
	float lostTimeSpan = 0.5f;
	int patrolIndex = 0;
	Vector3[] patrolPoint = new Vector3[4];

	void Start () {
		patrolPoint[0] = new Vector3(-3.0f, 0.5f, 1.0f);
		patrolPoint[1] = new Vector3(-3.0f, 0.5f, 7.1f);
		patrolPoint[2] = new Vector3(-9.0f, 0.5f, 7.4f);
		patrolPoint[3] = new Vector3(-9.9f, 0.5f, 0.8f);
	}

	// Update is called once per frame
	void Update () {
		Debug.Log(this.state);
		if (this.state == "patrol") {
			this.delta += Time.deltaTime;
			if (this.delta > this.span) {
				this.delta = 0;
				agent.SetDestination(patrolPoint[patrolIndex]);
				patrolIndex = (patrolIndex + 1) % 4;
			}
		} else if (this.state == "missing") {
			this.delta += Time.deltaTime;
			if (this.delta > this.lostTimeSpan) {
				this.delta = 0;
				this.state = "lost";
			}
		} else if (this.state == "lost") {
			this.delta += Time.deltaTime;
			if (this.delta > this.lostTimeSpan) {
				this.delta = 0;
				this.state = "patrol";
			}
		}
	}

	void OnTriggerStay(Collider col) {
		//　プレイヤーキャラクターを発見
		if(col.tag == "Player" ) {
			if (Physics.Linecast(transform.position,
				col.gameObject.transform.position)) {
				this.delta = 0;
				this.state = "found";
				// Debug.Log("Found");
			} else if (this.state == "found") {
				this.delta += Time.deltaTime;
				if (this.delta > this.lostTimeSpan) {
					this.delta = 0;
					this.state = "lost";
				}
			}
		}
	}
	
	void OnTriggerExit(Collider col) {
		if(col.tag == "Player" && this.state == "found") {
				this.state = "missing";
		}
	}
}
