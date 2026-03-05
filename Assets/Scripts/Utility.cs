using System.Collections;
using UnityEngine;

public static class Utility
{
	public static void Shuffle(IList list)
	{
		for (int i = list.Count - 1; i > 0; i--)
		{
			int j = Random.Range(0, i + 1);
			(list[i], list[j]) = (list[j], list[i]);
		}
	}
}
