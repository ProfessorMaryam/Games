using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFireState : StateMachineBehaviour
{
    private BossGhost boss;
    private Transform player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossGhost>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Start crate dropping through main script
        //if (boss != null) boss.StartCrateDropSequence();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            // Continuous player tracking
            animator.transform.LookAt(
                new Vector3(player.position.x,
                           animator.transform.position.y,
                           player.position.z));
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}