using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            other.GetComponentInParent<Player>().TakeDamage(2);
        }
    }
}
