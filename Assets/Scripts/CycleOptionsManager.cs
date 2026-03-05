using UnityEngine;

public class CycleOptionsManager : MonoBehaviour
{
	public static CycleOptionsManager Instance { get; private set; }

	public CyclicalOptions[] Rows;

	public void Awake()
	{
		Instance = this;
	}
}
