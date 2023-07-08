using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform target;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] Vector3 spawnOffset;
    float timer = 0f;
    float shootTime = 1f;
    int bulletsToFire = 1;

    void Start()
    {

    }

    void FixedUpdate()
    {
        RotateToTarget();
        timer += Time.deltaTime;
        if (timer >= shootTime && CanSeeTarget())
        {
            timer = 0f;
            Shoot();
            shootTime = 0.5f;
            bulletsToFire--;

            if(bulletsToFire == 0){
                bulletsToFire = Random.Range(1, 4);
                shootTime = Random.Range(1, 6);
            }
        }
    }

    bool CanSeeTarget() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + transform.rotation * spawnOffset, transform.forward);
        Physics.Raycast(ray, out hit);
        int layer = hit.collider.gameObject.layer;

        return targetLayers == (targetLayers | (1 << layer));
    }

    void RotateToTarget(){
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, target.transform.position - transform.position);
    }

    void Shoot()
    {
        Instantiate(bullet, transform.position + transform.rotation * spawnOffset, transform.rotation);
    }
}
