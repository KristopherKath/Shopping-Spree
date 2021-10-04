using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    GameObject player;
    PlayerInputActions playerInputActions;
    private Animator animator;


    [SerializeField] float speedBoostTimer = 5f;
    [Tooltip("Time for input disabled when hit")] [SerializeField] float timeForInputDisableOnHit = 1f;

    [Header("Player Movement")]
    [SerializeField] float speed = 25f;
    [SerializeField] float rotateSpeed = 30f;
    [SerializeField] float maxAcceleration = 5f;
    [SerializeField] float maxVelocityMag = 100f;
    [SerializeField] float maxAccelBoost = 4;
    [SerializeField] float maxVelocityBoost = 4;


    [Header("Player Sprite Stuff")]
    [SerializeField] GameObject playerSprite;

    Rigidbody2D rb;
    ItemStack itemStack;
    public bool isStunned = false;
    
    //Inputs
    Vector2 movement;
    float pauseButton;
    float rotationInput;
    Vector2 prevPos;

    //Used for pausing game
    public PauseMenu pauseMenu;
    public GameObject pauseFirstButton;

    public bool GetIsStunned() => isStunned;

    #region Debugging

    void MovementDebug()
    {
        Debug.Log("Speed: " + speed);
        Debug.Log("Max Acceleration: " + maxAcceleration);
        Debug.Log("maxVelocityMag: " + maxVelocityMag);
        Debug.Log("Velocity: " + rb.velocity);
    }

    private void OnDrawGizmos()
    {
        /*
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + playerSprite.transform.up);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)movement);
        */
    }

    #endregion


    private void Awake()
    {
        prevPos = new Vector2(transform.position.x, transform.position.y);

        itemStack = GetComponent<ItemStack>();

        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        player = gameObject;

        playerInputActions = new PlayerInputActions(); //create input actions for referencing
        playerInputActions.Disabled.Enable();

        animator = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        GatherInput();
        Animate();
        RotateSprites();
    }

    void FixedUpdate()
    {
        ProcessInput();
    }

    //changes player animation states
    private void Animate()
    {
        if (movement.magnitude > 0)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);
    }


    void RotateSprites()
    {
        float angle = Vector2.SignedAngle(playerSprite.transform.up, movement);

        playerSprite.transform.RotateAround(playerSprite.transform.position, Vector3.forward, angle * Time.deltaTime * rotateSpeed);
    }


    //Gets values from player controller
    void GatherInput()
    {
        movement = playerInputActions.Gameplay.Movement.ReadValue<Vector2>(); //Get input vector 
        pauseButton = playerInputActions.Gameplay.Pause.ReadValue<float>(); //get pause value
    }

    //Processes player inputs
    void ProcessInput()
    {
        Movement(); //process movement
        PauseGame(); //process pause
    }

    

    private void PauseGame()
    {
        if (pauseButton > 0 && !GameManager.Instance.gameOver)
        {
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.Pause();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
    }

    //Add item to item stack
    void GrabItem(Item i)
    {
        itemStack.AddItem(i);
        i.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    //Handles player movement
    void Movement()
    {
        //Get force vector
        Vector2 force = movement * speed;
        
        if (force == Vector2.zero)
            rb.AddRelativeForce(Vector2.zero, ForceMode2D.Force);
        else
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
            GrabItem(c.gameObject.GetComponent<Item>());
        }
        if (c.gameObject.tag == "SpeedBoost")
        {
            StartCoroutine(ApplySpeedBoost(c.gameObject.GetComponent<BoostItem>().GetSpeedBoost()));
            Destroy(c.gameObject);
        }
    }


    #region Modifiers

    IEnumerator inputDisable;

    //Enables the stop input coroutine
    public void StopMovement()
    {
        //if routine already called then stop it
        if (inputDisable != null)
            StopCoroutine(inputDisable);

        inputDisable = DisableInputRoutine();
        StartCoroutine(inputDisable);

        StartCoroutine(InfinityFramesRoutine());
    }

    //disables input for set an amount of time
    IEnumerator DisableInputRoutine()
    {
        EnableDisableInputMap();
        yield return new WaitForSeconds(timeForInputDisableOnHit);
        EnablePlayerGameplayInputMap();
    }

    //Gives player infinity frames after being hit
    IEnumerator InfinityFramesRoutine()
    {
        isStunned = true;

        yield return new WaitForSeconds(timeForInputDisableOnHit + 1f);

        isStunned = false;
    }

    IEnumerator ApplySpeedBoost(float boostAmount)
    {
        speed += boostAmount;
        maxAcceleration += maxAccelBoost;
        maxVelocityMag += maxVelocityBoost;

        yield return new WaitForSeconds(speedBoostTimer);
        maxVelocityMag -= maxVelocityBoost;
        maxAcceleration -= maxAccelBoost;
        speed -= boostAmount;
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
        //rotateSpeed += rd;
    }

    public void RemoveRotationalEffect(float rd)
    {
        //rotateSpeed -= rd;
    }
    #endregion


    #region Input Enable/Disable
    //gameplay input control
    public void EnablePlayerGameplayInputMap()
    {
        DisableDisableInputMap();
        DisableMenuInputMap();

        playerInputActions.Gameplay.Enable(); //Enable the gameplay action map
    }
    public void DisablePlayerGameplayInputMap()
    {
        playerInputActions.Gameplay.Disable(); //Disable the gameplay action map
    }

    //menu input control
    public void EnableMenuInputMap()
    {
        DisableDisableInputMap();
        DisablePlayerGameplayInputMap();

        playerInputActions.Menu.Enable();
    }
    public void DisableMenuInputMap()
    {
        playerInputActions.Menu.Disable();
    }

    //disable input control
    public void EnableDisableInputMap()
    {
        DisablePlayerGameplayInputMap();
        DisableMenuInputMap();

        playerInputActions.Disabled.Enable();
    }
    public void DisableDisableInputMap()
    {
        playerInputActions.Disabled.Disable();
    }
    #endregion
}
