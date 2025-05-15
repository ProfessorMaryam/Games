using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalGhostSpawner : MonoBehaviour
{
//    [Header("Settings")]
//    public GameObject ghostPrefab;
//    public float spawnDelay = 0.5f;
//    public int ghostsPerWave = 5; // Adjust based on boss difficulty

//    [Header("Effects")]
//    public GameObject spawnEffectPrefab;
//    public float effectDuration = 2f;

//    private List<Transform> spawnPoints = new List<Transform>();
//    private List<GameObject> currentGhosts = new List<GameObject>();

//    void Awake()
//    {
//        // Cache all spawn points (children with "Spawner" tag)
//        foreach (Transform child in transform)
//        {
//            if (child.CompareTag("Spawner"))
//            {
//                spawnPoints.Add(child);
//            }
//        }

//        if (spawnPoints.Count == 0)
//        {
//            Debug.LogError("No spawn points found in " + gameObject.name);
//        }
//    }

//    // Call this from BossGhost to spawn a wave
//    public void SpawnWave()
//    {
//        StartCoroutine(SpawnWaveCoroutine());
//    }

//    IEnumerator SpawnWaveCoroutine()
//    {
//        for (int i = 0; i < ghostsPerWave; i++)
//        {
//            if (spawnPoints.Count == 0) yield break;

//            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
//            SpawnGhost(spawnPoint.position);
//            yield return new WaitForSeconds(spawnDelay);
//        }
//    }

//    void SpawnGhost(Vector3 position)
//    {
//        if (ghostPrefab == null) return;

//        // Spawn ghost
//        GameObject ghost = Instantiate(ghostPrefab, position, Quaternion.identity);
//        currentGhosts.Add(ghost);

//        // Set up ghost
//        Ghost ghostScript = ghost.GetComponent<Ghost>();
//        if (ghostScript != null)
//        {
//            ghostScript.SetSpawnerController(this); // For death notifications
//        }

//        // Spawn effect
//        if (spawnEffectPrefab != null)
//        {
//            GameObject effect = Instantiate(spawnEffectPrefab, position, Quaternion.identity);
//            Destroy(effect, effectDuration);
//        }
//    }

//    // Called by Ghost when it dies
//    public void NotifyGhostDeath(GameObject ghost)
//    {
//        if (currentGhosts.Contains(ghost))
//        {
//            currentGhosts.Remove(ghost);
//        }
//    }

//    // Check if all ghosts are dead (call this from BossGhost)
//    public bool AreAllGhostsDead()
//    {
//        return currentGhosts.Count == 0;
//    }
}