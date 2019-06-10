using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PatrolData {
    [SerializeField] private List<Vector3> patrolPoint = new List<Vector3>();
    [SerializeField] private float span = 3.0f;
    [SerializeField] private float lostTimeSpan = 6.0f;

    public List<Vector3> PatrolPoint {
        get { return patrolPoint; }
    }

    public float Span {
        get { return span; }
    }

    public float LostTimeSpan {
        get { return lostTimeSpan; }
    }

    void OnValidate() {
        span = Mathf.Max(span, 0);
        lostTimeSpan = Mathf.Max(lostTimeSpan, 0);
    }


}

public class EnemyController : MonoBehaviour {


    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject target;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private PatrolData patrolData;

    private string state = "patrol";
    private bool seekRight = false;
    private bool seekLeft = false;

    private int patrolIndex = 0;
    private float delta = 0;

    void Update () {
        if (state == "patrol") {
            UpdatePatrol();
        } else if (state == "returning") {
            UpdateReturningToPatrolPoint();
        } else if (state == "chase") {
            UpdateChase();
        } else if (state == "lost") {
            UpdateLost();
        }
    }

    void UpdatePatrol() {
        delta += Time.deltaTime;
        if (delta > patrolData.Span) {
            delta = 0;
            GoToNextPosition();
        }
    }

    void UpdateReturningToPatrolPoint() {
        // Note:ゲームに合わせて設定を変更すべし
        Vector3 returnPoint =  patrolData.PatrolPoint[patrolIndex] + new Vector3(0, 0, 5);
        float dist = Vector3.Distance(transform.position, returnPoint);
        if (dist < 1.0f) {
            ChangeState("patrol");
            SetAgentDestination(patrolData.PatrolPoint[patrolIndex]);
        } else {
            SetAgentDestination(returnPoint);
        }
    }

    void UpdateChase() {
        SetAgentDestination(target.transform.position);
    }

    void UpdateLost() {
        delta += Time.deltaTime;
        if (delta > patrolData.LostTimeSpan) {
            ChangeState("returning");
            GoToNextPosition();
        } else if (delta > patrolData.LostTimeSpan / 3 * 2) {
            SeekLeft();
        } else if (delta > patrolData.LostTimeSpan / 3 * 1) {
            SeekRight();
        }
    }

    void SetAgentDestination(Vector3 destination) {
        agent.SetDestination(destination);
    }

    void SeekLeft() {
        if (!seekLeft) {
            seekLeft = true;
            ChangeAngularVelocity(Vector3.up * -1);
        }
    }

    void SeekRight() {
        if (!seekRight) {
            seekRight = true;
            ChangeAngularVelocity(Vector3.up);
        }
    }

    void ChangeAngularVelocity(Vector3 newVector) {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = newVector;
    }
    void ChangeState(string changeTo) {
        if (changeTo == "lost") {
            delta = 0;
            seekLeft = false;
            seekRight = false;
            state = "lost";
        } else {
            delta = 0;
            state = changeTo;
        }
    }
    void GoToNextPosition() {
        ChangeAngularVelocity(Vector3.zero);
        SetAgentDestination(GetNextPatrolPoint());
    }

    Vector3 GetNextPatrolPoint() {
        patrolIndex = (patrolIndex + 1) % patrolData.PatrolPoint.Count;
        return patrolData.PatrolPoint[patrolIndex];
    }
    void OnTriggerStay(Collider col) {
        //　プレイヤーキャラクターを発見
        if(col.tag == target.tag ) {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, 
            target.transform.position, out hit) && 
            hit.transform.gameObject == target) {
                ChangeState("chase");
            } else if (state == "chase") {
                delta += Time.deltaTime;
                if (delta > patrolData.LostTimeSpan) {
                    ChangeState("lost");
                }
            }
        }
    }
    
    void OnTriggerExit(Collider col) {
        if(col.tag == target.tag && state == "chase") {
            ChangeState("lost");
        }
    }
}
