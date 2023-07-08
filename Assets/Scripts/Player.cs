using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] LayerMask deathByLayers;
    Vector3 initalPosition;

    void Start()
    {
        initalPosition = transform.position;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.layer);
        if(deathByLayers == (deathByLayers | (1 << other.gameObject.layer))){
            //transform.position = initalPosition;
            Debug.Log("Dieded");
        }
    }
}
