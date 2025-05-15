using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightArea : MonoBehaviour
{
    public GameObject Area;
    public GameObject Boss;
    public GameObject BossGuitar;
    public bool isFinale = false;
    //public Marc marc;


    private void OnTriggerEnter(Collider other)
    {
        if (isFinale == false)
        {
            Boss.SetActive(true); 
            SoundManager.Instance.finaleChannel.Play();
            Area.SetActive(true);
            BossGuitar.SetActive(true);
            isFinale = true;
        }
    }

}
