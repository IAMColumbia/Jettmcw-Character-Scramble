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
			_rowIndex--;
			Transform area = PlayerAreas.GetArea(_playerInput);
			area.GetChild(0).gameObject.SetActive(true);
			_rows[0].Release();
			return;
		}
	}
}
