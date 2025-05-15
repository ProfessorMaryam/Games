using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatagoryDestroyTracker : MonoBehaviour
{
    public GameObject rewardCanvas; // Assign the corresponding canvas (e.g., GlassesRewardCanvas or MoneyPacksRewardCanvas)
    public float displayDuration = 5f; // Duration to display the canvas (5 seconds)

    private List<Transform> children = new List<Transform>(); // List to track child objects
    private bool hasTriggered = false; // Prevent multiple triggers

    private void Start()
    {
        // Populate the list of children at the start
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy) // Only add active children
            {
                children.Add(child);
            }
        }

        // Ensure the canvas is initially disabled
        if (rewardCanvas != null)
        {
            rewardCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Reward Canvas not assigned in " + gameObject.name);
        }

        Debug.Log(gameObject.name + " has " + children.Count + " children to destroy.");
    }

    private void Update()
    {
        // Skip if already triggered
        if (hasTriggered) return;

        // Check if all children are destroyed (inactive or null)
        bool allDestroyed = true;
        foreach (Transform child in children)
        {
            if (child != null && child.gameObject.activeInHierarchy)
            {
                allDestroyed = false;
                break;
            }
        }

        // If all children are destroyed, show the reward canvas
        if (allDestroyed)
        {
            hasTriggered = true;
            StartCoroutine(ShowRewardCanvas());
            Debug.Log(gameObject.name + " - All children destroyed! Showing reward.");
        }
    }

    private IEnumerator ShowRewardCanvas()
    {
        if (rewardCanvas != null)
        {
            SoundManager.Instance.ObjectChannel.PlayOneShot(SoundManager.Instance.trophySound);
            rewardCanvas.SetActive(true);
            yield return new WaitForSeconds(displayDuration);
            rewardCanvas.SetActive(false);
        }
    }
}