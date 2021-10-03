using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private bool canDie = false;
    [SerializeField] private float timeToWaitToDie = 0.5f;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            c.gameObject.GetComponent<ItemStack>().RemoveItems();

            if (canDie)
                StartCoroutine(HazardEndRoutine());
        }
    }

    private IEnumerator HazardEndRoutine()
    {
        yield return new WaitForSeconds(timeToWaitToDie);
        Destroy(gameObject);
    }
}
