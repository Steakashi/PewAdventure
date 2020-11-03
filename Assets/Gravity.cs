using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    public Rigidbody rb;

    private int layerMask = 1 << 8;
    private float equilibriumPoint = 3.0f;
    private float raycastLength;
    private float stableUpForceMax = 10.0f;
    private float StabilizedVelocity = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.raycastLength = equilibriumPoint + 2.0f;
    }


    void TiltFromGround(RaycastHit hit)
    {
        var reflected = Vector3.Reflect(transform.forward, hit.normal);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(
                new Vector3(
                    reflected.x,
                    reflected.y,
                    reflected.z
                )
            ),
            0.1f
        );
    }

    void PushPlayerUp(RaycastHit hit)
    {
        // If Player starts to go up, then we consider the real equilibrium point.
        // If not, we take the maximum raycastLength in order to apply a stronger force
        var equilibriumPointConsidered = rb.velocity.y > 0 ? equilibriumPoint: raycastLength;
        var distanceRatio = (((hit.distance / equilibriumPointConsidered) - 1) * -1);
        rb.AddForce(Vector3.up * distanceRatio * stableUpForceMax);

    }

    void StabilizePlayer()
    {
        Debug.Log(rb.velocity.sqrMagnitude);
        if (rb.velocity.sqrMagnitude > (StabilizedVelocity * StabilizedVelocity))
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(rb.velocity),
                0.1f
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * raycastLength, Color.yellow);
        if (Physics.Raycast(transform.position, -transform.up, out hit, raycastLength, layerMask))
        {

            PushPlayerUp(hit);
            StabilizePlayer();
            TiltFromGround(hit);

        }
    }
        
}
