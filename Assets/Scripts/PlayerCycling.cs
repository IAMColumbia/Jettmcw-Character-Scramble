using UnityEngine;

public class PlayerCycling : MonoBehaviour
{
	public SpriteRenderer Renderer;

	public Color[] colors;
	public int currentSelection = 0;

	public void Cycle(bool right)
	{
		if (right)
		{
			currentSelection++;
			if (currentSelection >= colors.Length) currentSelection = 0;
		}
		else
		{
			currentSelection--;
			if (currentSelection < 0) currentSelection = colors.Length - 1;
		}

		Renderer.color = colors[currentSelection];
	}

	public void LogHoriz(bool right)
	{
		Debug.Log(right ? "Right" : "Left");
	}
	public void LogVert(bool up)
	{
		Debug.Log(up ? "Up" : "Down");
	}
}
