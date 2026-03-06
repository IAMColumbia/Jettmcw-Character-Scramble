using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Progression : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private UIToggle _uiToggle;

	[SerializeField] private List<Color> _selectedColors;
	[SerializeField] private PlayerCycling[] _rows;
	[SerializeField] private int _rowIndex = 0;

	private void Awake()
	{
		_rows[0].Register();
		_rows[0].Show(true);
		_selectedColors.Add(_rows[0].Current);
	}

	public void GoBack()
	{
		_selectedColors.RemoveAt(_rowIndex);
		_rows[_rowIndex].Show(false);
		if (_rowIndex == 0)
		{
			Debug.Log("Back!");
			_rowIndex = -1;
			Transform area = PlayerAreas.GetArea(_playerInput);
			area.GetChild(0).gameObject.SetActive(true);
			_rows[0].Release();
			_playerInput.SwitchCurrentActionMap("Rejoin");
			_uiToggle.DisableUI();
			return;
		}
	}

	public void Rejoin(InputAction.CallbackContext context)
	{
		if (!context.started)
		{
			return;
		}

		_uiToggle.EnableUI();
		_rowIndex = 0;
		Transform area = PlayerAreas.GetArea(_playerInput);
		area.GetChild(0).gameObject.SetActive(false);
		_rows[0].Register();
		_rows[0].Show(true);
		_selectedColors.Add(_rows[0].Current);
		_playerInput.SwitchCurrentActionMap("Player Controls");
	}

	public void GoForward()
	{
		_rows[_rowIndex].Show(false);
		_rowIndex++;
		_rows[_rowIndex].Register();
		_rows[_rowIndex].Show(true);
		_selectedColors.Add(_rows[_rowIndex].Current);
	}

	public void Cycle(bool right)
	{
		PlayerCycling row = _rows[_rowIndex];
		row.Cycle(right);
		_selectedColors[_rowIndex] = row.Current;
	}
}
