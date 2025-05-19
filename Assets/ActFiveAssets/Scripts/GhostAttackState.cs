using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAttackState : StateMachineBehaviour
{

    Transform player;
    NavMeshAgent agent;

    public float stopAttackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogWarning("Player not found in " + animator.gameObject.name);
        }
        if (agent == null || !agent.isOnNavMesh || !agent.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("NavMeshAgent is not ready or GameObject is inactive in " + animator.gameObject.name);
        }

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
            SoundManager.Instance.ghostChannel.PlayOneShot(SoundManager.Instance.ghostAttack);
           
        }


        LookAtPlayer();

        // --- Checking if the agent should stop Attacking --- //

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }


    }
    

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.ghostChannel.Stop();
    }

}
