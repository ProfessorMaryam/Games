using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyatGuitar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GuitarData guitarData;
    [SerializeField] private Transform cam;

    [Header("Strike Effects")]
    [SerializeField] private GameObject electricityEffect;
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private GameObject sparkPrefab; //This is the lighting spark

    [Header("Strike Sounds")]
    [SerializeField] private AudioClip[] strikeSounds;
    [SerializeField] private AudioSource audioSource;

    private int strikeCount = 0;
    private int[] strikeSoundPattern = new int[] { 0, 0, 0, 1 };

    float timeSinceLastStrike;

    public GameObject guitar;
    private Animator guitarAnimator;
    private float animationDuration = 1f;
    private bool canBreak = true;
    private bool isGuitarAnimating = false;
    private bool canRanOutAnimation = true;
    private bool canChargeAnimation = true;

    private void Start()
    {
        AyatPlayerStrike.strikeInput += Strike;
        AyatPlayerStrike.chargeInput += StartCharge;
        AyatPlayerStrike.guitarBreak += Break;
    }

    private void SetIsAnimating(bool value)
    {
        isGuitarAnimating = value;
    }

    private void ResetIsAnimating()
    {
        isGuitarAnimating = false;
        Debug.Log("Guitar animation finished.");
    }

    public void Break()
    {
        if (canBreak && !isGuitarAnimating)
        {
            canBreak = false;
            isGuitarAnimating = true; // Set flag when animation starts

            guitar = GameObject.FindGameObjectWithTag("Guitar");
            if (guitar != null)
            {
                guitarAnimator = guitar.GetComponent<Animator>();
                if (guitarAnimator != null)
                {
                    guitarAnimator.enabled = true;
                    guitarAnimator.Play("Break", -1, 0f);
                    Debug.Log("Guitar Animation Enabled and Playing - Break");

                    Invoke("EnableBreak", animationDuration);
                    Invoke("DisableGuitarAnimator", animationDuration);
                    Invoke("ResetIsAnimating", animationDuration); // Reset flag after animation
                }
                else
                {
                    // ... error handling ...
                    canBreak = true;
                    isGuitarAnimating = false; // Reset flag on error
                }
            }
            else
            {
                // ... error handling ...
                canBreak = true;
                isGuitarAnimating = false; // Reset flag on error
            }
        }
        else if (isGuitarAnimating)
        {
            Debug.Log("Guitar is currently animating, cannot Break.");
        }
        else
        {
            Debug.Log("Break button is on cooldown.");
        }
    }

    private void EnableBreak()
    {
        canBreak = true;
        Debug.Log("Break button is now available.");
    }

    private void DisableGuitarAnimator()
    {
        if (guitarAnimator != null)
        {
            guitarAnimator.enabled = false;
            Debug.Log("Guitar Animator Disabled");
        }
    }

    public void StartCharge()
    {
        if (!guitarData.charging && canChargeAnimation && !isGuitarAnimating)
        {
            canChargeAnimation = false;
            isGuitarAnimating = true; // Set flag when animation starts

            guitar = GameObject.FindGameObjectWithTag("Guitar");
            if (guitar != null)
            {
                guitarAnimator = guitar.GetComponent<Animator>();
                if (guitarAnimator != null)
                {
                    guitarAnimator.enabled = true;
                    guitarAnimator.Play("Charged", -1, 0f);
                    Debug.Log("Guitar Animation Enabled and Playing - Charge");

                    Invoke("EnableChargeAnimation", animationDuration);
                    Invoke("DisableGuitarAnimator", animationDuration);
                    Invoke("ResetIsAnimating", animationDuration); // Reset flag after animation
                }
                else
                {
                    // ... error handling ...
                    canChargeAnimation = true;
                    isGuitarAnimating = false; // Reset flag on error
                }
            }
            else
            {
                // ... error handling ...
                canChargeAnimation = true;
                isGuitarAnimating = false; // Reset flag on error
            }

            StartCoroutine(Charge());
        }
        else if (guitarData.charging)
        {
            Debug.Log("Already Charging...");
        }
        else if (isGuitarAnimating)
        {
            Debug.Log("Guitar is currently animating, cannot Charge.");
        }
        else
        {
            Debug.Log("Charge animation is on cooldown.");
        }
        Debug.Log("Charging...");
    }

    private void EnableChargeAnimation()
    {
        canChargeAnimation = true;
        Debug.Log("Charge animation is now available.");
    }


    private IEnumerator Charge()
    {
        guitarData.charging = true;

        yield return new WaitForSeconds(guitarData.chargeTime);

        guitarData.currentEnergy = guitarData.magSize;

        guitarData.charging = false;

        Debug.Log("Charged!");
    }

    private IEnumerator ActivateElectricityEffect()
    {
        electricityEffect.SetActive(true); // Show it
        yield return new WaitForSeconds(3f); // Wait 3 seconds
        electricityEffect.SetActive(false); // Hide it
    }

    private bool CanStrike() => !guitarData.charging && timeSinceLastStrike > 1f / (guitarData.strikeRate / 60f);


    public void Strike()
    {
        if (guitarData.currentEnergy > 0)
        {
            if (CanStrike())
            {
                Vector3 strikePosition;
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, guitarData.maxDistance))
                {


                    strikePosition = hitInfo.point;
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(guitarData.damage);
                    SpawnLightning(hitInfo.point);

                    Debug.Log(hitInfo.transform.name);
                    Debug.Log("Striked!");


                }
                else
                {
                    // If you miss, zap to max range in front
                    SpawnLightning(cam.position + transform.forward * guitarData.maxDistance);
                    strikePosition = cam.position + transform.forward * guitarData.maxDistance;
                    Debug.Log("Hit Somethin'");
                }

                SpawnLightning(strikePosition);

                StartCoroutine(ActivateElectricityEffect());

                PlayAllStrikeSounds();

                guitarData.currentEnergy--;
                timeSinceLastStrike = 0;

            }
        }
        else
        {
            if (canRanOutAnimation && !isGuitarAnimating)
            {
                canRanOutAnimation = false;
                isGuitarAnimating = true; // Set flag when animation starts

                guitar = GameObject.FindGameObjectWithTag("Guitar");
                if (guitar != null)
                {
                    guitarAnimator = guitar.GetComponent<Animator>();
                    if (guitarAnimator != null)
                    {
                        guitarAnimator.enabled = true;
                        guitarAnimator.Play("RanOut", -1, 0f);
                        Debug.Log("Guitar Animation Enabled and Playing - RanOut");

                        Invoke("EnableRanOutAnimation", animationDuration);
                        Invoke("DisableGuitarAnimator", animationDuration);
                        Invoke("ResetIsAnimating", animationDuration); // Reset flag after animation
                    }
                    else
                    {
                        // ... error handling ...
                        canRanOutAnimation = true;
                        isGuitarAnimating = false; // Reset flag on error
                    }
                }
                else
                {
                    // ... error handling ...
                    canRanOutAnimation = true;
                    isGuitarAnimating = false; // Reset flag on error
                }
            }
            else if (isGuitarAnimating)
            {
                Debug.Log("Guitar is currently animating, cannot play RanOut animation.");
            }
            else
            {
                Debug.Log("RanOut animation is on cooldown.");
            }
        }
    }

    private void EnableRanOutAnimation()
    {
        canRanOutAnimation = true;
        Debug.Log("RanOut animation is now available.");
    }



    private void Update()
    {
        timeSinceLastStrike += Time.deltaTime;

        Debug.DrawRay(cam.position, cam.forward);
    }

    private void OnGuitarStrike()
    {
        // Play sound based on the current strike count
        int soundIndex = strikeSoundPattern[strikeCount % strikeSoundPattern.Length];
        if (strikeSounds.Length > soundIndex && strikeSounds[soundIndex] != null)
        {
            audioSource.clip = strikeSounds[soundIndex];
            audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f); // Optional: spice it up
            audioSource.Play();
        }

        strikeCount++;
    }

    private void PlayAllStrikeSounds()
    {
        foreach (AudioClip clip in strikeSounds)
        {
            // Create a temp AudioSource to play each sound
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }

    private void SpawnLightning(Vector3 targetPoint)
    {
        GameObject zap = Instantiate(lightningPrefab, cam.position, Quaternion.identity);

        // Point the zap toward the target
        zap.transform.LookAt(targetPoint);

        // Stretch it to reach
        float dist = Vector3.Distance(cam.position, targetPoint);
        zap.transform.localScale = new Vector3(1f, 1f, dist);

        // Spawn spark at the target point
        if (sparkPrefab != null)
        {
            GameObject spark = Instantiate(sparkPrefab, targetPoint, Quaternion.identity);
            Destroy(spark, 1f); // Auto-destroy after 1 second
        }

        // Destroy zap after short time
        Destroy(zap, 0.3f);
    }


}
