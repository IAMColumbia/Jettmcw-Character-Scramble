using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Progression : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private PlayerCycling[] _rows;
	[SerializeField] private int _rowIndex = 0;

	private void Awake()
	{
		_rows[0].Register();
	}

	public void GoBack()
	{
		if (_rowIndex == 0)
		{
			Debug.Log("Back!");
			_rowIndex = -1;
			Transform area = PlayerAreas.GetArea(_playerInput);
			area.GetChild(0).gameObject.SetActive(true);
			_rows[0].Release();
			_playerInput.SwitchCurrentActionMap("Rejoin");
			return;
		}
	}

	public void Rejoin(InputAction.CallbackContext context)
	{
		if (!context.started)
		{
			return;
		}

		_rowIndex = 0;
		Transform area = PlayerAreas.GetArea(_playerInput);
		area.GetChild(0).gameObject.SetActive(false);
		_rows[0].Register();
		_playerInput.SwitchCurrentActionMap("Player Controls");
	}

	public void GoForward()
	{
		if (_rowIndex == -1)
		{
			return;
		}

		Debug.Log("No!");
	}
}
