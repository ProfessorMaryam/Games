using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RecordPlayer : MonoBehaviour, IDamageable
{
    [Header("Audio Settings")]
    public List<AudioClip> musicTracks; // List of music to play
    private int currentTrackIndex = 0;
    public AudioSource audioSource;
    private bool isPlaying = false;

    [Header("Interaction Settings")]
    public float interactionDistance = 2f;
    public string playerCameraTag = "MainCamera"; // Tag of the player's first-person camera
    private Transform playerCamera;
    private bool canInteract = false;
    public bool hasBeenStruck = false; // To track if it has been struck by the guitar

    [Header("Record Player Visuals")]
    public GameObject disc;
    public GameObject arm;
    private int mode = 0;
    private float armAngle = 0.0f;
    private float discAngle = 0.0f;
    private float discSpeed = 0.0f;

    void Awake()
    {
        if (disc == null)
            disc = transform.Find("teller").gameObject;
        if (arm == null)
            arm = transform.Find("arm").gameObject;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
        }

        if (musicTracks.Count > 0 && audioSource.clip == null)
        {
            audioSource.clip = musicTracks[currentTrackIndex];
        }

        playerCamera = GameObject.FindGameObjectWithTag(playerCameraTag)?.transform;
        if (playerCamera == null)
        {
            Debug.LogError("Player's first-person camera not found with tag: " + playerCameraTag);
        }
    }

    void Update()
    {
        CheckInteraction();
        HandleInput();
        UpdateVisuals();
        UpdateAudio();
    }

    void CheckInteraction()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        canInteract = false;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                canInteract = true;
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerCamera.position);
        if (distanceToPlayer > interactionDistance)
        {
            canInteract = false;
        }
    }

    void HandleInput()
    {
        if (!canInteract) return;

        // Mouse button 0 (left-click): Switch to the next song if struck
        if (Input.GetMouseButtonDown(0) && hasBeenStruck)
        {
            NextTrack();
            isPlaying = true; // Ensure it starts playing after switching
        }

        // Mouse button 1 (right-click): Stop the record player
        if (Input.GetMouseButtonDown(1))
        {
            if (hasBeenStruck)
            {
                StopPlayer();
            }
        }
    }

    void UpdateVisuals()
    {
        // Mode 0: Player off
        if (mode == 0)
        {
            if (isPlaying)
                mode = 1;
        }
        // Mode 1: Activation
        else if (mode == 1)
        {
            if (isPlaying)
            {
                armAngle += Time.deltaTime * 30.0f;
                if (armAngle >= 30.0f)
                {
                    armAngle = 30.0f;
                    mode = 2;
                }
                discAngle += Time.deltaTime * discSpeed;
                discSpeed += Time.deltaTime * 80.0f;
            }
            else
            {
                mode = 3;
            }
        }
        // Mode 2: Running
        else if (mode == 2)
        {
            if (isPlaying)
                discAngle += Time.deltaTime * discSpeed;
            else
                mode = 3;
        }
        // Mode 3: Stopping
        else
        {
            if (!isPlaying)
            {
                armAngle -= Time.deltaTime * 30.0f;
                if (armAngle <= 0.0f)
                    armAngle = 0.0f;

                discAngle += Time.deltaTime * discSpeed;
                discSpeed -= Time.deltaTime * 80.0f;
                if (discSpeed <= 0.0f)
                    discSpeed = 0.0f;

                if (discSpeed == 0.0f && armAngle == 0.0f)
                    mode = 0;
            }
            else
            {
                mode = 1;
            }
        }

        // Update objects
        if (arm != null)
            arm.transform.localEulerAngles = new Vector3(0.0f, armAngle, 0.0f);
        if (disc != null)
            disc.transform.localEulerAngles = new Vector3(0.0f, discAngle, 0.0f);
    }

    void UpdateAudio()
    {
        if (isPlaying)
        {
            if (!audioSource.isPlaying && musicTracks.Count > 0)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); // Use Stop instead of Pause to reset the track
            }
        }

        // Check if the current track has finished playing
        if (audioSource.isPlaying && !audioSource.loop && audioSource.time >= audioSource.clip.length)
        {
            NextTrack();
        }
    }

    void NextTrack()
    {
        if (musicTracks.Count == 0)
        {
            Debug.LogWarning("No music tracks assigned to the Record Player.");
            audioSource.Stop();
            return;
        }

        currentTrackIndex++;
        if (currentTrackIndex >= musicTracks.Count)
        {
            currentTrackIndex = 0; // Loop back to the beginning
        }

        audioSource.clip = musicTracks[currentTrackIndex];
        if (isPlaying)
        {
            audioSource.Play();
        }
    }

    void StopPlayer()
    {
        isPlaying = false;
        audioSource.Stop();
        Debug.Log("Record Player stopped via right-click.");
    }

    public void Damage(float damage)
    {
        if (!hasBeenStruck)
        {
            // First strike: Turn on and play
            hasBeenStruck = true;
            isPlaying = true;
            Debug.Log("Record Player struck and turned ON!");
        }
        else
        {
            // Subsequent strikes: Switch to the next song
            NextTrack();
            isPlaying = true;
            Debug.Log("Record Player struck - switching to next song!");
        }
    }

    public void Currupted()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        isPlaying = false;
        if (audioSource != null)
        {
            audioSource.enabled = false;
        }

        canInteract = false;
        hasBeenStruck = true;

        mode = 0;
        discSpeed = 0f;
        armAngle = 0f;
        discAngle = 0f;

        Debug.Log("Record Player has been corrupted and disabled.");
    }
}