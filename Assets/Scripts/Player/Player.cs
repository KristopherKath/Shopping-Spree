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
        Vector2 force = movement * speed;
        AccelerateTo(force, maxAcceleration);
    }

    //Helper function to accelerate up to a max acceleration
    void AccelerateTo(Vector2 targetVelocity, float maxAccel)
    {
        Vector2 deltaV = targetVelocity - rb.velocity;

        Vector2 accel = deltaV / Time.deltaTime;

        if (accel.sqrMagnitude > maxAccel * maxAccel)
            accel = accel.normalized * maxAccel;

        rb.AddForce(accel, ForceMode2D.Force);
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
