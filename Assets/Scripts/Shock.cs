using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : AlternativeWeapon
{

    public GameObject ShockEffect;
    public Rigidbody player;

    public override void Trigger()
    {
        GameObject shockEffect = Instantiate(ShockEffect, player.transform.position, transform.rotation) as GameObject;
    }
}
