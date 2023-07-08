using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCollision : MonoBehaviour
{
    [SerializeField] LayerMask inViewColliderLayer;
    [SerializeField] LayerMask toLayer;
    int initalLayer;

    void Start()
    {
        initalLayer = gameObject.layer;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == (int)Mathf.Log(inViewColliderLayer, 2)){
            gameObject.layer = (int)Mathf.Log(toLayer, 2);
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == (int)Mathf.Log(inViewColliderLayer, 2)){
            gameObject.layer = initalLayer;
        }
    }
}
