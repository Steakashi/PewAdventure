using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public int maxLifePoints;
    public Slider healthBarSlider;
    public Image healthBarImage;
    public Gradient healthBarColor;

    private int lifePoints;

    // Start is called before the first frame update
    void Start()
    {
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

    public void SubstractLifePoints(int value)
    {
        Debug.Log("SubstractLifePoints");
        lifePoints -= value;
        UpdateHealthBarUI();
        if (IsDead())
        {
            Destroy(gameObject, 0);
        }
    }

}
