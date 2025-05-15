using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour, IDamageable
{
    [SerializeField] private float HP = 200f;
    [SerializeField] private float deathDelay = 2f;
    private Animator animator;
    private NavMeshAgent navAgent;
    public bool isDead;
    private GhostSpawnController spawnerController;
    
    
    // Direct reference to the spawner

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Called by the GhostSpawnController when this ghost is spawned
    public void SetSpawnerController(GhostSpawnController controller)
    {
        spawnerController = controller;
    }

    public void Damage(float damage)
    {
        HP -= damage;

        if (HP <= 0 && !isDead)
        {
            Die();
        }
        else if (HP > 0)
        {
            animator.SetTrigger("DAMAGE");
        }
    }

    private void Die()
    {
        int randomValue = Random.Range(0, 2);

        if (randomValue == 0)
        {
            animator.SetTrigger("DIE1");
        }
        else
        {
            animator.SetTrigger("DIE2");
        }

        isDead = true;
        if (navAgent != null && navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true;
            navAgent.enabled = false;
        }

        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(deathDelay);
        // Directly notify the spawner that this ghost has been destroyed
        spawnerController?.NotifyGhostDied(gameObject);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.6f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 60f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 65f); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GuitarHead"))
        {
            if (isDead == false)
            {
                Damage(other.gameObject.GetComponent<GuitarHead>().damage);
            }
        }
    }

}