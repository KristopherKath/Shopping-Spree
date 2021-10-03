using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    GameObject player;
    PlayerInputActions playerInputActions;

    //Player Control variables
    [SerializeField] float speed = 25f;
    [SerializeField] float rotateSpeed = 30f;
    [SerializeField] float maxAcceleration = 5f;
    [SerializeField] float maxVelocityMag = 100f;

    Rigidbody2D rb;
    bool grabbable = false;
    Item grabbableItem;
    ItemStack itemStack;
    
    //Inputs
    Vector2 movement;
    float button;
    float rotationInput;

    Vector2 prevPos;

    private void Awake()
    {
        prevPos = new Vector2(transform.position.x, transform.position.y);
        itemStack = GetComponent<ItemStack>();
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
        ProcessInput();
    }

    //Gets values from player controller
    void GatherInput()
    {
        movement = playerInputActions.Gameplay.Movement.ReadValue<Vector2>(); //Get input vector 
        button = playerInputActions.Gameplay.Pickup.ReadValue<float>();
        
        //rotationInput = playerInputActions.Gameplay.Rotation.ReadValue<float>();
    }

    //Processes player inputs
    void ProcessInput()
    {
        Movement(); //process movement
        GrabItem(); //process grabbing item
        
        //Rotation(); //prcess rotation
    }

    //Add item to item stack
    void GrabItem()
    {
        if (grabbable && button > 0)
        {
            grabbable = false;
            itemStack.AddItem(grabbableItem);
            grabbableItem.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    //Calculates rotation of player
    void Rotation()
    {
        /*
        Vector2 myVector = transform.position - (Vector3)prevPos;
        float angle = Vector2.Angle(Vector2.up, myVector);
        rb.rotation = angle;

        prevPos = new Vector2(transform.position.x, transform.position.y);

        
        Debug.Log(rotationInput);

        if (rotationInput > 0)
            transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
        else if (rotationInput < 0)
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        */
    }

    //Handles player movement
    void Movement()
    {
        //Get force vector
        Vector2 force = movement * speed;
        
        //Apply acceleration to position
        AccelerateTo(force, maxAcceleration);

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

        //if the acceleration is greater than the max acceleration then clamp it
        if (accel.sqrMagnitude > maxAccel * maxAccel)
            accel = accel.normalized * maxAccel;

        //apply acceleraition
        rb.AddRelativeForce(accel, ForceMode2D.Force);
    }


    //Handle grabbing items
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Item")
        {
            grabbable = true;
            grabbableItem = c.gameObject.GetComponent<Item>();
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Item")
        {
            grabbable = false;
            grabbableItem = null;
        }
    }

    //Add mass to rigidbody
    public void AddMass(float m)
    {
        rb.mass += m;
    }
    //Lose mass from rigidbody
    public void LoseMass(float m)
    {
        rb.mass -= m;
    }

    public void AddRotationalEffect(float rd)
    {
        rotateSpeed += rd;
    }

    public void RemoveRotationalEffect(float rd)
    {
        rotateSpeed -= rd;
    }



}
