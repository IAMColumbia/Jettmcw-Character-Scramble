using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerManager : MonoBehaviour
{
	public static PlayerControllerManager Instance { get; private set; }

    public List<PlayerInput> Players;

	private void Awake()
	{
		Instance = this;
	}

	public void OnPlayerAdded(PlayerInput player)
    {
        Players.Add(player);
		
		player.DeactivateInput();
		StartCoroutine(EnableNextFrame());

		IEnumerator EnableNextFrame()
		{
			yield return null;
			player.ActivateInput();
		}
	}

	public int GetPlayerNumber(PlayerInput player) => Players.IndexOf(player);
}
