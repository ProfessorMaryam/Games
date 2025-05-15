using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAct : MonoBehaviour
{
    public GameObject firstQuest;
    public GameObject marcHealth;
    public bool started = false;
    public GameObject friendTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (!started)
        {
            firstQuest.SetActive(true);
            marcHealth.SetActive(true);
            started = true;
            Destroy(friendTrigger);
            Destroy(gameObject);
        }
        
    }
}
