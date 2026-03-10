using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
	[SerializeField] private PlayerInput _player;

	private void Awake()
	{
		Transform area = PlayerAreas.GetArea(_player);
		transform.SetParent(area, true);
		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3(200, 200, 200);
		area.GetChild(0).gameObject.SetActive(false); // Remove "Press any button" text
	}
}
