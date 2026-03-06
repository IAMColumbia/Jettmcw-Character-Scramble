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

	[SerializeField] private Transform _finalCharacter;

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
		_rows[_rowIndex].Release();
		_rowIndex--;

		if (_rowIndex == -1)
		{
			Transform area = PlayerAreas.GetArea(_playerInput);
			area.GetChild(0).gameObject.SetActive(true);
			_playerInput.SwitchCurrentActionMap("Rejoin");
			_uiToggle.DisableUI();
			return;
		}

		_rows[_rowIndex].BeMovedBack();
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
		_rows[_rowIndex].BeMovedAway();
		_rowIndex++;
		
		if (_rowIndex == _rows.Length)
		{
			_uiToggle.DisableUI();
			_playerInput.SwitchCurrentActionMap("Movement");
			ConstructCharacter();
			return;
		}

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

	public void ConstructCharacter()
	{
		foreach (var row in _rows)
		{
			row.Selection.SetParent(_finalCharacter, true);
		}
		float randomAngle = Random.Range(-30f, 30f);
		_finalCharacter.gameObject.SetActive(true);
		_finalCharacter.GetComponent<Rigidbody2D>().angularVelocity = randomAngle;
	}
}
