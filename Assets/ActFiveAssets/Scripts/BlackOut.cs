using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackOut : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 7.0f;

    public void StartFade()
    {
        StartCoroutine(FadeOut());
        fadeImage.gameObject.SetActive(true);
    }

    public void StartFadeIn()
    {
        fadeImage.gameObject.SetActive(true); // Ensure image is active
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f); // Black with alpha 1.

        while (timer < fadeDuration)
        {
            // Interpolate the color between start and end over time.
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the image is completely black at the end.
        fadeImage.color = endColor;
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color startColor = fadeImage.color; // Current color (should be black)
        Color endColor = new Color(0f, 0f, 0f, 0f); // Black with alpha 0 (transparent)

        while (timer < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor; // Ensure fully transparent
        fadeImage.gameObject.SetActive(false); // Disable image to save performance
    }


}
