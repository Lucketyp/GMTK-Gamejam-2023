using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;

    void Start()
    {

    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
