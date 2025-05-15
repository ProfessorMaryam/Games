using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRiseGhostsState : StateMachineBehaviour
{
    private BossGhost boss;
    private Transform player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossGhost>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Trigger ghost spawning
        //if (boss != null) boss.SpawnGhostWave();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            // Face player while summoning
            animator.gameObject.transform.LookAt(
                new Vector3(player.position.x,
                           animator.transform.position.y,
                           player.position.z));
        }
    }
}