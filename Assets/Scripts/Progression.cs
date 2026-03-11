using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Progression : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private UIToggle _uiToggle;

	[SerializeField] private List<Color> _selectedColors;
	[SerializeField] private PlayerCycling[] _rows;
	[SerializeField] private PlayerAnchors _anchors;
	private int _rowIndex = 0;

	[SerializeField] private Transform _finalCharacter;
	private Coroutine _construction;

	public Sequence SnapShake;
	public Transform FinalCharacter => _finalCharacter;
	public bool Finished;

	private void Start()
	{
		_rows[0].Register();
		_rows[0].Show(true);
		_rows[1].Preview();
		_selectedColors.Add(_rows[0].Current);
		RoundManager.Instance.ResumeGame();
	}

	public void RestartProgression()
	{
		_rowIndex = -1;
		_playerInput.SwitchCurrentActionMap("Rejoin");

		// Reset UI
		_uiToggle.DisableUI();
		PlayerAreas.GetArea(_playerInput).PressAnyButton.gameObject.SetActive(true);

		// Stop animations & hide rows
		if (_construction != null)
		{
			StopCoroutine(_construction);
		}
		foreach (var row in _rows)
		{
			row.StopAnimations();
			row.Show(false);
		}

		// Return character to starting position
		Rigidbody2D rb = _finalCharacter.GetComponent<Rigidbody2D>();
		_finalCharacter.SetPositionAndRotation(_anchors.Finished[1].position, Quaternion.identity);
		rb.bodyType = RigidbodyType2D.Kinematic;
		rb.linearVelocity = Vector2.zero;
		rb.angularVelocity = 0f;
		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.simulated = false;

		// Put things in their correct positions
		_rows[0].SetPositionsToInitialForTop();
		foreach (var row in _rows.Skip(1))
		{
			row.SetPreviewAtPosition();
		}

		// Stop previous parts of character from shaking
		foreach (var row in _rows)
		{
			row.DisconnectCurrentFromComplete();
		}

		// RESET COLORS
		_selectedColors.Clear();
		Finished = false;
	}

	public void GoBack()
	{
		_selectedColors.RemoveAt(_rowIndex);
		_rows[_rowIndex].Release();
		_rowIndex--;

		if (_rowIndex == -1)
		{
			PlayerAreas.GetArea(_playerInput).PressAnyButton.gameObject.SetActive(true);
			_rows[0].Show(false);
			_rows[1].ShowCurrent(false);
			_playerInput.SwitchCurrentActionMap("Rejoin");
			_uiToggle.DisableUI();
			return;
		}

		_rows[_rowIndex].ReturnToPosition();
		_rows[_rowIndex + 1].ReturnToPreview();
		_rows[_rowIndex + 1].Preview();
		if (_rowIndex != _rows.Length - 2)
		{
			_rows[_rowIndex + 2].HidePreview();
			_rows[_rowIndex + 2].StopPreview();
		}
	}

	public void Rejoin(InputAction.CallbackContext context)
	{
		if (!context.started)
		{
			return;
		}

		RoundManager.Instance.ResumeGame();
		_uiToggle.EnableUI();
		_rowIndex = 0;
		PlayerAreas.GetArea(_playerInput).PressAnyButton.gameObject.SetActive(false);
		_rows[0].Register();
		_rows[0].Show(true);
		_rows[1].ShowCurrent(true);
		_selectedColors.Add(_rows[0].Current);
		_playerInput.SwitchCurrentActionMap("Player Controls");
	}

	public void GoForward()
	{
		_rows[_rowIndex].SendToEnd();
		_rowIndex++;
		
		if (_rowIndex == _rows.Length)
		{
			Finished = true;
			_uiToggle.DisableUI();
			_playerInput.SwitchCurrentActionMap("Movement");
			_construction = StartCoroutine(ConstructCharacter());
			return;
		}

		_rows[_rowIndex].StopPreview();
		_rows[_rowIndex].Register();
		_rows[_rowIndex].SendToPosition();
		_selectedColors.Add(_rows[_rowIndex].Current);

		if (_rowIndex != _rows.Length - 1)
		{
			_rows[_rowIndex + 1].Preview();
			_rows[_rowIndex + 1].ShowPreview();
		}
	}

	public void Cycle(bool right)
	{
		PlayerCycling row = _rows[_rowIndex];
		row.Cycle(right);
		_selectedColors[_rowIndex] = row.Current;
	}

	public IEnumerator ConstructCharacter()
	{
		foreach (var row in _rows)
		{
			row.OptionRow.HidePlayer(row);
		}

		RoundManager rm = RoundManager.Instance;
		PlayerStats stats = PlayerAreas.GetArea(_playerInput);

		Color top = _selectedColors[0];
		Color middle = _selectedColors[1];
		Color bottom = _selectedColors[2];
		Color[] combo = rm.PrimeSpecimenColors;

		bool topIsBottom = top == bottom;

		if (topIsBottom && top == middle)
		{
			stats.Score += 6;
		}
		else if (topIsBottom)
		{
			stats.Score += 4;
		}
		else if (top == combo[0] && middle == combo[1] && bottom == combo[2])
		{
			stats.Score += 10;
		}

		stats.Score += rm.GetSpeedBonus();

		yield return _rows[_rowIndex - 1].ProgressAnimation.ToYieldInstruction();
		yield return SnapShake.ToYieldInstruction();

		Rigidbody2D rb = _finalCharacter.GetComponent<Rigidbody2D>();

		rb.simulated = true;

		float randomAngle = Random.Range(-30f, 30f);
		rb.angularVelocity = randomAngle;
	}
}
