using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
 
    Rigidbody rb;
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float sprintSpeed = 20f;
    [SerializeField] float accelerationMultiplier = 1f;
    [SerializeField] float deccelerationMultiplier = 1f;
    [SerializeField] float turnAccelerationMultiplier = 1f;
    [SerializeField] float speed;
    [SerializeField] Transform relativeTo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float maxSpeed = walkSpeed;
        if(Input.GetKey(KeyCode.LeftShift)){
            maxSpeed = sprintSpeed;
        }

        Vector3 inputDirection = Vector3.zero;
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.z = Input.GetAxis("Vertical");

        Vector3 relativeDiff = transform.position - relativeTo.position;
        relativeDiff.y = 0;
        inputDirection = 
            Quaternion.FromToRotation(Vector3.forward, relativeDiff) 
            * inputDirection.normalized;


        Vector3 currentVelocity = rb.velocity;
        currentVelocity.y = 0;
        float currentSpeed = currentVelocity.magnitude;
        Vector3 currentDirection = Vector3.zero;
        if(currentSpeed > 0){
            currentDirection = currentVelocity / currentSpeed;
        } else {
            rb.velocity = Vector3.zero;
            currentDirection = inputDirection;
        }

        float moveInDirection = Vector3.Dot(currentDirection, inputDirection);

        Vector3 accelerate = currentDirection * Mathf.Max(moveInDirection, 0) * accelerationMultiplier * (1 - currentSpeed / maxSpeed);
        Vector3 deccelerate = -currentDirection * Mathf.Min(Mathf.Max(1-moveInDirection, 0), 1) * deccelerationMultiplier;
        Vector3 turn = Vector3.Cross(Vector3.Cross(currentDirection, inputDirection), currentDirection) * turnAccelerationMultiplier;

        Vector3 finalAcceleration = accelerate + deccelerate + turn;
        
        if(Vector3.Dot(currentVelocity, currentVelocity + finalAcceleration * Time.fixedDeltaTime) >= 0){
            rb.AddForce(finalAcceleration, ForceMode.Force);
        } else {
            rb.velocity = Vector3.zero;
        }

        speed = rb.velocity.magnitude;
    }
}
