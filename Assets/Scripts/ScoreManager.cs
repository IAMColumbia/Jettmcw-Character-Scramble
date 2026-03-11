using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance { get; private set; }

	[SerializeField] private PlayerStats[] _playerStats;

	private void Awake()
	{
		Instance = this;
	}

	public void UpdatePlacements()
	{
		List<PlayerStats> ordered = _playerStats.OrderByDescending(stats => stats.Score).ToList();
		ordered[0].Placement = 1;
		
		int prevScore = ordered[0].Score;
		int prevPlacement = 1;

		for (int i = 1; i < ordered.Count; i++)
		{
			PlayerStats stats = ordered[i];
			int score = stats.Score;
			if (score != prevScore)
			{
				prevScore = score;
				prevPlacement = i + 1;
			}
			stats.Placement = prevPlacement;
		}
	}
}
