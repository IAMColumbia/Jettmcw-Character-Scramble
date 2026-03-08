using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAreas : MonoBehaviour
{
    private static PlayerAreas s_instance;
	[SerializeField] private Transform[] _areas = new Transform[4];

	private void Awake()
	{
		s_instance = this;
	}

	public static Transform GetArea(PlayerInput input)
	{
		Transform[] areas = s_instance._areas;
		int idx = PlayerControllerManager.Instance.GetPlayerNumber(input);
		return areas[idx];
	}
}
