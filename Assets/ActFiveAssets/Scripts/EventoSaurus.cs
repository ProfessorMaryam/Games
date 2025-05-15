using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventoSaurus : MonoBehaviour, IDamageable
{
    public PlayerCollection playerCollection;
    public void Damage(float damage)
    {
        Destroy(gameObject);
        SoundManager.Instance.ObjectChannel.PlayOneShot(SoundManager.Instance.collectSound);
    }

    public void OnDestroy()
    {
        playerCollection = GameObject.FindWithTag("Player").GetComponent<PlayerCollection>();

        if (playerCollection != null)
        {
            playerCollection.EventoSaurusCollected();
        }

    }
}