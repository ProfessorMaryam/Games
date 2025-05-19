using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Guitar", menuName = "Weapon/Guitar")]
public class AyatGuitarData : ScriptableObject
{
        [Header("Info")]
        public new string name;

        [Header("Striking")]
        public float damage;
        public float maxDistance;

        [Header("Charging")]
        public float currentEnergy;
        public float magSize;
        public float strikeRate;
        public float chargeTime;
        [HideInInspector]
        public bool charging;

    }
