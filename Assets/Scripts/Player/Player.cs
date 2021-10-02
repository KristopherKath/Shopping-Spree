using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Input variables
    GameObject player;
    PlayerInputActions playerInputActions;

    //Player Control variables
    [SerializeField] float speed = 25f;
    [SerializeField] float maxAcceleration = 5f;
    [SerializeField] float maxVelocityMag = 100f;

    Rigidbody2D rb;
    Vector2 movement;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = gameObject;
        playerInputActions = new PlayerInputActions(); //create input actions for referencing
        playerInputActions.Gameplay.Enable(); //Enable the gameplay action map
    }

    private void Update()
    {
        GatherInput();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void GatherInput()
    {
        movement = playerInputActions.Gameplay.Movement.ReadValue<Vector2>(); //Get input vector 
    }

    //Handles player movement
    void Movement()
    {
        //Get force vector
        Vector2 force = movement * speed;
        
        //if (force.magnitude > 0)
        //Apply acceleration to position
        AccelerateTo(force, maxAcceleration);

        Debug.Log(rb.velocity.magnitude);
        //if the velocity is too high clamp it to max velocity
        if (rb.velocity.sqrMagnitude > maxVelocityMag * maxVelocityMag)
            rb.velocity = rb.velocity.normalized * maxVelocityMag;

    }

    //Helper function to accelerate up to a max acceleration
    void AccelerateTo(Vector2 targetVelocity, float maxAccel)
    {
        //get the change in velocity
        Vector2 deltaV = targetVelocity - rb.velocity;

        //Get the acceleration value
        Vector2 accel = deltaV / Time.deltaTime;

        //Debug.Log("rb vel: " + rb.velocity);
        //Debug.Log("delta vel: " + deltaV);
        //Debug.Log("accel: " + accel.sqrMagnitude);

        //if the acceleration is greater than the max acceleration then clamp it
        if (accel.sqrMagnitude > maxAccel * maxAccel)
            accel = accel.normalized * maxAccel;

        //apply acceleraition
        rb.AddRelativeForce(accel, ForceMode2D.Force);
    }


    public void AddMass(float m)
    {
        rb.mass += m;
    }
    public void LoseMass(float m)
    {
        rb.mass -= m;
    }


}
