using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movements : MonoBehaviour
{

    public Rigidbody rb;
    public Camera cam;
    public float force;

    const float force_factor = .5f;

    Vector3 click_position;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DisplayDebug();
    }

    Vector3 CalculateForceApplied()
    {
        var camera_angle = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up);
        var force_vector = new Vector3(Input.mousePosition.x - click_position.x, 0, Input.mousePosition.y - click_position.y);
        return camera_angle * force_vector;
    }

    void DisplayDebug()
    {
        ///Debug.DrawRay(transform.position, Input.mousePosition - click_position, Color.red);
        /*
        Debug.DrawRay(transform.position,
            new Vector3(
            Input.mousePosition.x - click_position.x,
            0,
            0
        ), Color.green);


        Debug.DrawRay(transform.position,
            new Vector3(
            0,
            0,
            Input.mousePosition.y - click_position.y
        ), Color.blue);*/

        /*var new_vector = CalculateForceApplied();
        Debug.DrawRay(transform.position, new_vector, Color.yellow);
        //var final_vector = cam.transform.rotation * new_vector;
        Debug.Log(cam.transform.rotation.eulerAngles.x);
        Debug.Log(Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.x, Vector3.up));

        Vector3 final_vector = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up) * new_vector;
        //final_vector = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.z, Vector3.right) * final_vector;

        Debug.DrawRay(transform.position, final_vector, Color.cyan);



        //Debug.DrawRay(transform.position, -CalculateForceApplied(), Color.green);*/
    }

    void OnMouseDown()
    {
        // When user click on character for the first time, we store the mouse position for later calculations.
        click_position = Input.mousePosition;

    }

    void OnMouseDrag()
    {
        // Drag is beginning : at this point we juste display a visual element for displaying force level which will be applied later.
        DisplayDebug();

        Vector3 difference = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y) - transform.position;
        Quaternion goalRot = Quaternion.LookRotation(difference);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRot, 50);
    }

    void OnMouseUp()
    {
        
        // Player has released the mouse button : it is now time to launch the character.
        // Current mouse position is substract to previous recorded mouse position and clamped (following characters properties).
        rb.AddForce(-CalculateForceApplied() * force_factor, ForceMode.Impulse);
    }
}
