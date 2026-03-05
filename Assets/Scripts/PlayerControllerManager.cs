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

		player.transform.SetParent(Canvas.GetChild(Players.Count - 1));
		player.transform.localScale = Vector3.one * 250;
		player.transform.localPosition = Vector3.zero;
	}
}
