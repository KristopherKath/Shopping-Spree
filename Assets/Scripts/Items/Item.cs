using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private float weight = 0.25f;

    public float GetWeight() => weight;
    public int GetValue() => value;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<ItemStack>().AddItem(this);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
