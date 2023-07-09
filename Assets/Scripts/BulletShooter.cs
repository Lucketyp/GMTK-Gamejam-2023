using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletShooter : MonoBehaviour
{
    public GameObject target;
    [SerializeField] GameObject bullet;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform spawnOffset;
    [SerializeField] int maxFiredInRound;
    [SerializeField] float maxTimeBetweenRounds;
    [SerializeField] float minTimeBetweenRounds;
    [SerializeField] float shootTimeInRound = 0.5f;
    [SerializeField] float minDistanceForOffset = Mathf.Infinity;
    [SerializeField] float bulletSpeed;
    [SerializeField] float moveSpeed = 4;
    [SerializeField] float fleeRange = 15; 
    [SerializeField] float shootAgainRange = 20; 
    [SerializeField] float deathRange = 1f;
    [SerializeField] AudioSource gunShot;

    float shootTime = 1f;
    float timer = 0f;
    int bulletsToFire = 1;
    [SerializeField] int behaviour;
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Debug.Log(animator);
    }

    void Update()
    {          
        if(behaviour != 2) {
            behaviour = CheckDistance();
            Behaviour(behaviour);
        }
    }

    int CheckDistance() {
        Vector3 difference = target.transform.position - spawnOffset.position;
        difference.y = 0;
        float distance = difference.magnitude;
        if(distance < deathRange){
            return 2;
        } else if(distance < fleeRange){
            return 1;
        } else if(distance > shootAgainRange){
            return 0;
        }
        return behaviour;
    }

    void Behaviour(int behaviour) {
        switch(behaviour){
            case 0:
                ShootBehaviour();
                break;
            case 1:
                RunBehaviour();
                break;
            case 2:
                DeathBehaviour();
                break;
            default:
                break;
        }
    }

    void RunBehaviour(){
        RotateToTarget(true);
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }

    void ShootBehaviour(){

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

    void DeathBehaviour(){
        Debug.Log("Hunter Death");
        StartCoroutine(HunterDeath());
    }

    IEnumerator HunterDeath(){
        GetComponentInChildren<Renderer>().material.color = Color.red;
        for(int i = 0; i < 90; i++){
            transform.Rotate(0, 0, 1);
            transform.position += new Vector3(0, 0.01f, 0);
            yield return new WaitForSeconds(0.005f);
        }
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("Win Screen");
    }

    bool CanSeeTarget() {
        RaycastHit hit;
        Ray ray = new Ray(spawnOffset.position, transform.forward);
        Debug.DrawRay(spawnOffset.position, transform.forward);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            int layer = hit.collider.gameObject.layer;
            return target.layer == layer;
        } else {
            return false;
        }
   
    }

    void RotateToTarget(bool flipped = false){
        Vector3 difference = target.transform.position - spawnOffset.position;
        if(flipped){
            difference *= -1;
        }
        difference.y = 0;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, difference);
    }

    void Shoot()
    {
        gunShot.Play();
        animator.SetTrigger("shoot");
        Vector3 spawnAt = spawnOffset.position;
        Vector3 diff = target.transform.position - spawnAt;
        float distance = diff.magnitude;

        if(distance > minDistanceForOffset){
            spawnAt += diff * (distance - minDistanceForOffset) / distance;
        }

        GameObject obj = Instantiate(bullet, spawnAt, transform.rotation);
        Debug.Log(spawnAt);
        obj.GetComponent<Bullet>().layerMask = layerMask;
        obj.GetComponent<Bullet>().speed = bulletSpeed;
        Destroy(obj, 15f);
    }
}
