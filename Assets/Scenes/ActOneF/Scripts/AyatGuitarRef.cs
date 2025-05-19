using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyatGuitarRef : MonoBehaviour
{
    public AyatGuitarHead guitarHead;

    public int guitarHeadDamage;
    private void Start()
    {
        guitarHead.damage = guitarHeadDamage;

    }
}
