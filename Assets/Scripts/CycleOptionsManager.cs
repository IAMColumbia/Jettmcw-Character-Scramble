using UnityEngine;

public class CycleOptionsManager : MonoBehaviour
{
	public static CycleOptionsManager Instance { get; private set; }

	public OptionRow[] Rows;

	public void Awake()
	{
		Instance = this;
	}
}
