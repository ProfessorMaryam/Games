using AC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Marc : MonoBehaviour
{
    public int HP = 250;
    public GameObject blackVision;

    public TextMeshProUGUI playerStringUI;
    public GameObject destringifingUI;
    public GameObject marcHealthBar;

    public GameObject myFavRecordPlayer;
    public GameObject myFavGrandPiano;
    public GameObject myFavGuitarAmp;

    [SerializeField] private AnimatorOverrideController destringifingAnimator;
    
    [SerializeField] private float respawnDelay = 3f;
    private BlackOut blackOut;

    private PlayerSpawnManager respawnManager;
    public bool isDestringified;
    public GameObject quests;
    //public bool isFightingBoss = false;
    private Animator cameraAnimator;
    [SerializeField] private string spawnAnimationName = "Spawn";
    [SerializeField] private AudioClip respawnSound;

    public event Action<float> OnDamageTaken;

    private void Start()
    {
        playerStringUI.text = $"Strings: {HP}";
        //GetComponent<Player>().enabled = true;
        GetComponent<PlayerStrike>().enabled = true;
        blackOut = GetComponent<BlackOut>();
        respawnManager = FindObjectOfType<PlayerSpawnManager>(); // Note the class name change

        if (respawnManager == null)
        {
            Debug.LogError("PlayerSpawnManager not found in the scene!", gameObject);
        }

        Transform firstPersonCamera = transform.Find("Player");
        if (firstPersonCamera != null)
        {
            cameraAnimator = firstPersonCamera.GetComponent<Animator>();
            if (cameraAnimator == null)
            {
                Debug.LogWarning("Animator not found on FirstPersonCamera!", firstPersonCamera.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("FirstPersonCamera not found as a child of Player!", gameObject);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        OnDamageTaken?.Invoke(damageAmount);

        if (HP <= 0)
        {
            Debug.Log("Dead Marc!");
            //deathPosition = parentTransform.position;
            PlayerDead();
            isDestringified = true;
        }

        else
        {
            Debug.Log("Marc Hurtin'");
            StartCoroutine(BlackVisionEffect());
            playerStringUI.text = $"Strings: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        quests.SetActive(false);

        myFavRecordPlayer.GetComponent<AudioSource>().Stop();
        myFavGrandPiano.GetComponent<AudioSource>().Stop(); 
        myFavGuitarAmp.GetComponent<AudioSource>().Stop();
        
        SoundManager.Instance.ObjectChannel.Stop();
        SoundManager.Instance.BossChannel.Stop();
        //SoundManager.Instance.finaleChannel.Stop();
        SoundManager.Instance.ghostChannel.Stop();
        SoundManager.Instance.spawnChannel.Stop();

        //isFightingBoss = true;
        //Area.SetActive(true);
        //Boss.SetActive(true);
        //BossGuitar.SetActive(true);
        //isFinale = true;


        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDie);
        
        GetComponent<Player>().enabled = false;
        //GetComponent<CursorInfluenceCamera>().enabled = false;
        GetComponent<PlayerStrike>().enabled = false;


        // Dying Animation
        GetComponentInChildren<Animator>().enabled = true;

        marcHealthBar.SetActive(false);
        //playerStringUI.gameObject.SetActive(false);

        // Destrinifing Player
        GetComponent<BlackOut>().StartFade();
        StartCoroutine(ShowDestringifingUI());
    }

    private IEnumerator ShowDestringifingUI()
    {
        yield return new WaitForSeconds(1f);
        destringifingUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(respawnDelay - 1f); 
        Respawn();
    }

    private void Respawn()
    {
        
        quests.SetActive(true);
        
        // Restore health
        HP = 250;
        playerStringUI.text = $"Strings: {HP}";
        isDestringified = false;

        // Re-enable components
        GetComponent<Player>().enabled = true;
        GetComponent<PlayerStrike>().enabled = true;

        // Reset player animator
        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.enabled = false; // Disable animator or reset to idle
        }

        // Restore UI
        marcHealthBar.SetActive(true);
        destringifingUI.SetActive(false);

        // Notify MarcHealth to restore health
        MarcHealth marcHealth = FindObjectOfType<MarcHealth>();
        if (marcHealth != null)
        {
            marcHealth.RestoreFullHealth();
        }
        else
        {
            Debug.LogWarning("MarcHealth not found for respawn!");
        }

        // Respawn to PlayerSpawn position
        if (respawnManager != null)
        {
            respawnManager.RespawnPlayer();
        }
        else
        {
            Debug.LogWarning("PlayerSpawnManager not found for respawn!");
        }


        // Play spawn animation on FirstPersonCamera to reverse death animation
        if (cameraAnimator != null)
        {
            cameraAnimator.Play(spawnAnimationName);
            Debug.Log($"Playing spawn animation '{spawnAnimationName}' on FirstPersonCamera.");
        }
        else
        {
            Debug.LogWarning("Cannot play spawn animation: Camera Animator not found!");
        }

        // Reset FirstPersonCamera's local transform after animation (optional safety)
        Transform firstPersonCamera = transform.Find("FirstPersonCamera");
        if (firstPersonCamera != null)
        {
            StartCoroutine(ResetCameraTransformAfterAnimation(firstPersonCamera));
        }

        // Fade in screen
        if (blackOut != null)
        {
            blackOut.StartFadeIn();
        }
        else
        {
            Debug.LogWarning("BlackOut component not found for fade-in!", gameObject);
        }

        // Play respawn sound
        if (respawnSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.playerChannel.PlayOneShot(respawnSound);
        }

        Debug.Log("Marc Respawned to PlayerSpawn position.");
    }

    private IEnumerator ResetCameraTransformAfterAnimation(Transform cameraTransform)
    {
        // Wait for the animation to finish (adjust duration based on your animation length)
        yield return new WaitForSeconds(1f); // Replace with actual animation length if known

        // Reset local position and rotation to ensure alignment
        cameraTransform.localPosition = new Vector3(0f, 1.5f, 0f); // Adjust to your desired eye level
        cameraTransform.localRotation = Quaternion.identity; // Reset rotation
        Debug.Log($"FirstPersonCamera local position reset to: {cameraTransform.localPosition}, local rotation: {cameraTransform.localRotation}");
    }

    private IEnumerator BlackVisionEffect()
    {
        if (blackVision.activeInHierarchy == false)
        {
            blackVision.SetActive(true);
        }

        var image = blackVision.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.
        }

        if (blackVision.activeInHierarchy)
        {
            blackVision.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GhostHand"))
        {
            if (isDestringified == false)
            {
                TakeDamage(other.gameObject.GetComponent<GhostHand>().damage);
            }
        }

        if (other.CompareTag("MoonTerrain"))
        {
            PlayerDead();
        }
    }


}
