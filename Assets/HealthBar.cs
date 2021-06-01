using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;

public class HealthBar : MonoBehaviour
{

    public int maxLifePoints;
    public Slider healthBarSlider;
    public Image healthBarImage;
    public Gradient healthBarColor;
    public GameObject explosionEffect;

    private int lifePoints;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lifePoints = maxLifePoints;
        healthBarSlider.value = 1;
    }

    bool IsDead()
    {
        return lifePoints <= 0 ? true : false;
    }

    bool MaxHealthReached()
    {
        return lifePoints >= maxLifePoints ? true : false;
    }

    void updateHealthBarColor(float ratio)
    {
        healthBarImage.color = healthBarColor.Evaluate(ratio);
    }

    void UpdateHealthBarUI()
    {
        if (IsDead()) { healthBarSlider.value = 0; }
        else if (MaxHealthReached()) { healthBarSlider.value = 1; }
        else {
            var ratio = (float)(lifePoints) / (float)(maxLifePoints);
            healthBarSlider.value = ratio;

            if (healthBarImage != null) { updateHealthBarColor(ratio); }
        }
    }

    public void addLifePoints(int value)
    {
        if (lifePoints + value > maxLifePoints) { lifePoints = maxLifePoints; }
        else { lifePoints += value; }
    }

    IEnumerator deathAnimation()
    {
        while (transform.localScale.sqrMagnitude > 0.01) {
            transform.localScale -= new Vector3(0.001f, 0.001f, 0.001f);
            yield return null;

        }

        Destroy(gameObject, 0);

    }

    public void kill()
    {
        GameObject explosion = Instantiate(explosionEffect);
        explosion.transform.position = transform.position;
        GetComponent<Gravity>().enabled = false;

        if (this.gameObject.tag == "Player")
        {
            GetComponent<Movements>().enabled = false;
            GetComponent<BasicWeapon>().enabled = false;
            GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameManager>().lose();
        }
        else
        {
            GetComponent<AI>().enabled = false;
            agent.enabled = false;
            StartCoroutine("deathAnimation");
        }

    }

    public void SubstractLifePoints(int value)
    {
        lifePoints -= value;
        UpdateHealthBarUI();
        if (IsDead())
        {
            kill();
        }
    }

}
