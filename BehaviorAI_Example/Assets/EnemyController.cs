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
	float lostTimeSpan = 6.0f;
	int patrolIndex = 0;
	Vector3[] patrolPoint = new Vector3[4];
	Rigidbody enemyBody;
	bool seekRight;
	bool seekLeft;

	void Start () {
		patrolPoint[0] = new Vector3(-2.7f, 0.5f, 0.6f);
		patrolPoint[1] = new Vector3(-2.7f, 0.5f, 7.1f);
		patrolPoint[2] = new Vector3(-10.0f, 0.5f, 7.4f);
		patrolPoint[3] = new Vector3(-10.0f, 0.5f, 0.6f);
		this.enemyBody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		if (this.state == "patrol") {
			this.delta += Time.deltaTime;
			if (this.delta > this.span) {
				this.delta = 0;
				this.GoToNextPosition();
			}
		} else if (this.state == "chase") {
			this.agent.SetDestination(target.transform.position);
		} else if (this.state == "lost") {
			this.delta += Time.deltaTime;
			if (this.delta > this.lostTimeSpan) {
				this.ChangeState("patrol");
				this.GoToNextPosition();
			} else if (this.delta > this.lostTimeSpan / 3 * 2) {
				if (!this.seekLeft) {
					this.seekLeft = true;
					this.enemyBody.velocity = Vector3.zero;
					this.enemyBody.angularVelocity = Vector3.up * -1;
				}
			} else if (this.delta > this.lostTimeSpan / 3 * 1) {
				if (!this.seekRight) {
					this.seekRight = true;
					this.enemyBody.velocity = Vector3.zero;
					this.enemyBody.angularVelocity = Vector3.up;
				}
			}
		}
	}
	void ChangeState(string changeto) {
		if (changeto == "chase") {
			this.delta = 0;
			this.state = "chase";
		} else if (changeto == "lost") {
			this.delta = 0;
			this.seekLeft = false;
			this.seekRight = false;
			this.state = "lost";
		} else if (changeto == "patrol") {
			this.delta = 0;
			this.state = "patrol";
		}
	}
	void GoToNextPosition() {
		this.enemyBody.velocity = Vector3.zero;
		this.enemyBody.angularVelocity = Vector3.zero;
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
				this.ChangeState("chase");
			} else if (this.state == "chase") {
				this.delta += Time.deltaTime;
				if (this.delta > this.lostTimeSpan) {
					this.ChangeState("lost");
				}
			}
		}
	}
	
	void OnTriggerExit(Collider col) {
		if(col.tag == "Player" && this.state == "chase") {
			this.ChangeState("lost");
		}
	}
}
