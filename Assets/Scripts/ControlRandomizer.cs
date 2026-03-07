using UnityEngine;
using UnityEngine.Events;

public class ControlRandomizer : MonoBehaviour
{
	private static readonly int[] s_bindingCounts = new int[] { 3, 4, 4 };

	public static ControlRandomizer Instance { get; private set; }

	public int[] FilterIdxs { get; } = new int[3];

	public UnityEvent ControlSchemeChanged;

	private void Awake()
	{
		Instance = this;
		RandomizeBindings();
	}

	public void RandomizeBindings()
	{
		for (int i = 0; i < 3; i++)
		{
			int filterIdx = Random.Range(0, s_bindingCounts[i]);

			FilterIdxs[i] = filterIdx;
		}

		ControlSchemeChanged.Invoke();
	}
}
