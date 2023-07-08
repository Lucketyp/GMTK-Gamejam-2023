using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] LayerMask deadlyLayers;
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
        if(deadlyLayers == (deadlyLayers | (1 << other.gameObject.layer))){
            //transform.position = initalPosition;
            Debug.Log("Dieded");
        }
    }
}
