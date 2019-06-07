using UnityEngine;
using System.Collections;

public class UnityChanController : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidbody;

    // Update is called once per frame
    void Update () {
        if (Input.GetKey("up")) {
            animator.SetBool("isRunning", true);
            transform.position += transform.forward * 0.1f;
        } else {
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKey("right")) {
            transform.Rotate(0, 5, 0);
        }
        if (Input.GetKey ("left")) {
            transform.Rotate(0, -5, 0);
        }
    }
}
