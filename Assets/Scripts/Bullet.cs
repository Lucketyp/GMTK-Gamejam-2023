using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public LayerMask layerMask;
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
        if(Physics.CheckBox(boxCollider.center + transform.position, boxCollider.size, Quaternion.identity, layerMask)) {
            Destroy(gameObject);
        }
    }

}
