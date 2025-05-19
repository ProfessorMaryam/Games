using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossGhost : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    public float bossHealth = 2000;
    public float phaseTransitionHealth = 200f;
    public bool isDead = false;
    public bool isVunrable = false;

    [SerializeField] private float deathDelay = 5f;

    private Animator animator;
    private NavMeshAgent navAgent;
    private Transform player;
    public BossGuitar bossGuitar;
    public GameObject ghostsSpawner;
    public GameObject fightArea;
    public GameObject BossHealthBar;
    public GameObject BossDevice;
    public GameObject fightEnterance;
    public GameObject BassQuest;
    public GameObject FinalQuest;
    public GameObject MarcHealth;

    public event Action<float> OnDamageTaken;

    void Start()
    {
        BassQuest.SetActive(true);
        BossHealthBar.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
        SoundManager.Instance.finaleChannel.Play();
        
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }    
  

    public void Damage(float damage)
    {
        if (isVunrable == false)
        {
            animator.SetTrigger("DAMAGE");
        }

        if (isVunrable)
        {
            bossHealth -= damage;

            OnDamageTaken?.Invoke(damage);

            if (bossHealth <= 0)
            {
                Die();
            }
        }
    }
    
    public void Striked()
    {
        SoundManager.Instance.BossChannel.PlayOneShot(SoundManager.Instance.BossCover);
        isVunrable = true;
        Debug.Log("Boss. Crouch.");
        animator.SetTrigger("CROUCH");
    }

    public void chargeBossGuitar()
    {
        bossGuitar.Charge();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.6f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 45f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 50f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GuitarHead"))
        {
            if (isDead == false && isVunrable == true )
            {
                Damage(other.gameObject.GetComponent<GuitarHead>().damage);
            }
        }
    }


    void Die()
    {
        animator.SetTrigger("DIE");
        SoundManager.Instance.BossChannel.PlayOneShot(SoundManager.Instance.BossDie);

        BassQuest.SetActive(false);
        isDead = true;
        if (navAgent != null && navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true;
            navAgent.enabled = false;
        }

        ghostsSpawner.SetActive(false);

        KillAllGhosts();
        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(deathDelay);
        FinalQuest.SetActive(true);
        fightArea.SetActive(false);
        if (isDead)
        {
            SoundManager.Instance.finaleChannel.Stop();
            //gameObject.GetComponent<AudioSource>().Stop();
        }
        BossDevice.SetActive(true);
        fightEnterance.SetActive(false);
        BossHealthBar.SetActive(false);
        MarcHealth.SetActive(false);
        Destroy(gameObject);
    }

    private void KillAllGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts)
        {
            Ghost ghostScript = ghost.GetComponent<Ghost>();
            if (ghostScript != null)
            {
                ghostScript.Damage(75);
                Debug.Log("Killed ghost: " + ghost.name);
            }
            else
            {
                Debug.LogWarning("Ghost " + ghost.name + " has no GhostScript component!");
            }
        }
    }


    //Ok

}