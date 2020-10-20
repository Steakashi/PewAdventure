using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{

    public GameObject projectile;
    public Camera cam;
    public int FireDamages;
    public int FireSpeed;
    public float FireRate;

    private bool MouseOnObject;
    private float LastShotTime;

    void Start()
    {
        LastShotTime = Time.time - FireSpeed;
    }

    void OnMouseEnter()
    {

        MouseOnObject = true;

    }

    void OnMouseExit()
    {

        MouseOnObject = false;

    }

    Vector3 CalculateProjectileDirection()
    {
        var cameraAngle = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up);
        var directionVector = new Vector3(Input.mousePosition.x - (Screen.width / 2), 0, Input.mousePosition.y - (Screen.height / 2));

        return (cameraAngle * directionVector);
    }


    void Update()
    {
        Debug.DrawLine(transform.position, CalculateProjectileDirection(), Color.cyan);

        if (Input.GetButtonDown("Fire1") && !(MouseOnObject) && (Time.time > LastShotTime + FireRate))
        {
            LastShotTime = Time.time;
            var missileDirection = CalculateProjectileDirection().normalized;
            GameObject bullet = Instantiate(
                projectile,
                transform.position + (missileDirection * 2) + (Vector3.up * 1),
                Quaternion.LookRotation(missileDirection)
            ) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 10 * FireSpeed);
        }

    }
}
