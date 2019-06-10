using UnityEngine;
using System.Collections;

public class UnityChanController : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidbody;

    // Update is called once per frame
    void Update () {
        if (InputUp()) {
            animator.SetBool("isRunning", true);
            transform.position += transform.forward * 0.1f;
        } else {
            animator.SetBool("isRunning", false);
        }
        if (InputRight()) {
            transform.Rotate(0, 5, 0);
        }
        if (InputLeft()) {
            transform.Rotate(0, -5, 0);
        }
    }

    private bool InputUp() {
        return (Input.GetKey("up") || Input.GetKey("w"));
    }

    private bool InputRight() {
        return (Input.GetKey("right") || Input.GetKey("d"));
    }

    private bool InputLeft() {
        return (Input.GetKey("left") || Input.GetKey("a"));
    }

}
