using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject target;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform spawnOffset;
    [SerializeField] int maxFiredInRound;
    [SerializeField] float maxTimeBetweenRounds;
    [SerializeField] float minTimeBetweenRounds;
    [SerializeField] float shootTimeInRound = 0.5f;
    [SerializeField] float minDistanceForOffset = Mathf.Infinity;
    [SerializeField] float bulletSpeed;
    float shootTime = 1f;
    float timer = 0f;
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
            shootTime = shootTimeInRound;
            bulletsToFire--;

            if(bulletsToFire == 0){
                bulletsToFire = Random.Range(1, maxFiredInRound + 1);
                shootTime = Random.Range(0, 1000) / 1000 * (maxTimeBetweenRounds - minTimeBetweenRounds) + minTimeBetweenRounds;
            }
        }
    }

    bool CanSeeTarget() {
        RaycastHit hit;
        Ray ray = new Ray(spawnOffset.position, transform.forward);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            int layer = hit.collider.gameObject.layer;
            return target.layer == layer;
        } else {
            return false;
        }
   
    }

    void RotateToTarget(){
        Vector3 difference = target.transform.position - spawnOffset.position;
        difference.y = 0;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, difference);
    }

    void Shoot()
    {
        Vector3 spawnAt = spawnOffset.position;
        Vector3 diff = target.transform.position - spawnAt;
        float distance = diff.magnitude;
        if(distance > minDistanceForOffset){
            spawnAt += diff * (distance - minDistanceForOffset) / distance;
        }

        GameObject obj = Instantiate(bullet, spawnAt, transform.rotation);
        obj.GetComponent<Bullet>().layerMask = layerMask;
        obj.GetComponent<Bullet>().speed = bulletSpeed;
        Destroy(obj, 15f);
    }
}
