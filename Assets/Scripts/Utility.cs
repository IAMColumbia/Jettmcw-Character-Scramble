using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

	public static IEnumerable<T> Choose<T>(IList<T> source)
	{
		List<T> pool = new(source);

		while (pool.Count > 0)
		{
			int index = Random.Range(0, pool.Count);
			yield return pool[index];
			pool.RemoveAtSwapBack(index);
		}
	}
}
