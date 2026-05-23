using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioClip GetAudioClip;
    [SerializeField] private AudioClip kekkaAudioClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        BGMSource.Play();
        BGMSource.loop = true;
    }

    public void PlayGetAudio()
    {
        BGMSource.PlayOneShot(GetAudioClip);
    }

    public void PlayKekkaAudio()
    {
        BGMSource.PlayOneShot(kekkaAudioClip);
    }
}