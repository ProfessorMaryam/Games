using AC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCoverState : StateMachineBehaviour
{
    private BossGhost boss;
    private Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossGhost>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            animator.transform.LookAt(
                new Vector3(player.position.x,
                           animator.transform.position.y,
                           player.position.z));
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // You might want to add logic here for when the boss exits the shielded state.
        // For example, you could potentially set a boolean flag in the BossGhost script.
    }
}