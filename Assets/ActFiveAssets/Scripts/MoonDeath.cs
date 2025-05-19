using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonDeath : MonoBehaviour
{
    public Marc marc;
    private void OnTriggerEnter(Collider other)
    {
        marc.HP = 0;
    }
}
