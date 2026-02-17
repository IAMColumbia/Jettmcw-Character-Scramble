using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerManager : MonoBehaviour
{
    public static PlayerControllerManager Instance;
    public Transform Canvas;
    public List<PlayerInput> Players;

	private void Awake()
	{
        Instance = this;
	}

	public void OnPlayerAdded(PlayerInput player)
    {
        Players.Add(player);
        Debug.Log(player.user.controlScheme);
    }
}
