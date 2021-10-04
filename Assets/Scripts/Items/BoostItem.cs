using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostItem : MonoBehaviour
{
    [SerializeField] float speedBoost = 10f;

    public float GetSpeedBoost() => speedBoost;
}
