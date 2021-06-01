using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockEnergy : MonoBehaviour
{
    [Header("General")]
    public string targetTag;

    [Header("Values")]
    public int damages;
    public float maxSize;
    public float timeToLive;

    private List<string> enemiesCollided;
    private float birth;
    private float scale;
    private float ratio;
    private Color materialColor;
    private GameObject player;


    void Start()
    {
        birth = Time.time;
        enemiesCollided = new List<string>();
        materialColor = gameObject.GetComponent<Renderer>().material.color;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        ratio = (Time.time - birth) / timeToLive;

        if (ratio >= 1) { Destroy(gameObject); }

        scale = maxSize * ratio;
        transform.localScale = new Vector3(scale, scale, scale);

        Material material = new Material(Shader.Find("Transparent/Diffuse"));
        material.color = new Color(1.0f, 1.0f, 1.0f, -4.0f + (ratio * 4.0f));
        GetComponent<Renderer>().material = material;

        transform.position = player.transform.position;

    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == targetTag && !(enemiesCollided.Contains(target.name)))
        {
            enemiesCollided.Add(target.name);
            target.gameObject.GetComponent<HealthBar>().SubstractLifePoints(damages);
        }

    }
}
