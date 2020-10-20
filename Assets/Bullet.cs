using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int TimeToLive;

    int damages = 10;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeToLive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            collision.gameObject.GetComponent<HealthBar>().SubstractLifePoints(damages);
            Destroy(gameObject, 0);
            
        }

    }
}
