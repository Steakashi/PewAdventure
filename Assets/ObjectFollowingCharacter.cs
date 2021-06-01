using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowingCharacter : MonoBehaviour
{

    public Rigidbody player;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
    }
}
