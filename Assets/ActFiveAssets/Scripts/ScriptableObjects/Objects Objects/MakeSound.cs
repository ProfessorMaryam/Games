using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour, IDamageable
{
    [Header("Audio Settings")]
    public AudioSource voiceSource; // Assign the AudioSource component in the Inspector
    public AudioClip voiceClipToPlay; // Assign the AudioClip in the Inspector

    private bool canPlayVoice = true;
    public float playCooldown = 0.5f; // Adjust the cooldown time as needed
    public bool hasBeenStruck = false;

    void Awake()
    {
        // Ensure an AudioSource is assigned
        if (voiceSource == null)
        {
            voiceSource = GetComponent<AudioSource>();
            if (voiceSource == null)
            {
                voiceSource = gameObject.AddComponent<AudioSource>();
                Debug.LogWarning($"No AudioSource found on '{gameObject.name}'. One has been automatically added.");
            }
        }
        voiceSource.playOnAwake = false;
        voiceSource.loop = false;
    }

    public void Damage(float damage)
    {
        hasBeenStruck = true;
        if (canPlayVoice && voiceSource != null && voiceClipToPlay != null)
        {
            voiceSource.PlayOneShot(voiceClipToPlay);
            canPlayVoice = false;
            Invoke("ResetCanPlayVoice", playCooldown);
            Debug.Log($"'{gameObject.name}' played voice '{voiceClipToPlay.name}' on damage.");
        }
        else if (!canPlayVoice)
        {
            Debug.Log($"'{gameObject.name}' voice is on cooldown.");
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' cannot play voice. AudioSource or AudioClip is not assigned.");
        }
    }

    public void Currupted()
    {

        if (voiceSource != null && voiceSource.isPlaying)
        {
            voiceSource.Stop();
            voiceSource.enabled = false;
        }

        canPlayVoice = false;
    }

    private void ResetCanPlayVoice()
    {
        canPlayVoice = true;
    }
}
