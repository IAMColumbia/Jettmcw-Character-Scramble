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
		int rightIndex = CycleWhile(WrapRight, i => _playerPositions[i] != null, insertionIndex, true);

		// Step 2: (L-Open, start] -> R-Open
		int leftIndex = CycleWhile(WrapLeft, SetRight, insertionIndex, false);

		// Step 3: L-Open <- [Start, R-Open)
		CycleWhile(WrapRight, SetLeft, insertionIndex, false);

		bool SetLeft(int i)
		{
			PlayerCycling check = _playerPositions[i];
			if (check == null) return false;
			check.Left = _options[leftIndex];
			return true;
		}

		bool SetRight(int i)
		{
			PlayerCycling check = _playerPositions[i];
			if (check == null) return false;
			check.Right = _options[rightIndex];
			return true;
		}
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
		CycleWhile(WrapLeft, SetRight, start, true);

		// Step 2: start <- (start, open)
		int openToRight = CycleWhile(WrapRight, SetLeft, start, true);

		// Step 3: (start, end] -> open
		CycleWhile(WrapLeft, SetRightUnlessStart, end, false);

		bool SetLeft(int i)
		{
			PlayerCycling check = _playerPositions[i];
			if (check == null) return false;
			check.Left = _options[start];
			return true;
		}

		bool SetRight(int i)
		{
			PlayerCycling check = _playerPositions[i];
			if (check == null) return false;
			check.Right = _options[start];
			return true;
		}

		bool SetRightUnlessStart(int i)
		{
			if (i == start) return false;
			_playerPositions[i].Right = _options[openToRight];
			return true;
		}
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
		CycleWhile(WrapRight, SetLeft, start, true);

		// Step 2: (open, start) -> start
		int openToLeft = CycleWhile(WrapLeft, SetRight, start, true);

		// Step 3: (start, end] -> open | open <- [end, start)
		CycleWhile(WrapRight, SetLeftUnlessStart, end, false);

		bool SetLeft(int i)
		{
			PlayerCycling check = _playerPositions[i];
			if (check == null) return false;
			check.Left = _options[start];
			return true;
		}

		bool SetRight(int i)
		{
			PlayerCycling check = _playerPositions[i];
			if (check == null) return false;
			check.Right = _options[start];
			return true;
		}

		bool SetLeftUnlessStart(int i)
		{
			if (i == start) return false;
			_playerPositions[i].Left = _options[openToLeft];
			return true;
		}
	}

	private int CycleWhile(Func<int, IEnumerator<int>> wrapFunction, Predicate<int> condition, int start, bool excludeStart)
	{
		using IEnumerator<int> iterator = wrapFunction(start);
		if (excludeStart) iterator.MoveNext();
		while (iterator.MoveNext() && condition(iterator.Current));
		return iterator.Current;
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
