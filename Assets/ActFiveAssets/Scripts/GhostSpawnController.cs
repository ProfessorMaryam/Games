// Inside GhostSpawnController.cs

using AC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostSpawnController : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ghostPrefab;
    public float spawnDelay = 0.5f;

    [Header("Wave Control")]
    public int numberOfWaves = 2;
    public float waveCooldown = 5.0f;
    private int currentWave = 0;
    private bool inCooldown = false;
    private float cooldownCounter = 0;

    [Header("Ghost Spawn Effect")]
    public GameObject ghostSpawnEffectPrefab; // Assign this in the Inspector
    public float effectPlayTime = 2f;
    public float effectFadeOutTime = 1f;

    [Header("Door Control")]
    public GameObject door;
    
    public string device;
   
    private List<Transform> spawnPoints = new List<Transform>();
    private HashSet<GameObject> currentGhostsAlive = new HashSet<GameObject>();
    private bool areaActivated = false;

    public GameObject previousQuest;

    void OnEnable()
    {
        DestructionNotifier.OnObjectDestroyed += HandleDeviceDestruction;
        
        FindSpawnPoints();
    }

    void OnDisable()
    {
        DestructionNotifier.OnObjectDestroyed -= HandleDeviceDestruction;
        
    }

    void Update()
    {
        if (areaActivated)
        {
            if (inCooldown)
            {
                cooldownCounter -= Time.deltaTime;
                if (cooldownCounter <= 0)
                {
                    inCooldown = false;
                    StartNextWave();
                }
            }
            else if (currentGhostsAlive.Count == 0 && currentWave < numberOfWaves && !inCooldown)
            {
                inCooldown = true;
                cooldownCounter = waveCooldown;
                Debug.Log(gameObject.name + ": Wave " + currentWave + " cleared. Starting cooldown.");
            }
            else if (currentWave >= numberOfWaves && currentGhostsAlive.Count == 0)
            {
                Debug.Log(gameObject.name + ": All waves completed for " + gameObject.name + ".");
                Destroy(door);
                previousQuest.SetActive(false);
                SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerPass);
                enabled = false;
            }
        }
    }

    void FindSpawnPoints()
    {
        spawnPoints.Clear();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Spawner"))
            {
                spawnPoints.Add(child);
            }
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogError(gameObject.name + ": No spawn points found!");
            enabled = false;
        }
        else
        {
            Debug.Log(gameObject.name + ": Found " + spawnPoints.Count + " spawn points.");
        }
    }

    void HandleDeviceDestruction(GameObject destroyedObject)
    {
        //GameObject Device = GameObject.FindGameObjectWithTag(device);
        if (destroyedObject != null && destroyedObject.CompareTag(device))
        {
            areaActivated = true;
            StartNextWave();
            Debug.Log(gameObject.name + " activated.");
            DestructionNotifier.OnObjectDestroyed -= HandleDeviceDestruction;
        }
        else
        {
            Debug.Log("Error in Handling.");
        }
    }

    void StartNextWave()
    {
        currentWave++;
        Debug.Log(gameObject.name + ": Starting Wave " + currentWave);
        StartCoroutine(SpawnWaveWithEffect());
    }

    IEnumerator SpawnWaveWithEffect()
    {
        if (ghostPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Ghost Prefab not assigned!");
            yield break;
        }

        foreach (Transform spawnPoint in spawnPoints)
        {
            // Instantiate the ghost
            GameObject spawnedGhost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
            NavMeshAgent ghostAgent = spawnedGhost.GetComponent<NavMeshAgent>();

            if (ghostAgent != null)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(spawnPoint.position, out hit, 1f, NavMesh.AllAreas))
                {
                    spawnedGhost.transform.position = hit.position;
                    ghostAgent.enabled = true;
                    currentGhostsAlive.Add(spawnedGhost);
                    Ghost ghostScript = spawnedGhost.GetComponent<Ghost>();
                    if (ghostScript != null)
                    {
                        ghostScript.SetSpawnerController(this);
                    }

                    // Instantiate the spawn effect at the ghost's creation position
                    if (ghostSpawnEffectPrefab != null)
                    {
                        GameObject effectInstance = Instantiate(ghostSpawnEffectPrefab, spawnedGhost.transform.position, Quaternion.identity);
                        StartCoroutine(FadeOutEffect(effectInstance));

                        // Play the sound effect using the SoundManager
                        if (SoundManager.Instance != null && SoundManager.Instance.spawnChannel != null && SoundManager.Instance.spawnSound != null)
                        {
                            SoundManager.Instance.spawnChannel.PlayOneShot(SoundManager.Instance.spawnSound);
                        }
                        else
                        {
                            Debug.LogWarning(gameObject.name + ": SoundManager or spawn audio resources not properly set up.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning(gameObject.name + ": Ghost Spawn Effect Prefab not assigned!");
                    }
                }
                else
                {
                    Debug.LogError("Could not find a valid NavMesh position for spawned ghost at " + spawnPoint.position);
                    Destroy(spawnedGhost);
                    continue;
                }
            }
            else
            {
                Debug.LogError("Spawned ghost prefab does not have a NavMeshAgent component!");
                Destroy(spawnedGhost);
                continue;
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        Debug.Log(gameObject.name + ": Wave " + currentWave + " spawned " + spawnPoints.Count + " ghosts.");
    }

    IEnumerator FadeOutEffect(GameObject effect)
    {
        yield return new WaitForSeconds(effectPlayTime);

        Renderer[] renderers = effect.GetComponentsInChildren<Renderer>();
        float timer = 0f;
        Color[] originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null && renderers[i].material != null && renderers[i].material.HasProperty("_Color"))
            {
                originalColors[i] = renderers[i].material.color;
            }
        }

        while (timer < effectFadeOutTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / effectFadeOutTime);

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].material != null && renderers[i].material.HasProperty("_Color"))
                {
                    Color newColor = originalColors[i];
                    newColor.a *= alpha;
                    renderers[i].material.color = newColor;
                }
            }
            yield return null;
        }

        Destroy(effect);
    }

    public void NotifyGhostDied(GameObject ghost)
    {
        if (currentGhostsAlive.Contains(ghost))
        {
            currentGhostsAlive.Remove(ghost);
            Debug.Log(gameObject.name + ": Ghost fully died. Remaining: " + currentGhostsAlive.Count);
        }
    }
}