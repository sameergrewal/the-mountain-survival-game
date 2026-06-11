using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource footstepSource;
    public AudioSource ambienceSource;
    public AudioSource fireSource;
    
    [Header("Ambience")]
    public AudioClip natureAmbience;
    public float ambienceVolume = 0.3f;
    
    [Header("Footstep Sounds")]
    public AudioClip grassFootsteps;
    public AudioClip rockFootsteps;
    public AudioClip snowFootsteps;
    public float footstepVolume = 0.4f;
    
    [Header("Effect Sounds")]
    public AudioClip jumpSound;
    public AudioClip victorySound;
    public AudioClip fireSound;
    
    [Header("Settings")]
    public float fireVolume = 0.3f;
    
    private static AudioManager instance;
    private AudioClip currentFootstepClip;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (ambienceSource != null && natureAmbience != null)
        {
            ambienceSource.clip = natureAmbience;
            ambienceSource.volume = ambienceVolume;
            ambienceSource.loop = true;
            ambienceSource.Play();
        }
    }
    
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
    
    public void PlayFootsteps(string terrainType)
    {
        AudioClip newClip = GetFootstepClip(terrainType);
        
        if (newClip != currentFootstepClip)
        {
            currentFootstepClip = newClip;
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
        
        if (footstepSource != null && currentFootstepClip != null && !footstepSource.isPlaying)
        {
            footstepSource.clip = currentFootstepClip;
            footstepSource.volume = footstepVolume;
            footstepSource.loop = true;
            footstepSource.Play();
        }
    }
    
    AudioClip GetFootstepClip(string terrainType)
    {
        switch (terrainType.ToLower())
        {
            case "grass":
                return grassFootsteps;
            case "rock":
            case "stone":
                return rockFootsteps;
            case "snow":
                return snowFootsteps;
            default:
                return grassFootsteps;
        }
    }
    
    public void StopFootsteps()
    {
        if (footstepSource != null && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
        currentFootstepClip = null;
    }
    
    public void PlayFireSound()
    {
        if (fireSource != null && fireSound != null && !fireSource.isPlaying)
        {
            fireSource.clip = fireSound;
            fireSource.volume = fireVolume;
            fireSource.loop = true;
            fireSource.Play();
        }
    }
    
    public void StopFireSound()
    {
        if (fireSource != null && fireSource.isPlaying)
        {
            fireSource.Stop();
        }
    }
    
    public static void PlayJumpSound()
    {
        if (instance != null && instance.jumpSound != null)
        {
            instance.PlaySound(instance.jumpSound, 0.6f);
        }
    }
    
    public static void PlayVictorySound()
    {
        if (instance != null && instance.victorySound != null)
        {
            instance.PlaySound(instance.victorySound, 0.8f);
        }
    }
}
