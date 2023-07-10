using UnityEngine;

/// <summary>
/// Play a simple sounds using Play one shot with volume, and pitch
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayQuickSound : MonoBehaviour
{
    [Tooltip("The sound that is played")]
    public AudioClip sound;

    [Tooltip("The volume of the sound")]
    public float volume = 1.0f;

    [Tooltip("The range of pitch the sound is played at (-pitch, pitch)")]
    [Range(0, 1)] public float randomPitchVariance;

    private AudioSource _audioSource;

    private const float DefaultPitch = 1.0f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        float randomVariance = Random.Range(-randomPitchVariance, randomPitchVariance);
        randomVariance += DefaultPitch;

        _audioSource.pitch = randomVariance;
        _audioSource.PlayOneShot(sound, volume);
        _audioSource.pitch = DefaultPitch;
    }

    private void OnValidate()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
}
