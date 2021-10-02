using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private float weight = 0.25f;

    public float GetWeight() => weight;
    public int GetValue() => value;

}
