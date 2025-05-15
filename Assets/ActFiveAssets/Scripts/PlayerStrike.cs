using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrike : MonoBehaviour
{
    public static Action strikeInput;
    public static Action chargeInput;
    public static Action guitarBreak;

    [SerializeField] private KeyCode chargeKey;
    [SerializeField] private KeyCode breakKey;
  


    private void Update()
    {
        if (Input.GetMouseButton(0))
         strikeInput?.Invoke();
        
        if (Input.GetKeyDown(chargeKey)) 
            chargeInput?.Invoke();

        if (Input.GetKeyDown(breakKey))
            guitarBreak?.Invoke();
    }
}
