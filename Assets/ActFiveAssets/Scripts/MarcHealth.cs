using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarcHealth : MonoBehaviour
{
    public Slider marcSlider;
    public Slider easeMarcSlider;
    public float maxHealth = 250;
    public float marcHealth;
    private float lerpSpeed = 0.05f;
    private Marc marc;

    // Start is called before the first frame update
    void Start()
    {
        marcHealth = maxHealth;
        
        marc = FindObjectOfType<Marc>();
        if (marc != null)
        {
            // Subscribe to the damage event
            marc.OnDamageTaken += lowerMarcHealth;
        }
        else
        {
            Debug.LogError("Marc not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (marcSlider.value != marcHealth)
        {
            marcSlider.value = marcHealth;
        }

        if (marcSlider.value != easeMarcSlider.value)
        {
            easeMarcSlider.value = Mathf.Lerp(easeMarcSlider.value, marcHealth, lerpSpeed);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (marc != null)
        {
            marc.OnDamageTaken -= lowerMarcHealth;
        }
    }

    void lowerMarcHealth(float damage)
    {
        marcHealth -= damage;
    }

    public void RestoreFullHealth()
    {
        marcHealth = maxHealth;
        // Sliders will update automatically in Update()
        Debug.Log("Marc health restored to full: " + marcHealth);
    }


}

