using System.Runtime.CompilerServices;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] GameObject activeWatcher;
    [SerializeField] private Transform cameraTf;

    [Header("Music Tracks")]
    public AudioClip howlingWind;
    public AudioClip victory;

    [Header("Sound Effects")]
    public AudioClip boxingRingBell;
    public AudioClip buff;
    public AudioClip card;
    public AudioClip damage;
    public AudioClip heal;
    public AudioClip noEffect;
    public AudioClip shuffle;
    public AudioClip proceed;
    
    private AudioSource musicAudioSource;
    private AudioSource oneShotAudioSource;
    private float currentAnim = 1;
    private float velocity;
    private bool muted = false;
    

    private void Awake()
    {
        Instance = this;

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;

        oneShotAudioSource = gameObject.AddComponent<AudioSource>();
        oneShotAudioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            muted = !muted;

        bool a = activeWatcher.activeSelf;

        float target = a ? 0: 1;

        currentAnim = Mathf.SmoothDamp(currentAnim, target, ref velocity, 0.25f);

        musicAudioSource.pitch = 0.5f + (currentAnim * 0.5f);
        musicAudioSource.volume = (0.5f + (currentAnim * 0.5f)) * 0.25f * (muted ? 0: 1);

        oneShotAudioSource.volume = muted ? 0 : 1;

        cameraTf.transform.position = Vector3.Lerp(new Vector3(0, 2.5f, 0), new Vector3(0, 1.25f, -0.3f), currentAnim);
        cameraTf.transform.localEulerAngles = Vector3.Lerp(new Vector3(90, 0, 0), new Vector3(80, 0, 0), currentAnim);
    }

    public void PlayHowlingWind()
    {
        musicAudioSource.clip = howlingWind;
        musicAudioSource.Play();
        musicAudioSource.loop = true;
    }

    public void PlayVictoryMusic()
    {
        musicAudioSource.clip = victory;
        musicAudioSource.Play();
        musicAudioSource.loop = false;
    }

    public void PlayMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
        musicAudioSource.loop = true;
    }

    public void StopPlayingMusic()
    {
        musicAudioSource.Stop();
    }

    public void PlayBoxingRingBell()
    {
        oneShotAudioSource.clip = boxingRingBell;
        oneShotAudioSource.Play();
    }

    public void PlayBuff()
    {
        oneShotAudioSource.clip = buff;
        oneShotAudioSource.Play();
    }

    public void PlayCard()
    {
        oneShotAudioSource.clip = card;
        oneShotAudioSource.Play();
    }

    public void PlayDamage()
    {
        oneShotAudioSource.clip = damage;
        oneShotAudioSource.Play();
    }

    public void PlayHeal()
    {
        oneShotAudioSource.clip = heal;
        oneShotAudioSource.Play();
    }

    public void PlayNoEffect()
    {
        oneShotAudioSource.clip = noEffect;
        oneShotAudioSource.Play();
    }

    public void PlayShuffle()
    {
        /*
        oneShotAudioSource.clip = shuffle;
        oneShotAudioSource.Play();
        */
        oneShotAudioSource.PlayOneShot(shuffle, 0.5f);
    }

    public void PlayProceed()
    {
        oneShotAudioSource.PlayOneShot(proceed);
    }
}
