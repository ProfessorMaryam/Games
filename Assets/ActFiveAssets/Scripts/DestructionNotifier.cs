using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionNotifier : MonoBehaviour
{
    public static event Action<GameObject> OnObjectDestroyed;
    private void OnDestroy()
    {
        if (OnObjectDestroyed != null)
        {
            OnObjectDestroyed(gameObject);
        }
    }
}