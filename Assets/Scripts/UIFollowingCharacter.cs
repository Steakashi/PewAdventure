using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowingCharacter : MonoBehaviour
{

    public Rigidbody mainPlayer;

    private RectTransform rect_transform;

    // Start is called before the first frame update
    void Start()
    {
        rect_transform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {   
        rect_transform.position = new Vector3(
            mainPlayer.transform.position.x,
            mainPlayer.transform.position.y,
            mainPlayer.transform.position.z
        );
    }

}
