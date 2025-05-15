using AC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    private Vector3 respawnPosition; // Store the PlayerSpawn position
    private Transform playerTransform; // Reference to the player's transform
    private Rigidbody rb; // Reference to the player's Rigidbody
   
    public string currentSpawner;

    private void Awake()
    {
        // Find the player
        GameObject player = GameObject.FindWithTag("Player");
        playerTransform = player ? player.transform : null;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found in the scene!", gameObject);
            return;
        }

        // Cache the Rigidbody
        rb = playerTransform.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody not found on player! Ensure it’s attached.", playerTransform.gameObject);
        }
    }

    private void Update()
    {
       
            // Find the GameObject tagged with "PlayerSpawn" and store its position
            GameObject playerSpawn = GameObject.FindWithTag(currentSpawner);
            if (playerSpawn != null)
            {
                respawnPosition = playerSpawn.transform.position;
                //Debug.Log($"PlayerSpawn position stored: {respawnPosition}");
            }
            else
            {
                //Debug.LogError($"GameObject with tag {currentSpawner} not found in the scene!", gameObject);
            }
        

    }

    public void RespawnPlayer()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player transform not found for respawn!");
            return;
        }

        if (respawnPosition == Vector3.zero)
        {
            Debug.LogWarning($"Respawn position not set! Ensure a GameObject is tagged with {currentSpawner}'.");
            return;
        }

        // Debug the current state before respawn
        Debug.Log($"Player position before respawn: {playerTransform.position}");
        Transform firstPersonCamera = playerTransform.Find("FirstPersonCamera");
        Transform redGuitar = playerTransform.Find("WeaponHolder/RedGuitar");
        Debug.Log($"FirstPersonCamera position before respawn: {firstPersonCamera?.position}");
        Debug.Log($"RedGuitar position before respawn: {redGuitar?.position}");
        if (rb != null)
        {
            Debug.Log($"Rigidbody velocity before respawn: {rb.velocity}, IsKinematic: {rb.isKinematic}, Constraints: {rb.constraints}");
        }

        // Reset Rigidbody state
        if (rb != null)
        {
            rb.isKinematic = false; // Ensure physics is active
            rb.velocity = Vector3.zero; // Clear velocity
            rb.angularVelocity = Vector3.zero; // Clear angular velocity
            rb.constraints = RigidbodyConstraints.FreezeRotation; // Freeze rotation but allow position movement
            rb.useGravity = true; // Ensure gravity is enabled
        }

        // Move the entire hierarchy to the respawn position
        Vector3 safeRespawnPosition = respawnPosition + Vector3.up * 0.5f; // Small offset above ground
        playerTransform.position = safeRespawnPosition; // Move the root transform

        // Sync child positions relative to the new parent position (optional, if needed)
        if (firstPersonCamera != null)
        {
            firstPersonCamera.localPosition = firstPersonCamera.localPosition; // Reset local position to maintain offset
        }
        if (redGuitar != null)
        {
            redGuitar.localPosition = redGuitar.localPosition; // Reset local position
            MeshCollider mc = redGuitar.GetComponent<MeshCollider>();
            if (mc != null)
            {
                mc.isTrigger = true; // Prevent physics interaction
            }
            Rigidbody rbChild = redGuitar.GetComponent<Rigidbody>();
            if (rbChild != null)
            {
                rbChild.isKinematic = true; // Make it kinematic if it has a Rigidbody
            }
        }

        // Ensure Rigidbody aligns with the new position
        if (rb != null)
        {
            rb.MovePosition(safeRespawnPosition); // Sync with Rigidbody
        }

        // Debug the state after respawn
        Debug.Log($"Player position after respawn: {playerTransform.position}");
        Debug.Log($"FirstPersonCamera position after respawn: {firstPersonCamera?.position}");
        Debug.Log($"RedGuitar position after respawn: {redGuitar?.position}");
        if (rb != null)
        {
            Debug.Log($"Rigidbody velocity after respawn: {rb.velocity}, IsKinematic: {rb.isKinematic}, Constraints: {rb.constraints}");
        }

        // Optionally, reset the camera and fade in (similar to PlayerStart behavior)
        GameObject playerSpawn = GameObject.FindWithTag(currentSpawner);
        if (playerSpawn != null)
        {
            PlayerStart playerStartComponent = playerSpawn.GetComponent<PlayerStart>();
            if (playerStartComponent != null)
            {
                if (playerStartComponent.cameraOnStart != null)
                {
                    playerStartComponent.SetCameraOnStart();
                }

                if (playerStartComponent.fadeInOnStart)
                {
                    KickStarter.mainCamera.FadeIn(playerStartComponent.fadeSpeed);
                }

                KickStarter.player.SetLookDirection(playerStartComponent.ForwardDirection, true);
            }
        }
    }
}