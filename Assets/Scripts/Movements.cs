using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Movements : MonoBehaviour
{
    [Header("General")]
    public Rigidbody rb;
    public Camera cam;
    public GameManager gameManager;
    public float doubleClickTime;
    public Ability ability;

    [Header("UI")]
    public Image dragBarUI;
    public Gradient DragBarColor;
    public GameObject circle;
    public GameObject arrow;
    public AlternativeWeaponHandler weaponHandler;

    [Header("Values")]
    public float force;
    public float speedLimit;
    public float dragTime;

    const float dragDistanceMax = .8f; // Correspond to a ratio multiplied by the narrower screen part size
    const int dragDistanceMin = 40;
    const float dragTimeWeapons = .2f;
    const int circleTimeApparitionSpeed = 4;

    private float squaredSpeedLimit;
    private float squaredDragDistanceMin;
    private float dragTimeStart;
    private float dragTimeEnd;
    private float clickTime;
    private bool canDrag = true;
    private bool doubleTapTriggered = false;
    private bool forceNeedsToBeApplied = false;
    private bool dragDistanceMinReached = false;
    private Vector3 forceToApply;

    Vector3 click_position;


    void Start()
    {
        squaredSpeedLimit = speedLimit * speedLimit;
        squaredDragDistanceMin = dragDistanceMin * dragDistanceMin;
        dragTimeEnd = -dragTime;
    }

    void Update()
    {
        var ratio = Mathf.Clamp((Time.time - dragTimeEnd) / dragTime, 0, 1);
        dragBarUI.color = DragBarColor.Evaluate(ratio);
        dragBarUI.transform.localScale = new Vector2(ratio, 1);
        if (ratio == 1){ canDrag = true; }
        
    }

    bool CanOpenWeapons(){ return (!(dragDistanceMinReached)) && ((Time.time - clickTime) > dragTimeWeapons); }
    bool ShouldDragCharacter(double characterMagnitude) { return (characterMagnitude > squaredDragDistanceMin) || dragDistanceMinReached; }

    Vector3 CalculateForceRotation()
    {
        var cameraAngle = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up);
        var forceVector = new Vector3(Input.mousePosition.x - click_position.x, 0, Input.mousePosition.y - click_position.y);

        return (cameraAngle * forceVector * -1);
    }

    float CalculateForceRatio()
    {

        return Mathf.Clamp
            (
                (
                    Vector2.Distance(Input.mousePosition, click_position) /
                    (Mathf.Min(Screen.width, Screen.height) * .5f * dragDistanceMax)
                ), 0, 1
            );
    }

    void OnMouseDown()
    {

        click_position = Input.mousePosition;
        dragTimeStart = Time.time;

        if ((Time.time <= (clickTime + doubleClickTime)) && doubleTapTriggered)
        {
            ability.use();
            doubleTapTriggered = false;
        }
        else { doubleTapTriggered = true; }
        clickTime = Time.time;

    }

    void OnMouseDrag()
    {
  
        if (!(canDrag) || !(gameManager.isPlaying) || weaponHandler.IsActive()) { return; }

        var apparitionRatio = (Time.time - dragTimeStart) * circleTimeApparitionSpeed;
        var forceRotation = CalculateForceRotation();

        // All the conditions are meet to enable character's dragging
        if (ShouldDragCharacter(forceRotation.sqrMagnitude))
        {
            dragDistanceMinReached = true;
            circle.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, apparitionRatio);
            arrow.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Min(CalculateForceRatio(), apparitionRatio));

            if (forceRotation == Vector3.zero) { return; }
            else
            {
                arrow.transform.rotation = Quaternion.LookRotation(forceRotation);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    Quaternion.LookRotation(forceRotation),
                    100
                );
            }
        }
        // if not, check if we should instead open weapons menu
        else
        {
            // If minimum drag distance is not reached, and if user has pressed the screen long enough, then we consider user action as a long click, and therefore open weapons menu
            if (CanOpenWeapons())
            {
                weaponHandler.Enter();
            }
        }

    }

    void OnMouseUp()
    {
        weaponHandler.Exit();
        if (!(canDrag) || !(gameManager.isPlaying)) { return; }
        if (!(dragDistanceMinReached)) { return; }

        circle.transform.localScale = Vector3.zero;
        arrow.transform.localScale = Vector3.zero;

        forceToApply = CalculateForceRotation().normalized * force * CalculateForceRatio();

        if (rb.velocity.sqrMagnitude > squaredSpeedLimit) { return; }
        else
        {
            if ((rb.velocity.sqrMagnitude + forceToApply.sqrMagnitude) > squaredSpeedLimit) {

                forceToApply = forceToApply * (float)((squaredSpeedLimit - rb.velocity.sqrMagnitude) / forceToApply.sqrMagnitude);

            }

            rb.velocity = new Vector3();
            
            forceNeedsToBeApplied = true;
            dragTimeEnd = Time.time;
            canDrag = false;
            dragDistanceMinReached = false;

        }                 
    }

    void FixedUpdate()
    {
        if (forceNeedsToBeApplied)
        {
            rb.AddForce(forceToApply, ForceMode.Impulse);
            forceNeedsToBeApplied = false;
        }
    }
}
