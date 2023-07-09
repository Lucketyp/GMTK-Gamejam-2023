using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public LayerMask layerMask;

    void Start()
    {

    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            other.GetComponentInParent<Player>().TakeDamage(1);
        }
        Destroy(gameObject);
    }
}
