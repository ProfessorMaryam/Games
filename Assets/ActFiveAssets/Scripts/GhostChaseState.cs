using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostChaseState : StateMachineBehaviour
{

    NavMeshAgent agent;
    Transform player;

    public float chaseSpeed = 6f;

    public float stopChasingDistance = 60;
    public float attackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- Initialization --- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        if (agent == null || !agent.isOnNavMesh || !agent.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("NavMeshAgent is not ready or GameObject is inactive in " + animator.gameObject.name);
            return;
        }

        agent.speed = chaseSpeed;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (agent == null || !agent.isOnNavMesh || !agent.gameObject.activeInHierarchy)
        {
            return;

        }

        if (SoundManager.Instance.ghostChannel.isPlaying == false)
        {
            SoundManager.Instance.ghostChannel.PlayOneShot(SoundManager.Instance.ghostRun);
          //  SoundManager.Instance.ghostChannel.PlayDelayed(1f);
        }


        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // --- Checking if agent should stop Chasing --- //

        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        // --- Checking if the agent should Attack --- //

        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || !agent.isOnNavMesh || !agent.gameObject.activeInHierarchy)
        {
            return;
        }

        agent.SetDestination(animator.transform.position);

        SoundManager.Instance.ghostChannel.Stop();
    }

}
