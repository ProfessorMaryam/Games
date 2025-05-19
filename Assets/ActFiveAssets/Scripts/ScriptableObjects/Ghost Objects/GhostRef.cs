using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRef : MonoBehaviour
{
    public GhostHand ghostHand;

    public int ghostDamage;

    private void Start()
    {
        ghostHand.damage = ghostDamage;

    }
}
