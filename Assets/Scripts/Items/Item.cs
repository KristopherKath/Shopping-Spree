using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Effect Values")]
    [SerializeField] private int value = 1;
    [SerializeField] private float weight = 0.25f;
    [SerializeField] [Range(-5f, 0)] private float rotationalEffect = -0.2f;

    [Header("After Item Falls Variables")]
    [SerializeField] private float timeToWaitTillDeath = 0.5f;


    public float GetWeight() => weight;
    public int GetValue() => value;

    public float GetRotationalEffect => rotationalEffect;


    public void ItemRemoval()
    {
        StartCoroutine(ItemDeathRoutine());
    }

    private IEnumerator ItemDeathRoutine()
    {
        yield return new WaitForSeconds(timeToWaitTillDeath);
        Destroy(gameObject);
    }
}
