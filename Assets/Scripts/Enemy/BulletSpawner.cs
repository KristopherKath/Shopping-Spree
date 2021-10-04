using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Throwing Variables")]
    [SerializeField] float timeTillSpawn = 5f;
    [SerializeField] float spawnOffset = 5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10f;

    [Header("Animation Variables")]
    [SerializeField] float throwAnimationTime = 0.5f;


    bool enableTimer = false;
    Vector3 spawnPos;
    float timeLeft;
    private Animator animator;
    bool throwAnimTrue;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        GameManager.OnGameStateChanged += GameManagerOnOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnStateChanged;
    }

    private void Start()
    {

        spawnPos = transform.position;
        spawnPos.y += spawnOffset;

        timeLeft = timeTillSpawn;
    }

    private void FixedUpdate()
    {
        if (enableTimer)
        {
            if (timeLeft <= 0)
            {
                timeLeft = timeTillSpawn;
                GameObject bulletInstance = Instantiate(bulletPrefab, spawnPos, transform.rotation);
                bulletInstance.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
                StartCoroutine(AnimationRoutine());
            }
            timeLeft -= Time.deltaTime;
            Animate();
        }
    }


    private void GameManagerOnOnStateChanged(GameState newState)
    {
        if (newState == GameState.GameStart)
            enableTimer = true;
        else
            enableTimer = false;
    }

    IEnumerator AnimationRoutine()
    {
        throwAnimTrue = true;
        yield return new WaitForSeconds(throwAnimationTime);
        throwAnimTrue = false;
    }

    private void Animate()
    {
        if (throwAnimTrue)
            animator.SetBool("isThrowing", true);
        else
            animator.SetBool("isThrowing", false);
    }
}
