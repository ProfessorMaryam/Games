using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAct : MonoBehaviour
{
    public GameObject firstQuest;
    public GameObject marcHealth;
    private void OnTriggerEnter(Collider other)
    {
        firstQuest.SetActive(true);
        marcHealth.SetActive(true);
    }
}
