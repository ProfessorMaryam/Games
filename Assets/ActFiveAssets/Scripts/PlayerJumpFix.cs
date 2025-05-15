using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpFix : MonoBehaviour
{
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.3f;
    public Transform groundCheckPoint;
    private bool isGrounded;

    void Update()
    {
        GroundCheck();

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("We could jump here!");
            // You can send a signal to another script, or use Rigidbody directly if allowed
        }
    }

    void GroundCheck()
    {
        // Fallback to object's position if no custom point
        Vector3 checkPos = groundCheckPoint ? groundCheckPoint.position : transform.position;
        isGrounded = Physics.CheckSphere(checkPos, groundCheckDistance, groundLayer);

        Debug.DrawRay(checkPos, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

}