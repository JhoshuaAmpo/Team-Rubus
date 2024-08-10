using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public enum AudioType {music, sfx}

    [SerializeField]
    private List<AudioSource> musicSources;
    [SerializeField]
    private List<AudioSource> sfxSources;

    [Header("Settings Menu")]
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private TextMeshProUGUI musicPercentageText;
    [SerializeField]
    private TextMeshProUGUI sfxPercentageText;

    private void Awake() {
        foreach (var source in musicSources)
        {
            source.volume = GetMusicVolume();
        }
        foreach (var source in sfxSources)
        {
            source.volume = GetSFXVolume();
        }

        if (musicSlider != null) {
            musicSlider.value = GetMusicVolume();
        }
        if (sfxSlider != null) {
            sfxSlider.value = GetSFXVolume();
        }
        if (musicPercentageText != null) {

        }
    }

    public void SetMusicVolume(float vol) {
        SetVolume(AudioType.music,vol);
    }

    public void SetSFXVolume(float vol) {
        SetVolume(AudioType.sfx,vol);
    }

    public float GetMusicVolume() {
        return GetVolume(AudioType.music);
    }

    public float GetSFXVolume() {
        return GetVolume(AudioType.sfx);
    }

    public string FloatToPercent(float f) {
        return Mathf.RoundToInt(f * 100) + "%";
    }
    
    private void SetVolume(AudioType type, float val) {
        Mathf.Clamp(val, 0, 1);
        PlayerPrefs.SetFloat(type.ToString(), val);
    }

    private float GetVolume(AudioType type) {
        return PlayerPrefs.GetFloat(type.ToString());
    }
}
