using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingCharacter : MonoBehaviour
{

    public Rigidbody mainPlayer;
    public int speed;

    private Vector3 base_position;

    // Start is called before the first frame update
    void Start()
    {
        base_position = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, base_position + mainPlayer.transform.position, Time.deltaTime * speed);
        
        //transform.position = base_position * mainPlayer.transform.position;
        //transform.Translate(mainPlayer.transform.position - base_position);
        /*
        transform.position = new Vector3(
            mainPlayer.transform.position.x,
            mainPlayer.transform.position.y,
            mainPlayer.transform.position.z
        );*/
    }

}
