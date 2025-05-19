using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro; // for TextMeshProUGUI

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer MainAudioMixer;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeLabel; // Assign this to the text label in Inspector

    void Start()
    {
        volumeSlider.onValueChanged.RemoveAllListeners();

        float volume = PlayerPrefs.GetFloat("Volume", 0.75f);
        volumeSlider.value = volume;
        UpdateVolumeUI(volume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        Debug.Log("SetVolume called with value: " + volume);

        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20f;
        MainAudioMixer.SetFloat("Volume", dB);
        PlayerPrefs.SetFloat("Volume", volume);
        UpdateVolumeUI(volume);
    }


    private void UpdateVolumeUI(float volume)
    {
        int percentage = Mathf.RoundToInt(volume * 100f);
        volumeLabel.text = percentage + "%";
    }
}
