
using System;
using UnityEngine;

public class BonFire: MonoBehaviour, iBurn
{
    public bool CanBurn { get; set; }

    private void Start()
    {
        CanBurn = true;
    }
}