using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Progression : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;

	[SerializeField] private List<Color> _selectedColors;
	[SerializeField] private PlayerCycling[] _rows;
	[SerializeField] private int _rowIndex = 0;

	private void Awake()
	{
		_rows[0].Register();
		_selectedColors.Add(_rows[0].Current);
	}

	public void GoBack()
	{
		_selectedColors.RemoveAt(_rowIndex);
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
		_selectedColors.Add(_rows[0].Current);
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

	public void Cycle(bool right)
	{
		PlayerCycling row = _rows[_rowIndex];
		row.Cycle(right);
		_selectedColors[_rowIndex] = row.Current;
	}
}
