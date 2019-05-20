using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {


	public NavMeshAgent agent;
	float span = 5.0f;
	float delta = 0;
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
		this.delta += Time.deltaTime;
		if (this.delta > this.span) {
			this.delta = 0;
			agent.SetDestination(patrolPoint[patrolIndex]);
			patrolIndex = (patrolIndex + 1) % 4;
		}
	}
}
