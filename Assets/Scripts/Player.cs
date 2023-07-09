using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamgeable
{
    [SerializeField] GameObject RestartScreen;
    [SerializeField] GameObject BearModel;
    [SerializeField] LayerMask deadlyLayers;
    [SerializeField] int maxHealth = 3;
    Vector3 initalPosition;
    AudioSource audioSource;
    ParticleSystem blood;

    void Start()
    {
        blood = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        initalPosition = transform.position;
    }

    public void TakeDamage(int damage) {
        audioSource.Play();
        blood.Play();
        maxHealth -= damage;
        if(maxHealth <= 0){
            StartCoroutine(PlayerDeath());
        }
    }

    IEnumerator PlayerDeath() {
        GetComponentInChildren<Renderer>().material.color = Color.red;
        // stop the player from moving
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        // rotate the player 90 degrees slowly
        for(int i = 0; i < 90; i++){
            BearModel.transform.Rotate(0, 0, 1);
            BearModel.transform.position += new Vector3(0, 0.005f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        RestartScreen.SetActive(true);
    }
}

interface IDamgeable {
    void TakeDamage(int damage);
}