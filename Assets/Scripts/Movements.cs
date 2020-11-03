using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Movements : MonoBehaviour
{

    public Rigidbody rb;
    public Camera cam;
    public GameObject circle;
    public GameObject arrow;
    public float force;
    public float speedLimit;
    public float dragTime;

    public Image dragBarUI;
    public Gradient DragBarColor;

    const float dragDistanceMax = .8f; // Correspond to a ratio multiplied by the narrower screen part size
    const int circleTimeApparitionSpeed = 4;

    private float squaredSpeedLimit;
    private float dragTimeStart;
    private float dragTimeEnd;
    private bool canDrag = true;
  

    Vector3 click_position;


    void Start()
    {
        squaredSpeedLimit = speedLimit * speedLimit;
        dragTimeEnd = -dragTime;
    }

    void Update()
    {
        var ratio = Mathf.Clamp((Time.time - dragTimeEnd) / 2, 0, 1);
        dragBarUI.color = DragBarColor.Evaluate(ratio);
        dragBarUI.transform.localScale = new Vector2(ratio, 1);
        if (ratio == 1){ canDrag = true; }
    }

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

    }

    void OnMouseDrag()
    {

        if (!(canDrag)) { return; }

        var apparitionRatio = (Time.time - dragTimeStart) * circleTimeApparitionSpeed;
        var forceRotation = CalculateForceRotation();

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

    void OnMouseUp()
    {

        if (!(canDrag)) { return; }

        circle.transform.localScale = Vector3.zero;
        arrow.transform.localScale = Vector3.zero;

        var forceApplied = CalculateForceRotation().normalized * force * CalculateForceRatio();

        if (rb.velocity.sqrMagnitude > squaredSpeedLimit) { return; }
        else
        {
            if ((rb.velocity.sqrMagnitude + forceApplied.sqrMagnitude) > squaredSpeedLimit) {

                forceApplied = forceApplied * (float)((squaredSpeedLimit - rb.velocity.sqrMagnitude) / forceApplied.sqrMagnitude);

            }

            rb.velocity = new Vector3();
            rb.AddForce(forceApplied, ForceMode.Impulse);

            dragTimeEnd = Time.time;
            canDrag = false;
        }

      
    }
}
