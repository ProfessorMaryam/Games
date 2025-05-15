using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostPatrolingState : StateMachineBehaviour
{

    Transform player;
    NavMeshAgent agent;

    public float detectionArea = 45f;
    public float patrolSpeed = 2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- Initialization --- //

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogWarning("Player not found in " + animator.gameObject.name);
        }
        if (agent == null || !agent.isOnNavMesh || !agent.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("NavMeshAgent is not ready or GameObject is inactive in " + animator.gameObject.name);
            return;
        }

        agent.speed = patrolSpeed;

        agent.SetDestination(player.position);
        animator.transform.LookAt(player);


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null || agent == null || !agent.isOnNavMesh || !agent.gameObject.activeInHierarchy)
        {
            return;
        }

        if (SoundManager.Instance.ghostChannel.isPlaying == false)
        {
            SoundManager.Instance.ghostChannel.clip = SoundManager.Instance.ghostWalk;
            SoundManager.Instance.ghostChannel.PlayDelayed(1f);
        }

        // --- Transition to Chase State --- //

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //agent.SetDestination(agent.transform.position);
        SoundManager.Instance.ghostChannel.Stop();
    }

}
