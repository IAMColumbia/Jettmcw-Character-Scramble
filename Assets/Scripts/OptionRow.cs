using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionRow : MonoBehaviour
{
	[SerializeField] private Color[] _options;
	private PlayerCycling[] _playerPositions;
	private readonly List<PlayerCycling> _watchers = new();

	public int FirstFreeIndex = 0;
	public bool IsFull = false;

	private int _freeSlots;

	private Color _directionTarget;
	private PlayerCycling _playerToDirect;

	private void Awake()
	{
		Utility.Shuffle(_options);
		_freeSlots = _options.Length;
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
			_directionTarget = Color.clear;
			DirectAll();
			return;
		}

		// Recalculate the next open index
		FindOpenIndex();
		_directionTarget = _options[FirstFreeIndex];

		// Case where there's only one loose option
		if (_freeSlots == 1)
		{
			DirectAll();
			return;
		}

		_directionTarget = IterateOverNeighbors(LeftNeighbors(insertionIndex), DirectRight, 0);
		IterateOverNeighbors(RightNeighbors(insertionIndex), DirectLeft, 0);
	}

	public void Cycle(PlayerCycling player, bool right)
	{
		// Empty the player's original space
		int start = ClearPlayerSpace(player);

		// Set the player's new color & position
		Color newColor = right ? player.Right : player.Left;
		player.Current = newColor;
		int end = Array.IndexOf(_options, newColor);
		_playerPositions[end] = player;

		// Determine the new first open index
		if (start < FirstFreeIndex)
		{
			FirstFreeIndex = start;
			NotifyPreviews();
		}
		else if (FirstFreeIndex == end)
		{
			FindOpenIndex();
		}

		// If there's only one open spot, redirect all players accordingly
		if (_freeSlots == 1)
		{
			DirectAll();
			return;
		}

		// Tell Self + previous neighbors to point to the newly-opened space
		Color leftOpenColor = IterateOverNeighbors(LeftNeighbors(start), DirectRight, 1);
		Color rightOpenColor = IterateOverNeighbors(RightNeighbors(start), DirectLeft, 1);

		// Tell Self + jumped-over neighbors to find new space in direction of movement
		_directionTarget = right ? rightOpenColor : leftOpenColor;
		Func<int, IEnumerable<int>> directionToPassedNeighbors = right ? LeftNeighbors : RightNeighbors;
		IEnumerable<int> passedNeighbors = directionToPassedNeighbors(end).TakeWhile(i => i != start);
		Action setOpening = right ? DirectRight : DirectLeft;
		foreach (int i in passedNeighbors)
		{
			_playerToDirect = _playerPositions[i];
			setOpening.Invoke();
		}
	}

	private int ClearPlayerSpace(PlayerCycling player)
	{
		int removalIndex = Array.IndexOf(_playerPositions, player);
		_playerPositions[removalIndex] = null;
		_directionTarget = _options[removalIndex];
		return removalIndex;
	}

	public void Remove(PlayerCycling player)
	{
		int removalIndex = ClearPlayerSpace(player);

		if (removalIndex < FirstFreeIndex)
		{
			FirstFreeIndex = removalIndex;
			NotifyPreviews();
		}

		_freeSlots++;
		if (_freeSlots == 1)
		{
			IsFull = false;
			DirectAll();
			return;
		}

		IterateOverNeighbors(LeftNeighbors(removalIndex), DirectRight, 1);
		IterateOverNeighbors(RightNeighbors(removalIndex), DirectLeft, 1);
	}

	private void FindOpenIndex()
	{
		do
		{
			FirstFreeIndex++;
		}
		while (_playerPositions[FirstFreeIndex]);
		NotifyPreviews();
	}

	private void NotifyPreviews()
	{
		foreach (PlayerCycling watcher in _watchers)
		{
			watcher.Current = _options[FirstFreeIndex];
		}
	}

	public void AddPreviewer(PlayerCycling watcher)
	{
		_watchers.Add(watcher);
		watcher.Current = _options[FirstFreeIndex];
	}

	public void RemovePreviewer(PlayerCycling watcher)
	{
		_watchers.Remove(watcher);
	}

	private void DirectAll()
	{
		foreach (PlayerCycling player in _playerPositions)
		{
			if (player)
			{
				player.Left = _directionTarget;
				player.Right = _directionTarget;
			}
		}
	}

	private void DirectLeft()
	{
		_playerToDirect.Left = _directionTarget;
	}

	private void DirectRight()
	{
		_playerToDirect.Right = _directionTarget;
	}

	private Color IterateOverNeighbors(IEnumerable<int> enumerable, Action action, int skip)
	{
		foreach (int i in enumerable.Skip(skip))
		{
			_playerToDirect = _playerPositions[i];
			if (!_playerToDirect)
			{
				return _options[i];
			}
			action.Invoke();
		}
		throw new InvalidOperationException();
	}

	private IEnumerable<int> LeftNeighbors(int start) => RightNeighbors(start + 1).Reverse();
	private IEnumerable<int> RightNeighbors(int start) => Enumerable.Range(start, _options.Length - start).Concat(Enumerable.Range(0, start));
}