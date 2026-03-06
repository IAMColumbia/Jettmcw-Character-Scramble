using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionRow : MonoBehaviour
{
	[SerializeField] private Color[] _options;
	[SerializeField] private PlayerCycling[] _playerPositions;
	private int _freeSlots;

	public int FirstFreeIndex;
	public bool IsFull;

	private Color _openedColor;
	private PlayerCycling _processedPlayer;

	private void Awake()
	{
		Utility.Shuffle(_options);
		_freeSlots = _options.Length;
		FirstFreeIndex = 0;
		IsFull = false;
		_playerPositions = new PlayerCycling[_freeSlots];
	}

	public void Register(PlayerCycling player)
	{
		// Insert player at first open index from the left
		int insertionIndex = FirstFreeIndex;
		player.Current = _options[insertionIndex];
		_playerPositions[insertionIndex] = player;

		// Track remaining free slots
		_freeSlots--;
		if (_freeSlots == 0)
		{
			IsFull = true;
			return;
		}

		// Recalculate the next open index
		do
		{
			FirstFreeIndex++;
		}
		while (IsIndexOccupied(FirstFreeIndex));

		// Case where there's only one loose option
		if (_freeSlots == 1)
		{
			DirectAllTo(_options[FirstFreeIndex]);
			return;
		}

		_openedColor = _options[FirstFreeIndex];
		_openedColor = IterateOverNeighbors(LeftNeighbors(insertionIndex), SetRight, 0);
		IterateOverNeighbors(RightNeighbors(insertionIndex), SetLeft, 0);
	}

	public void Cycle(PlayerCycling player, bool right)
	{
		// Empty the player's original space
		int start = Array.IndexOf(_playerPositions, player);
		_playerPositions[start] = null;
		_openedColor = player.Current;

		// Set the player's new color & position
		Color newColor = right ? player.Right : player.Left;
		player.Current = newColor;
		int end = Array.IndexOf(_options, newColor);
		_playerPositions[end] = player;

		// Determine the new first open index
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
			while (IsIndexOccupied(FirstFreeIndex));
		}

		// If there's only one open spot, redirect all players accordingly
		if (_freeSlots == 1)
		{
			DirectAllTo(_openedColor);
			return;
		}

		// Tell Self + previous neighbors to point to the newly-opened space
		Color leftOpenColor = IterateOverNeighbors(LeftNeighbors(start), SetRight, 1);
		Color rightOpenColor = IterateOverNeighbors(RightNeighbors(start), SetLeft, 1);

		// Tell Self + jumped-over neighbors to find new space in direction of movement
		_openedColor = right ? rightOpenColor : leftOpenColor;
		Func<int, IEnumerable<int>> directionToPassedNeighbors = right ? LeftNeighbors : RightNeighbors;
		IEnumerable<int> passedNeighbors = directionToPassedNeighbors(end).TakeWhile(i => i != start);
		Action setOpening = right ? SetRight : SetLeft;
		foreach (int i in passedNeighbors)
		{
			_processedPlayer = _playerPositions[i];
			setOpening();
		}
	}

	public void Remove(PlayerCycling player)
	{
		int removalIndex = Array.IndexOf(_playerPositions, player);
		_playerPositions[removalIndex] = null;
		_openedColor = _options[removalIndex];

		if (removalIndex < FirstFreeIndex)
		{
			FirstFreeIndex = removalIndex;
		}

		_freeSlots++;
		if (_freeSlots == 1)
		{
			IsFull = false;
			DirectAllTo(_openedColor);
			return;
		}

		IterateOverNeighbors(LeftNeighbors(removalIndex), SetRight, 1);
		IterateOverNeighbors(RightNeighbors(removalIndex), SetLeft, 1);
	}

	private bool IsIndexOccupied(int index) => _processedPlayer = _playerPositions[index];

	private void DirectAllTo(Color target)
	{
		foreach (PlayerCycling player in _playerPositions)
		{
			if (player)
			{
				player.Left = target;
				player.Right = target;
			}
		}
	}

	private void SetLeft()
	{
		_processedPlayer.Left = _openedColor;
	}

	private void SetRight()
	{
		_processedPlayer.Right = _openedColor;
	}

	private Color IterateOverNeighbors(IEnumerable<int> enumerable, Action action, int skip)
	{
		foreach (int i in enumerable.Skip(skip))
		{
			if (!IsIndexOccupied(i))
			{
				return _options[i];
			}
			action();
		}
		throw new InvalidOperationException();
	}

	private IEnumerable<int> LeftNeighbors(int start) => RightNeighbors(start + 1).Reverse();
	private IEnumerable<int> RightNeighbors(int start) => Enumerable.Range(start, _options.Length - start).Concat(Enumerable.Range(0, start));
}