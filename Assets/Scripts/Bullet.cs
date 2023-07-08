using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void FixedUpdate() {
        if(Physics.CheckBox(boxCollider.center + transform.position, boxCollider.size)) {
            Destroy(gameObject);
        }
    }

}
