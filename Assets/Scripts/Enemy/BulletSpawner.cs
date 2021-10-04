using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] float timeTillSpawn = 5f;
    [SerializeField] float spawnOffset = 5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10f;

    bool enableTimer = false;
    Vector3 spawnPos;
    float timeLeft;

    private void Awake()
    {
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
                //Debug.Log("SPAWN BULLET");
                timeLeft = timeTillSpawn;
                GameObject bulletInstance = Instantiate(bulletPrefab, spawnPos, transform.rotation);
                bulletInstance.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);

            }
            timeLeft -= Time.deltaTime;
        }
    }


    private void GameManagerOnOnStateChanged(GameState newState)
    {
        if (newState == GameState.GameStart)
            enableTimer = true;
        else
            enableTimer = false;
    }
}
