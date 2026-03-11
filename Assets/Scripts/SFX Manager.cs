using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public static SFXManager Instance { get; private set; }

	[SerializeField] private AudioSource _audioSource;
	public AudioClip incorrect;

	private void Awake()
	{
		Instance = this;
	}

	public static void PlayIncorrectSound() => Instance._audioSource.PlayOneShot(Instance.incorrect);
}
