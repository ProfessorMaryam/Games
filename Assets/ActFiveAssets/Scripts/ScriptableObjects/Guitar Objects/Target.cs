using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 10f;
    [SerializeField] private AudioClip damageSound; // Audio clip for damage sound

    public void Damage(float damage)
    {
        health -= damage;
        Debug.Log($"Took damage: {damage}, Remaining health: {health}");

        // Play damage sound if assigned
        if (damageSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.TargetChannel.PlayOneShot(damageSound);
        }
        else
        {
            if (damageSound == null)
            {
                Debug.LogWarning("Damage sound not assigned in Target!", gameObject);
            }
            if (SoundManager.Instance == null)
            {
                Debug.LogWarning("SoundManager instance not found!", gameObject);
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Target Destroyed!");
        }
    }
}