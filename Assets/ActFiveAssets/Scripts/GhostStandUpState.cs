using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStandUpState : StateMachineBehaviour
{
    private BossGhost boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.BossChannel.PlayOneShot(SoundManager.Instance.BossRise);
        boss = animator.GetComponent<BossGhost>();
        boss.isVunrable = false;
        boss.chargeBossGuitar();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("STANDUP");
    }
}