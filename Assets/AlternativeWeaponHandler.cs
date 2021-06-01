using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeWeaponHandler : MonoBehaviour
{

    public GameObject weapons;

    private bool active = false;

    public bool IsActive() { return active; }

    public void Enter()
    {
        active = true;
        weapons.SetActive(true);
    }

    public void Exit()
    {
        active = false;
        weapons.SetActive(false);
    }
}
