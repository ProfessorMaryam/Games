using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAct : MonoBehaviour
{
    public GameObject finalQuest;
    public GameObject healthBar;
    private void OnTriggerEnter(Collider other)
    {
        finalQuest.SetActive(false);
        healthBar.SetActive(false);
    }

}
