using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShieldSurface : MonoBehaviour
{

    //[SerializeField] public float HP = 200f;
    //private Animator animator;
    //public bool isBroken;
    //public BossGhost myBoss;

    //private void Start()
    //{
    //    gameObject.SetActive(false);
    //    animator = GetComponent<Animator>();
    //}

    //public void Damage(float damage)
    //{
    //    HP -= damage;

    //    if (HP <= 0 && !isBroken)
    //    {
    //        StartCoroutine(Break());
    //    }
    //    else if (HP > 0)
    //    {
    //        animator.SetTrigger("SHIELDDAMAGE");
    //    }
    //}

    //IEnumerator Break()
    //{
    //    animator.SetTrigger("BREAK");
    //    isBroken = true;
    //    yield return new WaitForSeconds(2f);
    //    gameObject.SetActive(false);
    //    Debug.Log("Shield Is Brokennnnn!!!!!");
    //    myBoss.isShieldActive = false;

    //}

    //public void Open()
    //{
    //    if (animator != null)
    //    {
    //        gameObject.SetActive(true);
    //        myBoss.isShieldActive=true;
    //        animator.SetTrigger("OPEN");
    //        HP = 200;
    //        isBroken = false;
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("GuitarHead"))
    //    {
    //        //Debug.Log("Touched The Shield");
    //        if (isBroken == false)
    //        {
    //            GuitarHead guitarHead = other.GetComponent<GuitarHead>();
    //            if (guitarHead != null)
    //            {
    //                Debug.Log("Damaged The Shield");
    //                Damage(guitarHead.damage); // Access the damage variable
    //            }
    //        }
    //    }
    //}
}
