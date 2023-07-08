using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float shootTime = 1f;
    float timer = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= shootTime){
            timer = 0f;
            Shoot();
        }
    }

    void Shoot() {
        Instantiate(bullet, transform.position + transform.forward * 1, transform.rotation);
    }
}
