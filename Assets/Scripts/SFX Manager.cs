using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public static SFXManager Instance { get; private set; }

	[SerializeField] private AudioSource _audioSource;
	public AudioClip incorrect;
	public AudioClip timerLow;

	private void Awake()
	{
		Instance = this;
	}

	public static void PlayIncorrectSound() => Instance._audioSource.PlayOneShot(Instance.incorrect);
	public static void PlayTimerLow(float intensity) => Instance._audioSource.PlayOneShot(Instance.timerLow, intensity);
}
