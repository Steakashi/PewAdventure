using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    public Rigidbody rb;
    public Collider playerBody;
    public float equilibriumPoint;
    public bool stabilizePlayer;

    private int layerMask = 1 << 8;
    private float raycastLength;
    private float stableUpForceMax = 10.0f;
    private float StabilizedVelocity = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.raycastLength = equilibriumPoint * 2;
    }


    void TiltFromGround(RaycastHit hit)
    {
        var reflected = Vector3.Reflect(transform.forward, hit.normal);

        // Gradually lower angular velocity of player to make him face the path he follows
        if (rb.angularVelocity.sqrMagnitude > .5) { rb.angularVelocity *= 0.95f; }

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
        if (rb.velocity.sqrMagnitude > (StabilizedVelocity * StabilizedVelocity))
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(rb.velocity),
                1f
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
            if (stabilizePlayer){ StabilizePlayer();}
            
            TiltFromGround(hit);

        }
    }
        
}
