using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : Ability
{
    
    public float speedBoost;
    public Rigidbody rb;

    public override void use()
    {
        if (rb.velocity.sqrMagnitude < 1) { return; }
        rb.AddForce(rb.velocity.normalized * speedBoost, ForceMode.Impulse);
    }
}
