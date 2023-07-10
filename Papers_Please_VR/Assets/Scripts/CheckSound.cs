using Unity.Template.VR;
using UnityEngine;

/// <summary>
/// Play a sound depend on the check status
/// </summary>
public class CheckSound : MonoBehaviour
{
    #region Variables
    
    [Tooltip("The sound that is played when correct")]
    public AudioClip correctSound;
    
    [Tooltip("The sound that is played when false")]
    public AudioClip falseSound;

    [Tooltip("The volume of the sound")]
    public float volume = 1.0f;
    
    [Tooltip("The range of pitch the sound is played at (-pitch, pitch)")]
    [Range(0, 1)] public float randomPitchVariance;

    private AudioSource _audioSource ;
    
    private const float DefaultPitch = 1.0f;

    #endregion

    #region Functions

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameEvents.current.onVisaCheckSound += Play;
    }

    private void Play(CheckStatus checkStatus)
    {
        float randomVariance = Random.Range(-randomPitchVariance, randomPitchVariance);
        randomVariance += DefaultPitch;

        _audioSource.pitch = randomVariance;
        switch (checkStatus)
        {
            case CheckStatus.Correct: 
                _audioSource.PlayOneShot(correctSound, volume);
                break;
            case CheckStatus.Wrong:
                _audioSource.PlayOneShot(falseSound, volume);
                break;
            default:
                print("Should not have happened!");
                break;
        }
        _audioSource.pitch = DefaultPitch;
    }

    #endregion

}
