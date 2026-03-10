using System.Linq;
using UnityEngine;

public class CycleOptionsManager : MonoBehaviour
{
	public static CycleOptionsManager Instance { get; private set; }

	public OptionRow[] Rows;

	[SerializeField] private Color[] _colors;

	public void Awake()
	{
		Instance = this;
	}

	public void SetColors()
	{
		Rows[0].ReceiveColors(Utility.Choose(_colors).Take(6));
		Rows[1].ReceiveColors(Utility.Choose(_colors).Take(4));
		Rows[2].ReceiveColors(Utility.Choose(_colors).Take(5));
	}
}
