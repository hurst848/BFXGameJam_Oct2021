using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer master;
    public AudioMixer music;
    public AudioMixer effects;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;
    float masterVolume;
    float musicVolume;
    float effectsVolume;


    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 0.75f);
        effectsSlider.value = PlayerPrefs.GetFloat("Effects", 0.75f);
    }
    public void MasterVolume(float mastervolume)
    {
        master.SetFloat("Master", Mathf.Log10(mastervolume) * 20);
        masterVolume = mastervolume;
    }

    public void MusicVolume(float musicvolume)
    {
        music.SetFloat("Music", Mathf.Log10(musicvolume) * 20);
        musicVolume = musicvolume;
    }

    public void EffectsVolume(float effectsvolume)
    {
        effects.SetFloat("Effects", Mathf.Log10(effectsvolume) * 20);
        effectsVolume = effectsvolume;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Master", masterVolume);
        PlayerPrefs.SetFloat("Music", musicVolume);
        PlayerPrefs.SetFloat("Effects", effectsVolume);
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        else
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");

        if (PlayerPrefs.HasKey("MusicVolume"))
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        else
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        if (PlayerPrefs.HasKey("EffectVolume"))
            effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
        else
            effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
    }


    public void SetFullscreen(bool isFullscreen)    // Toggle Fullscreen mode
    {
        Screen.fullScreen = isFullscreen;
    }
}
