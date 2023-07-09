using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            animator.SetTrigger("Activate");
            audioSource.Play();
            other.GetComponentInParent<Player>().TakeDamage(2);
        }
    }
}
