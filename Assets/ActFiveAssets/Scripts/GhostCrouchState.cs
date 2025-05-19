using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCrouchState : StateMachineBehaviour
{
    private BossGhost boss;
    private float crouchTimer = 0f;
    public float crouchDuration = 7f; // Publicly adjustable duration in the Inspector

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossGhost>();
        //boss.isVunrable = true;
        crouchTimer = 0f; // Reset the timer when entering the crouch state
        Debug.Log("Boss entered Crouch state - Vulnerable!");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        crouchTimer += Time.deltaTime;

        if (crouchTimer >= crouchDuration)
        {
            animator.SetTrigger("STANDUP"); // Trigger the stand up animation
            Debug.Log("Boss Crouch duration reached - Triggering Stand Up.");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optional: Reset the timer here if needed for any reason.
        crouchTimer = 0f;
        Debug.Log("Boss exited Crouch state.");
    }
}