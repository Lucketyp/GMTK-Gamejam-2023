using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    float speed;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        speed = rb.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        //Face the direction of movement
        Vector3 direction = -rb.velocity;
        direction.y = 0;
        if(direction.magnitude > 0.1){
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
