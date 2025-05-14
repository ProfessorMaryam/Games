using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light myLight;
    public AudioSource buzzSound;

    private float interval;
    private float timer = 0f;

    // Sway settings
    public float swayAngle = 3f;       // Max angle to sway to either side
    public float swaySpeed = 1f;       // Speed of the sway
    private Quaternion initialRotation;

    void Start()
    {
        interval = Random.Range(0.02f, 0.2f);
        initialRotation = transform.localRotation;

        if (buzzSound != null)
        {
            buzzSound.loop = true;
            buzzSound.Play(); // Start buzzing sound
        }
    }

    void Update()
    {
        if (myLight == null) return;

        // Flicker logic
        timer += Time.deltaTime;

        if (timer > interval)
        {
            bool flickerState = !myLight.enabled;
            myLight.enabled = flickerState;

            if (buzzSound != null)
            {
                if (flickerState && !buzzSound.isPlaying)
                    buzzSound.Play();
                else if (!flickerState)
                    buzzSound.Pause();
            }

            interval = Random.Range(0.02f, 0.2f);
            timer = 0f;
        }

        // Swaying logic
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAngle;
        transform.localRotation = initialRotation * Quaternion.Euler(sway, 0f, 0f);
    }
}
