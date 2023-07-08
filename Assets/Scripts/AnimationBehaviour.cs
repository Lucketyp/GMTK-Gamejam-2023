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
    }
}
