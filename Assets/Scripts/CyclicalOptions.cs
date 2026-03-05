using System;
using System.Collections.Generic;
using UnityEngine;

public class CyclicalOptions : MonoBehaviour
{
	public static CyclicalOptions Instance { get; private set; }

	[SerializeField] private Color[] _options;
	[SerializeField] private PlayerCycling[] _playerPositions;
	private int _freeSlots;
	public int FirstFreeIndex;
	private Color _target;

	private void Awake()
	{
		Instance = this;
		Utility.Shuffle(_options);
		_freeSlots = _options.Length;
		FirstFreeIndex = 0;
		_playerPositions = new PlayerCycling[_freeSlots];
	}

	public void Register(PlayerCycling player)
	{
		// Insert player at first open index from the left
		int insertionIndex = FirstFreeIndex;
		do
		{
			FirstFreeIndex++;
		}
		while (_playerPositions[FirstFreeIndex] != null);

		player.Current = _options[insertionIndex];
		_playerPositions[insertionIndex] = player;

		// Count remaining free slots
		_freeSlots--;

		// Case where there's only one loose option
		if (_freeSlots == 1)
		{
			int emptyCheck = insertionIndex + 1;
			while (_playerPositions[emptyCheck] != null)
			{
				emptyCheck++;
			}

			DirectAllTo(_options[emptyCheck]);
			return;
		}

		// Step 1: Find R-Open
		int rightIndex = CycleWhile(WrapRight, i => _playerPositions[i] != null, insertionIndex, true);
		_target = _options[rightIndex];

		// Step 2: (L-Open, start] -> R-Open
		int leftIndex = CycleWhile(WrapLeft, SetRight, insertionIndex, false);
		_target = _options[leftIndex];

		// Step 3: L-Open <- [Start, R-Open)
		CycleWhile(WrapRight, SetLeft, insertionIndex, false);
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

		if (start < FirstFreeIndex)
		{
			FirstFreeIndex = start;
		}
		else if (FirstFreeIndex == end)
		{
			do
			{
				FirstFreeIndex++;
			}
			while (_playerPositions[FirstFreeIndex] != null);
		}

		// Case where there's only one loose option
		if (_freeSlots == 1)
		{
			DirectAllTo(_options[start]);
			return;
		}

		_target = _options[start];

		// Step 1: (open, start) -> start
		CycleWhile(WrapLeft, SetRight, start, true);

		// Step 2: start <- (start, open)
		int openToRight = CycleWhile(WrapRight, SetLeft, start, true);

		// Step 3: (start, end] -> open
		CycleWhile(WrapLeft, SetRightUnlessStart, end, false);

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

		if (start < FirstFreeIndex)
		{
			FirstFreeIndex = start;
		}
		else if (FirstFreeIndex == end)
		{
			do
			{
				FirstFreeIndex++;
			}
			while (_playerPositions[FirstFreeIndex] != null);
		}

		// Case where there's only one loose option
		if (_freeSlots == 1)
		{
			DirectAllTo(_options[start]);
			return;
		}

		_target = _options[start];

		// Step 1: start <- (start, open)
		CycleWhile(WrapRight, SetLeft, start, true);

		// Step 2: (open, start) -> start
		int openToLeft = CycleWhile(WrapLeft, SetRight, start, true);

		// Step 3: (start, end] -> open | open <- [end, start)
		CycleWhile(WrapRight, SetLeftUnlessStart, end, false);

		bool SetLeftUnlessStart(int i)
		{
			if (i == start) return false;
			_playerPositions[i].Left = _options[openToLeft];
			return true;
		}
	}

	private void DirectAllTo(Color target)
	{
		foreach (PlayerCycling p in _playerPositions)
		{
			if (p == null)
			{
				continue;
			}
			p.Left = target;
			p.Right = target;
		}
	}

	private bool SetLeft(int i)
	{
		PlayerCycling check = _playerPositions[i];
		if (check == null) return false;
		check.Left = _target;
		return true;
	}

	private bool SetRight(int i)
	{
		PlayerCycling check = _playerPositions[i];
		if (check == null) return false;
		check.Right = _target;
		return true;
	}

	private int CycleWhile(Func<int, IEnumerator<int>> wrapFunction, Predicate<int> condition, int start, bool excludeStart)
	{
		using IEnumerator<int> iterator = wrapFunction(start);
		if (excludeStart) iterator.MoveNext();
		while (iterator.MoveNext() && condition(iterator.Current)) ;
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