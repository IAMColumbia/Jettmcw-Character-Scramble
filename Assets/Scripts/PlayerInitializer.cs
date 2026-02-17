using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
	public void Awake()
	{
		PlayerControllerManager pcm = PlayerControllerManager.Instance;
		transform.SetParent(pcm.Canvas.GetChild(pcm.Players.Count - 1));
		transform.localScale = Vector3.one * 100;
		transform.localPosition = Vector3.zero;
	}
}
