using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAnimation : MonoBehaviour
{
    Vector3 oldPosition;
    Vector3 position;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        position = transform.position;
        if(position != oldPosition){
            animator.SetBool("Moving", true);
        } else {
            animator.SetBool("Moving", false);
        }
        oldPosition = position;
    }
}
