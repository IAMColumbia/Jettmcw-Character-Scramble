using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public static SFXManager Instance { get; private set; }

	[SerializeField] private AudioSource _audioSource;
	public AudioClip incorrect;
	public AudioClip timerLow;
	public AudioClip move;
	public AudioClip up;

	private void Awake()
	{
		Instance = this;
	}

	public static void PlayIncorrectSound() => Instance._audioSource.PlayOneShot(Instance.incorrect);
	public static void PlayTimerLow(float intensity) => Instance._audioSource.PlayOneShot(Instance.timerLow, intensity);
	public static void PlayMove(float intensity) => Instance._audioSource.PlayOneShot(Instance.move, intensity);
	public static void PlayUp(float intensity) => Instance._audioSource.PlayOneShot(Instance.up, intensity);
}
