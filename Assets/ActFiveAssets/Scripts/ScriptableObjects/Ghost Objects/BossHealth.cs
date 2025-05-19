using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{

    public Slider bossSlider;
    public Slider easeBossSlider;
    public float maxHealth = 2000;
    public float bossHealth;
    private float lerpSpeed = 0.05f;
    private BossGhost bossGhost;

    // Start is called before the first frame update
    void Start()
    {
        bossHealth = maxHealth;
        // Find the BossGhost component in the scene
        bossGhost = FindObjectOfType<BossGhost>();
        if (bossGhost != null)
        {
            // Subscribe to the damage event
            bossGhost.OnDamageTaken += lowerBossHealth;
        }
        else
        {
            Debug.LogError("BossGhost not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bossSlider.value != bossHealth)
        {
            bossSlider.value = bossHealth;
        }

        if (bossSlider.value != easeBossSlider.value)
        {
            easeBossSlider.value = Mathf.Lerp(easeBossSlider.value, bossHealth, lerpSpeed);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (bossGhost != null)
        {
            bossGhost.OnDamageTaken -= lowerBossHealth;
        }
    }

    void lowerBossHealth(float damage)
    {
        bossHealth -= damage;
    }



}
