using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [Header("General")]
    public Rigidbody Target;

    [Header("Movements")]
    public float acceleration;
    public float maxSpeed;
    public float angularSpeed;
    public float startPursuit;
    public float startRotating;
    public float speedRotating;
    public float maximumShootingAngle = 90;
    public float maximumShootingDistance = 10;
    public float sideVisibility = 1;
    public float safetyRange;
    public float safetyForce;

    [Header("Weapon")]
    public GameObject projectile;
    public int fireDamages;
    public int fireSpeed;
    public float fireRate;

    private  NavMeshAgent agent;
    private Rigidbody rb;
    private float weightedDistanceToTarget;
    private float maximumSqrShootingDistance;
    private float angleRatio;
    private float lastShotTime;
    private Vector3 flattened_velocity;
    private int layerMask = 1 << 8;

    private float squaredStartRotating;
    private float squaredSpeedLimit;
    private int turningDirection = 1;

    private bool canShoot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;
        agent.updateRotation = false;
        

        maximumSqrShootingDistance = maximumShootingDistance * maximumShootingDistance;
        lastShotTime = Time.time - fireSpeed;

        squaredStartRotating = startRotating * startRotating;
        squaredSpeedLimit = maxSpeed * maxSpeed;
    }


    bool CanFire(){ return (Time.time > lastShotTime + fireRate); }

    bool CloseEnough(Vector3 fromPositionToPlayer) { return fromPositionToPlayer.sqrMagnitude < (startPursuit * startPursuit); }

    bool ObstacleDetected()
    {
        RaycastHit hit;
        flattened_velocity = new Vector3(
            rb.velocity.normalized.x,
            0,
            rb.velocity.normalized.z
        );
        return !(Physics.Raycast(transform.position + (flattened_velocity * safetyRange), Vector3.down, out hit, Mathf.Infinity, layerMask));
    }

    bool CanAim(Vector3 fromPositionToPlayer)
    {
        angleRatio = 1 - ((Mathf.Clamp(Vector3.Angle(transform.forward, fromPositionToPlayer), 0, maximumShootingAngle)) / maximumShootingAngle);
        weightedDistanceToTarget = maximumSqrShootingDistance * angleRatio * sideVisibility;

        return (fromPositionToPlayer.sqrMagnitude < weightedDistanceToTarget * weightedDistanceToTarget);
    }

    Vector3 ClampWithMaxSpeed(Vector3 forceApplied)
    {
        
        if ((rb.velocity.sqrMagnitude + forceApplied.sqrMagnitude) > squaredSpeedLimit)
        {
            return forceApplied * (float)((squaredSpeedLimit - rb.velocity.sqrMagnitude) / forceApplied.sqrMagnitude);
        }

        return forceApplied;
    }

    Vector3 ClampWithTurningDistance(Vector3 fromPositionToPlayer, Vector3 forceApplied)
    {
        return forceApplied * Mathf.Clamp(((fromPositionToPlayer.sqrMagnitude - squaredStartRotating) / (squaredStartRotating * 0.5f)), -1, 1);
    }

    Vector3 ApplyRotationMovement(Vector3 forceApplied)
    {

        return transform.right * turningDirection * (speedRotating / 10);
    }

    void ApplyMovement(Vector3 fromPositionToPlayer)
    {

        Vector3 forceApplied = new Vector3(
            fromPositionToPlayer.x,
            0,
            fromPositionToPlayer.z
        ).normalized * (acceleration / 10);
        Vector3 rotationApplied = Vector3.zero;

        
        if (fromPositionToPlayer.sqrMagnitude < squaredStartRotating * 1.5f)
        {
            forceApplied = ClampWithTurningDistance(fromPositionToPlayer, forceApplied);
            rotationApplied = ApplyRotationMovement(forceApplied);
        }

        Debug.DrawRay(transform.position, forceApplied * 5 , Color.cyan);

        if ((rb.velocity + forceApplied + rotationApplied).sqrMagnitude > squaredSpeedLimit) { return; }
        rb.AddForce(forceApplied + rotationApplied, ForceMode.Acceleration);
    }

    void ApplyRotation(Vector3 fromPositionToPlayer)
    {
        var rotation = Vector3.RotateTowards(
            transform.forward,
            new Vector3(
                fromPositionToPlayer.x,
                0,
                fromPositionToPlayer.z
            ),
           angularSpeed / 10000,
            0.0f
        );

        transform.rotation = Quaternion.LookRotation(rotation);
    }

    void Fire(Vector3 fromPositionToPlayer)
    {

        lastShotTime = Time.time;
        var missileDirection = fromPositionToPlayer.normalized;
        GameObject bullet = Instantiate(
            projectile,
            transform.position + (missileDirection * 5),
            Quaternion.LookRotation(missileDirection)
        ) as GameObject;
        bullet.GetComponent<Bullet>().AssignTarget("Player");
        bullet.GetComponent<Bullet>().setDamages(fireDamages);
        bullet.GetComponent<Rigidbody>().AddForce(fromPositionToPlayer.normalized * 10 * fireSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fromPositionToPlayer = Target.position - transform.position;

        agent.Warp(transform.position);
        agent.destination = Target.position;

        if (!(CloseEnough(fromPositionToPlayer))) {
            rb.velocity *= 0.99f;
        }
        else if (ObstacleDetected()){ rb.AddForce(-(rb.velocity.normalized * (safetyForce / 10))); }
        else
        {

            ApplyMovement(fromPositionToPlayer);
            ApplyRotation(fromPositionToPlayer);

            if (CanAim(fromPositionToPlayer) && (CanFire())){ Fire(fromPositionToPlayer); }

        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, startPursuit);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, startRotating);
    }

    private void OnTriggerEnter(Collider other){ turningDirection *= -1;}

}
