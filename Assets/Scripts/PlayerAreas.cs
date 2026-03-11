using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAreas : MonoBehaviour
{
    private static PlayerAreas s_instance;
	[SerializeField] private PlayerStats[] _areas = new PlayerStats[4];

	private void Awake()
	{
		s_instance = this;
	}

	public static PlayerStats GetArea(PlayerInput input)
	{
		PlayerStats[] areas = s_instance._areas;
		int idx = PlayerControllerManager.Instance.GetPlayerNumber(input);
		return areas[idx];
	}

	public static PlayerStats GetStats(int idx) => s_instance._areas[idx];
}
