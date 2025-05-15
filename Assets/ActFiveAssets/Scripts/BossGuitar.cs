using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossGuitar : MonoBehaviour, IDamageable
{
    [SerializeField] public float HP = 300f;
    private Animator animator;
    public BossGhost myBoss;
    public GameObject shield;

    //strike prefab

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Charge()
    {
        HP = 300f;
        Debug.Log("Boss Guitar Charged.");
    }

    public void Damage(float damage)
    {
        HP -= damage;
        SoundManager.Instance.ObjectChannel.PlayOneShot(SoundManager.Instance.BGuitarHit);

        if (HP <= 0)
        {
            Strike();
            SoundManager.Instance.ObjectChannel.PlayOneShot(SoundManager.Instance.BGuitarStruck);
        }
        else if (HP > 0)
        {
            if (HP < 100)
            {
                Destroy(shield);
            }

            Debug.Log("Boss Guitar Striked!");
            //Create Strike Prefab following the boss guitar
        }
    }

    public void Strike()
    {

        Debug.Log("Guitar Striking Its Owner!!!!!");
        myBoss.Striked(); 

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GuitarHead"))
        {
                GuitarHead guitarHead = other.GetComponent<GuitarHead>();
                if (guitarHead != null)
                {
                    Debug.Log("Damaged The Guitar");
                    //Damage(guitarHead.damage); 
                    myBoss.Damage(guitarHead.damage);
                }
            }
        }
    }

