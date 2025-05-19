using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathToEarth : MonoBehaviour
{
    public string device; // Not used currently, consider removing unless needed
    public GameObject oldFightArea; // Assign in Inspector

    private void Awake()
    {
        // Subscribe to the event when the script is loaded
        LastDestruction.OnObjectDestroyed += OnDeviceDestroyed;
        Debug.Log(gameObject.name + " subscribed to OnObjectDestroyed event in Awake.");
    }

    private void Start()
    {
        gameObject.SetActive(false); // Initially disabled
    }

    private void OnEnable()
    {
        if (oldFightArea != null)
        {
            oldFightArea.SetActive(false);
            Debug.Log(gameObject.name + " activated and old fight area disabled.");
        }
        else
        {
            Debug.LogWarning("oldFightArea is not assigned in Inspector!");
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        LastDestruction.OnObjectDestroyed -= OnDeviceDestroyed;
    }

    private void OnDeviceDestroyed(GameObject destroyedObject)
    {
        if (destroyedObject == gameObject) return; // Avoid self-triggering
        if (string.IsNullOrEmpty(device) || destroyedObject.CompareTag(device))
        {
            Debug.Log("OnDeviceDestroyed called. Activating " + gameObject.name);
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        // Cleanup on destroy (optional)
        LastDestruction.OnObjectDestroyed -= OnDeviceDestroyed;
    }
}