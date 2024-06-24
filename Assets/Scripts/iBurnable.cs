using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iBurnable
{
    public delegate void Effect();

    public bool OnFire { get; set; }
    public Effect Burning();
}