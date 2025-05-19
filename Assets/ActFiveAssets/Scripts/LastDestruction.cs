using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDestruction : MonoBehaviour, IDamageable
{
    public GameObject pathToEarth; // Assign in Inspector
    public static event Action<GameObject> OnObjectDestroyed;
    public GameObject deviceHotspot;

    public void Damage(float damage)
    {
        pathToEarth.SetActive(true);
        deviceHotspot.SetActive(true);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("LastDestruction OnDestroy called for " + gameObject.name);
        if (OnObjectDestroyed != null)
        {
            Debug.Log("OnObjectDestroyed event has subscribers. Invoking...");
            if (SoundManager.Instance != null && SoundManager.Instance.playerChannel != null && SoundManager.Instance.playerPass != null)
            {
                SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerPass);
            }
            else
            {
                Debug.LogWarning("SoundManager or playerPass audio not set up!");
            }

            OnObjectDestroyed(gameObject);
        }
        else
        {
            Debug.LogWarning("No subscribers to OnObjectDestroyed event!");
        }
    }
}