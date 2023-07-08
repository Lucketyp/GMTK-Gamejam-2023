using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject target;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Vector3 spawnOffset;
    [SerializeField] int maxFiredInRound;
    [SerializeField] int maxSecondsBetweenRounds;
    float timer = 0f;
    float shootTime = 1f;
    int bulletsToFire = 1;

    void Start()
    {

    }

    void Update()
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
                bulletsToFire = Random.Range(1, maxFiredInRound + 1);
                shootTime = Random.Range(1, maxSecondsBetweenRounds + 1);
            }
        }
    }

    bool CanSeeTarget() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + transform.rotation * spawnOffset, transform.forward);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            int layer = hit.collider.gameObject.layer;
            return target.layer == layer;
        } else {
            return false;
        }
   
    }

    void RotateToTarget(){
        Vector3 difference = target.transform.position - transform.position;
        difference.y = 0;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, difference);
    }

    void Shoot()
    {
        GameObject obj = Instantiate(bullet, transform.position + transform.rotation * spawnOffset, transform.rotation);
        obj.GetComponent<Bullet>().layerMask = layerMask;
    }
}
