using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private float weight = 0.25f;
    [SerializeField] [Range(-5f, 0)] private float rotationalEffect = -0.2f;

    public float GetWeight() => weight;
    public int GetValue() => value;

    public float GetRotationalEffect => rotationalEffect;

}
