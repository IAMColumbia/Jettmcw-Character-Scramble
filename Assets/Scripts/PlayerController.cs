using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public PlayerInput PlayerInput;
	public SpriteRenderer Renderer;

	public Color[] colors;
	public int currentSelection = 0;

	public void OnDirectionalInput(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();
		Debug.Log(input);

		float threshold = 0.5f;

		if (input.x > threshold)
		{
			currentSelection++;
			if (currentSelection >= colors.Length) currentSelection = 0;
			Renderer.color = colors[currentSelection];
		}
		else if (input.x < -threshold)
		{
			currentSelection--;
			if (currentSelection < 0) currentSelection = colors.Length - 1;

			Renderer.color = colors[currentSelection];
		}
	}
}
