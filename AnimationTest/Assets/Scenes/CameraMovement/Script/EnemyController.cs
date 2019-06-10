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
    Vector3 patrolPoint = new Vector3();
    Vector3 patrolRotation = new Vector3();
    Rigidbody enemyBody;
    bool seekRight;
    bool seekLeft;

    void Start () {
        // 定点パトロールは問題があるので指定した位置に移動するだけにする
        patrolPoint = new Vector3(-6f, -3.9f, 11f);
        patrolRotation = new Vector3(0, 180, 0);
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
        } else if (this.state == "returning") {
            this.UpdateReturningToPatrolPoint();
        } else if (this.state == "chase") {
            this.agent.SetDestination(target.transform.position);
        } else if (this.state == "lost") {
            this.delta += Time.deltaTime;
            if (this.delta > this.lostTimeSpan) {
                this.ChangeState("returning");
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
    void ChangeState(string changeTo) {
        if (changeTo == "lost") {
            this.delta = 0;
            this.seekLeft = false;
            this.seekRight = false;
            this.state = "lost";
        } else {
            this.delta = 0;
            this.state = changeTo;
        }
    }
    void GoToNextPosition() {
        this.enemyBody.velocity = Vector3.zero;
        this.enemyBody.angularVelocity = Vector3.zero;
        this.agent.SetDestination(patrolPoint);
    }

    void UpdateReturningToPatrolPoint() {
        Vector3 returnPoint =  patrolPoint + new Vector3(0, 0, 5);
        float dist = Vector3.Distance(transform.position, returnPoint);
        if (dist < 1.0f) {
            ChangeState("patrol");
            this.agent.SetDestination(patrolPoint);
        } else {
            this.agent.SetDestination(returnPoint);
        }
    }

    void OnTriggerStay(Collider col) {
        //　プレイヤーキャラクターを発見
        if(col.tag == target.tag ) {
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
