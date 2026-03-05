using System;
using System.Collections.Generic;
using UnityEngine;

public class CyclicalOptions : MonoBehaviour
{
	public static CyclicalOptions Instance { get; private set; }

	[SerializeField] private Color[] _options;
	private PlayerCycling[] _playerPositions;

	private void Awake()
	{
		Instance = this;
		ShuffleOptions();
		_playerPositions = new PlayerCycling[_options.Length];
	}

	public void ShuffleOptions() => Utility.Shuffle(_options);

	public void Register(PlayerCycling player)
	{
		// Insert player at first open index from the left
		int insertionIndex = 0;
		while (_playerPositions[insertionIndex] != null)
		{
			insertionIndex++;
		}
		player.Current = _options[insertionIndex];
		_playerPositions[insertionIndex] = player;

		// Step 1: Find R-Open
		var iterator = WrapRight(insertionIndex);
		iterator.MoveNext();
		while (iterator.MoveNext() && _playerPositions[iterator.Current] != null);
		int rightIndex = iterator.Current;
		iterator.Dispose();

		// Step 2: (L-Open, start] -> R-Open
		iterator = WrapLeft(insertionIndex);
		while (iterator.MoveNext())
		{
			PlayerCycling check = _playerPositions[iterator.Current];
			if (check == null)
			{
				break;
			}
			check.Right = _options[rightIndex];
		}
		int leftIndex = iterator.Current;
		iterator.Dispose();

		// Step 3: L-Open <- [Start, R-Open)
		iterator = WrapRight(insertionIndex);
		while (iterator.MoveNext())
		{
			PlayerCycling check = _playerPositions[iterator.Current];
			if (check == null)
			{
				break;
			}
			check.Left = _options[leftIndex];
		}
		iterator.Dispose();
	}

	public void CycleRight(PlayerCycling player)
	{
		// Mark the player's previous spot as empty
		int start = Array.IndexOf(_playerPositions, player);
		_playerPositions[start] = null;

		// Mark the player's new spot as full
		player.Current = player.Right;
		int end = Array.IndexOf(_options, player.Right);
		_playerPositions[end] = player;

		// Step 1: (open, start) -> start
		var iterator = WrapLeft(start);
		iterator.MoveNext();
		while (iterator.MoveNext())
		{
			PlayerCycling check = _playerPositions[iterator.Current];
			if (check == null)
			{
				break;
			}
			check.Right = _options[start];
		}
		iterator.Dispose();

		// Step 2: start <- (start, open)
		iterator = WrapRight(start);
		iterator.MoveNext();
		while (iterator.MoveNext())
		{
			PlayerCycling check = _playerPositions[iterator.Current];
			if (check == null)
			{
				break;
			}
			check.Left = _options[start];
		}
		int openToRight = iterator.Current;
		iterator.Dispose();

		// Step 3: (start, end] -> open
		iterator = WrapLeft(end);
		while (iterator.MoveNext() && iterator.Current != start)
		{
			PlayerCycling check = _playerPositions[iterator.Current];
			check.Right = _options[openToRight];
		}
		iterator.Dispose();
	}
	public void CycleLeft(PlayerCycling player)
	{
		// Mark the player's previous spot as empty
		int start = Array.IndexOf(_playerPositions, player);
		_playerPositions[start] = null;

		// Mark the player's new spot as full
		player.Current = player.Left;
		int end = Array.IndexOf(_options, player.Left);
		_playerPositions[end] = player;

		// Step 1: start <- (start, open)
		var iterator = WrapRight(start);
		iterator.MoveNext();
		while (iterator.MoveNext())
		{
			PlayerCycling check = _playerPositions[iterator.Current];
			if (check == null)
			{
				break;
			}
			check.Left = _options[start];
		}
		iterator.Dispose();

		// Step 2: (open, start) -> start
		iterator = WrapLeft(start);
		iterator.MoveNext();
		while (iterator.MoveNext())
		{
			PlayerCycling check = _playerPositions[iterator.Current];
			if (check == null)
			{
				break;
			}
			check.Right = _options[start];
		}
		int openToLeft = iterator.Current;
		iterator.Dispose();

		// Step 3: (start, end] -> open | open <- [end, start)
		iterator = WrapRight(end);
		while (iterator.MoveNext() && iterator.Current != start)
		{
			PlayerCycling check = _playerPositions[iterator.Current];
			check.Left = _options[openToLeft];
		}
		iterator.Dispose();
	}

	private IEnumerator<int> WrapLeft(int start)
	{
		for (int i = start; i >= 0; i--)
		{
			yield return i;
		}
		for (int i = _options.Length - 1; i > start; i--)
		{
			yield return i;
		}
	}

	private IEnumerator<int> WrapRight(int start)
	{
		for (int i = start; i < _options.Length; i++)
		{
			yield return i;
		}
		for (int i = 0; i < start; i++)
		{
			yield return i;
		}
	}
}
