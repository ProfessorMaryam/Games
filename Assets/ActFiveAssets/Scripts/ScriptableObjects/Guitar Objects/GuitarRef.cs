using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarRef : MonoBehaviour
{
    public GuitarHead guitarHead;

    public int guitarHeadDamage;
    private void Start()
    {
        guitarHead.damage = guitarHeadDamage;

    }
}
