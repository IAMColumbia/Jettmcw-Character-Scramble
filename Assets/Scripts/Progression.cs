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
			StartCoroutine(AllowRejoin());
			return;
		}
	}

	private IEnumerator AllowRejoin()
	{
		yield return null;
		_playerInput.actionEvents[3].AddListener(TryRejoin);
	}

	public void TryRejoin(InputAction.CallbackContext rejoin)
	{
		Debug.Log("Rejoin!");
		_playerInput.actionEvents[3].RemoveListener(TryRejoin);
		_rowIndex = 0;
		Transform area = PlayerAreas.GetArea(_playerInput);
		area.GetChild(0).gameObject.SetActive(false);
		_rows[0].Register();
		return;
	}

	public void GoForward()
	{
		Debug.Log("No!");
	}
}
