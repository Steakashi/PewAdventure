using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int TimeToLive;

    private int damages;
    private string targetTag;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeToLive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignTarget(string givenTargetTag)
    {
        targetTag = givenTargetTag;
    }

    public void setDamages(int givenDamages)
    {
        damages = givenDamages;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == targetTag)
        {
            //If the GameObject has the same tag as specified, output this message in the console
            collision.gameObject.GetComponent<HealthBar>().SubstractLifePoints(damages);
            Debug.Log(collision.gameObject);
            Destroy(gameObject, 0);
            
        }

    }
}
