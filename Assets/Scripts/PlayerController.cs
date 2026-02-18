using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public PlayerInput PlayerInput;
	public SpriteRenderer Renderer;

	public Color[] colors;
	public int currentSelection = 0;

	private bool holdingHorizontally;
	private bool holdingRight;
	private bool startingHorizontalHold;

	[SerializeField] private float firstRepeatDelay = 0.5f;
	[SerializeField] private float repeatRate = 0.25f;

	private float horizontalRepeatWait;

	[SerializeField] private float threshold = 0.5f;


	public void OnDirectionalInput(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Canceled:
				holdingHorizontally = false;
				return;
			case InputActionPhase.Performed:
				break;
			default:
				return;
		}

		Vector2 input = context.ReadValue<Vector2>();

		bool nowHoldingRight = input.x > threshold;
		bool nowHoldingHorizontally = nowHoldingRight || input.x < -threshold;

		// Case where we're not holding anything.
		if (!nowHoldingHorizontally)
		{
			holdingHorizontally = false;
			return;
		}

		startingHorizontalHold = !(holdingHorizontally && nowHoldingRight == holdingRight);
		holdingHorizontally = true;
		holdingRight = nowHoldingRight;
	}

	public void Update()
	{
		if (!holdingHorizontally)
		{
			return;
		}

		if (startingHorizontalHold)
		{
			Cycle();
			horizontalRepeatWait = firstRepeatDelay;
			startingHorizontalHold = false;
			return;
		}

		horizontalRepeatWait -= Time.deltaTime;
		
		if (horizontalRepeatWait <= 0)
		{
			Cycle();
			horizontalRepeatWait = repeatRate;
		}
	}

	private void Cycle()
	{
		if (holdingRight)
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

	public void OnConfirm(InputAction.CallbackContext context)
	{
		transform.localScale *= 2;
		PlayerInput.DeactivateInput();
	}
}
