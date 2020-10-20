using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public int maxLifePoints;
    public Slider healthBarUI;

    private int lifePoints;

    // Start is called before the first frame update
    void Start()
    {
        lifePoints = maxLifePoints;
        healthBarUI.value = 1;
    }

    bool IsDead()
    {
        return lifePoints <= 0 ? true : false;
    }

    bool MaxHealthReached()
    {
        return lifePoints >= maxLifePoints ? true : false;
    }

    void UpdateHealthBarUI()
    {
        if (IsDead()) { healthBarUI.value = 0; }
        else if (MaxHealthReached()) { healthBarUI.value = 1; }
        else {
            healthBarUI.value = (float)(lifePoints) / (float)(maxLifePoints);
            Debug.Log(healthBarUI.value);
        }
    }

    public void addLifePoints(int value)
    {
        if (lifePoints + value > maxLifePoints) { lifePoints = maxLifePoints; }
        else { lifePoints += value; }
    }

    public void SubstractLifePoints(int value)
    {
        Debug.Log(lifePoints);
        lifePoints -= value;
        UpdateHealthBarUI();
        if (IsDead())
        {
            Destroy(gameObject, 0);
        }
    }

}
