using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;

    [SerializeField] private Slider MusicVolume;

    public void Start()
    {
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }

    }
    public void SetMusicVolume()
    {
        float volume = MusicVolume.value;
        AudioMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void LoadVolume()
    {
        MusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
    }
}
