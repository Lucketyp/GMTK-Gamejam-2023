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
    [SerializeField] float moveAroundMultiplier;
    [SerializeField] float moveFromMultiplier;
    [SerializeField] float turnSpeed;

    float shootTime = 1f;
    float timer = 0f;
    int bulletsToFire = 1;
    [SerializeField] int behaviour;
    Animator animator;
    Vector3 velocity;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (behaviour != 2)
        {
            behaviour = CheckDistance();
            Behaviour(behaviour);
        }
    }

    int CheckDistance()
    {
        Vector3 difference = target.transform.position - spawnOffset.position;
        difference.y = 0;
        float distance = difference.magnitude;
        if (distance < deathRange)
        {
            return 2;
        }
        else if (distance < fleeRange)
        {
            return 1;
        }
        else if (distance > shootAgainRange)
        {
            return 0;
        }

        return behaviour;
    }

    void Behaviour(int behaviour)
    {
        switch (behaviour)
        {
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

    void RunBehaviour()
    {
        float speed = velocity.magnitude;
        Vector3 currentMoveDirection = velocity / speed;

        Vector3 toPlayer = target.transform.position - transform.position;
        toPlayer.y = 0;
        Vector3 playerDirection = toPlayer / toPlayer.magnitude;

        Vector3 boxSize = new Vector3(100f, 100f, 100f);
        Vector3 boxCenter = transform.position;
        Collider[] colliders = Physics.OverlapBox(boxCenter, boxSize, transform.rotation);

        Vector3 moveAround = Vector3.zero;
        Vector3 closestDifference = Vector3.zero;
        float closestDistance = -1;
        float proximity = 0;

        foreach (Collider c in colliders)
        {
            if (layerMask == (layerMask | (1 << c.gameObject.layer)))
            {
                Vector3 toObstacle = c.transform.position - transform.position;
                toObstacle.y = 0;
                float distanceToObstacle = toObstacle.magnitude;

                moveAround += Vector3.Cross(toObstacle, Vector3.Cross(currentMoveDirection, toObstacle)) / Mathf.Pow(distanceToObstacle, 3);
                proximity += 1 / Mathf.Pow(distanceToObstacle, 2);

                if(closestDistance < 0 || closestDistance > distanceToObstacle){
                    closestDifference = toObstacle;
                    closestDistance = distanceToObstacle;
                }
            }
        }

        if(closestDistance > 0){
            RaycastHit hit;
            if(Physics.Raycast(new Ray(transform.position, closestDifference), out hit, Mathf.Infinity, layerMask)){
                closestDifference = hit.point - transform.position;
                closestDifference.y = 0;
            }
        }

        velocity = -playerDirection
            + moveAroundMultiplier * moveAround.normalized * proximity 
            - moveFromMultiplier * closestDifference / Mathf.Pow(closestDifference.magnitude, 3);

        velocity = velocity.normalized;

        transform.position += velocity * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(
            transform.rotation, 
            Quaternion.FromToRotation(Vector3.forward, velocity), 
            Time.deltaTime * turnSpeed
        );
    }

    void ShootBehaviour()
    {

        RotateToTarget();
        timer += Time.deltaTime;
        if (timer >= shootTime && CanSeeTarget())
        {

            timer = 0f;
            Shoot();
            shootTime = shootTimeInRound;
            bulletsToFire--;

            if (bulletsToFire == 0)
            {
                bulletsToFire = Random.Range(1, maxFiredInRound + 1);
                shootTime = Random.Range(0, 1000) / 1000 * (maxTimeBetweenRounds - minTimeBetweenRounds) + minTimeBetweenRounds;
            }
        }
    }

    void DeathBehaviour()
    {
        Debug.Log("Hunter Death");
        StartCoroutine(HunterDeath());
    }

    IEnumerator HunterDeath()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
        for (int i = 0; i < 90; i++)
        {
            transform.Rotate(0, 0, 1);
            transform.position += new Vector3(0, 0.01f, 0);
            yield return new WaitForSeconds(0.005f);
        }
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("Win Screen");
    }

    bool CanSeeTarget()
    {
        RaycastHit hit;
        Ray ray = new Ray(spawnOffset.position, transform.forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            int layer = hit.collider.gameObject.layer;
            return target.layer == layer;
        }
        else
        {
            return false;
        }

    }

    void RotateToTarget(bool flipped = false)
    {
        Vector3 difference = target.transform.position - spawnOffset.position;
        if (flipped)
        {
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

        if (distance > minDistanceForOffset)
        {
            spawnAt += diff * (distance - minDistanceForOffset) / distance;
        }

        GameObject obj = Instantiate(bullet, spawnAt, transform.rotation);
        obj.GetComponent<Bullet>().layerMask = layerMask;
        obj.GetComponent<Bullet>().speed = bulletSpeed;
        Destroy(obj, 15f);
    }
}
