using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();
    private int currentResolutionIndex = 0;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        // Filter unique resolutions and populate dropdown
        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution res = resolutions[i];
            string option = res.width + " x " + res.height;

            if (!options.Contains(option))  // avoid duplicates
            {
                options.Add(option);
                filteredResolutions.Add(res);

                if (res.width == Screen.currentResolution.width &&
                    res.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = options.Count - 1;
                }
            }
        }

        // Add label at top (fake entry)
        options.Insert(0, "Resolution");
        filteredResolutions.Insert(0, Screen.currentResolution); // dummy

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = 0; // Keep "Resolution" as label
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    void SetResolution(int index)
    {
        if (index == 0)
            return; // ignore label row

        Resolution res = filteredResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
