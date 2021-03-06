using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public List<Image> heartsOnFire;
    public int lives = 3;

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth()
    {
        slider.value = 100;
    }

    public int LivesLeft()
    {
        return heartsOnFire.Count;
    }

    void Update()
    {
        if (slider.value <= 0)
        {
            int heartsLeft = LivesLeft();

            if (heartsLeft > 1)
            {
                Destroy(heartsOnFire[heartsLeft - 1]);
                heartsOnFire.RemoveAt(heartsLeft - 1);

                slider.value = 100;
            }
            else if (heartsLeft == 1)
            {
                Destroy(heartsOnFire[0]);
                heartsOnFire.RemoveAt(0);
            }
        }    
    }
}
