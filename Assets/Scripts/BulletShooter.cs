using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float shootTime = 1f;
    [SerializeField] Transform target;
    float timer = 0f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, target.transform.position - transform.position);

        timer += Time.deltaTime;
        if(timer >= shootTime){
            timer = 0f;

            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit);
            
            Shoot();
        }
    }

    void Shoot() {
        Instantiate(bullet, transform.position + transform.forward * 1, transform.rotation);
    }
}
