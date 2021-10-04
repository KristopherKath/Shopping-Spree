using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    //When player hit
    [SerializeField] private bool canDie = false;
    [SerializeField] private bool isProjectile = false;
    [SerializeField] private float timeToWaitToDie = 0.5f;
    [SerializeField] private float timeToWaitToActivate = 3f;

    //used for all hazards
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            //if not projectile disable collider to not remove more items
            if (!isProjectile)
                GetComponent<BoxCollider2D>().enabled = true;

            //Only take items and stop movement if they arent hit already
            if (!c.gameObject.GetComponent<Player>().GetIsStunned())
            {
                c.gameObject.GetComponent<ItemStack>().RemoveItems(); //remove random amount of items
                c.gameObject.GetComponent<Player>().StopMovement(); //stops player movement for a little
            }

            //if it dies then kill it
            if (canDie)
                StartCoroutine(HazardEndRoutine());
            //Otherwise reactivate the collider
            else
                StartCoroutine(ActivateColliderRoutine());
        }

    }

    //used for bullets
    private void OnCollisionEnter2D(Collision2D c)
    {
        //destroys projectile
        if (isProjectile)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ActivateColliderRoutine()
    {
        yield return new WaitForSeconds(timeToWaitToActivate);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private IEnumerator HazardEndRoutine()
    {
        yield return new WaitForSeconds(timeToWaitToDie);
        Destroy(gameObject);
    }
}
