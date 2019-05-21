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
	float lostTimeSpan = 3.0f;
	int patrolIndex = 0;
	Vector3[] patrolPoint = new Vector3[4];

	void Start () {
		patrolPoint[0] = new Vector3(-2.7f, 0.5f, 0.6f);
		patrolPoint[1] = new Vector3(-2.7f, 0.5f, 7.1f);
		patrolPoint[2] = new Vector3(-10.0f, 0.5f, 7.4f);
		patrolPoint[3] = new Vector3(-10.0f, 0.5f, 0.6f);
	}

	// Update is called once per frame
	void Update () {
		if (this.state == "patrol") {
			this.delta += Time.deltaTime;
			if (this.delta > this.span) {
				this.delta = 0;
				this.GoToNextPosition();
			}
		} else if (this.state == "found") {
			this.agent.SetDestination(target.transform.position);
		} else if (this.state == "missing") {
			this.delta += Time.deltaTime;
			if (this.delta > this.lostTimeSpan) {
				this.delta = 0;
				this.state = "lost";
				this.GoToNextPosition();
			}
		} else if (this.state == "lost") {
			this.delta += Time.deltaTime;
			if (this.delta > this.lostTimeSpan) {
				this.delta = 0;
				this.state = "patrol";
			}
		}
	}

	void GoToNextPosition() {
		this.agent.SetDestination(patrolPoint[patrolIndex]);
		this.patrolIndex = (this.patrolIndex + 1) % 4;
	}

	void OnTriggerStay(Collider col) {
		//　プレイヤーキャラクターを発見
		if(col.tag == "Player" ) {
			RaycastHit hit;
			if (Physics.Linecast(transform.position,
				col.gameObject.transform.position, out hit) && 
				hit.transform.gameObject == this.target) {
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
