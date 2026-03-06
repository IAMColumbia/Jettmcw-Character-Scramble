using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAreas : MonoBehaviour
{
    private static PlayerAreas s_instance;
	[SerializeField] private Transform[] _areas = new Transform[4];
	[SerializeField] private List<PlayerInput> _players = new(4);

	private void Awake()
	{
		s_instance = this;
	}

	public static Transform GetArea(PlayerInput input)
	{
		Transform[] areas = s_instance._areas;
		List<PlayerInput> players = s_instance._players;

		int idx = players.IndexOf(input);
		if (idx != -1)
		{
			return areas[idx];
		}

		idx = players.Count;
		players.Add(input);
		return areas[idx];
	}
}
