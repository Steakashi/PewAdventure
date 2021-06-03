using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeWeaponHandler : MonoBehaviour
{

    public GameObject weapons;

    private bool active = false;
    private GraphicsManager vision;

    void Start()
    {
        vision = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GraphicsManager>();
    }

    public bool IsActive() { return active; }

    public void Enter()
    {
        active = true;
        weapons.SetActive(true);
        vision.slow();
    }

    public void Exit()
    {
        active = false;
        weapons.SetActive(false);
        vision.restore();
    }
}
