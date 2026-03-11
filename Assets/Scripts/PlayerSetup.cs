using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
	[SerializeField] private PlayerInput _player;

	private void Awake()
	{
		PlayerStats area = PlayerAreas.GetArea(_player);
		transform.SetParent(area.transform, true);
		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3(190, 190, 190);
		area.PressAnyButton.gameObject.SetActive(false); // Remove "Press any button" text
		area.GetComponent<PlayerStats>().Activate();
	}
}
