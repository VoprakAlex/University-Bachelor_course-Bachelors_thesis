using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AuidioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;

    public AudioClip BackgroundMusic;

    public static AuidioManager instance;

    private void Start()
    {
        MusicSource.clip = BackgroundMusic;
        MusicSource.Play();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);   
        }

    }
}
