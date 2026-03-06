using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerControlProcessor : MonoBehaviour
{
	public PlayerInput PlayerInput;

	private HoldData _horizontalData;
	private HoldData _verticalData;

	[SerializeField] private float firstRepeatDelay = 0.5f;
	[SerializeField] private float repeatRate = 0.25f;

	private float horizontalRepeatWait;
	private float verticalRepeatWait;

	[SerializeField] private float threshold = 0.5f;

	public UnityEvent<bool> MoveHorizontal;
	public UnityEvent<bool> MoveVertical;
	public UnityEvent Confirm;

	[SerializeField] private IconTexture _leftIcon;
	[SerializeField] private IconTexture _rightIcon;
	[SerializeField] private IconTexture _confirmIcon;

	public void OnDirectionalInput(InputAction.CallbackContext context)
	{
		// Only respond to "performed" phase, as to avoid duplicate signals at "started"
		switch (context.phase)
		{
			// If input is cancelled that means the player is no longer holding
			case InputActionPhase.Canceled:
				_horizontalData = _verticalData = HoldData.None;
				return;
			case InputActionPhase.Performed:
				break;
			default:
				return;
		}

		Vector2 input = context.ReadValue<Vector2>();

		AdjustHoldData(input.x, ref _horizontalData);
		AdjustHoldData(input.y, ref _verticalData);
	}

	private void AdjustHoldData(float input, ref HoldData data)
	{
		bool nowHoldingPositive = input > threshold;
		bool nowHolding = nowHoldingPositive || input < -threshold;

		// Case where we're not holding anything.
		if (!nowHolding)
		{
			data = HoldData.None;
			return;
		}

		bool wasNotHolding = (data & HoldData.Holding) == 0;
		bool wasNotPositive = (data & HoldData.PositiveDir) == 0;

		if (wasNotHolding || nowHoldingPositive == wasNotPositive)
		{
			data |= HoldData.Starting;
		}

		data |= HoldData.Holding;

		if (nowHoldingPositive)
		{
			data |= HoldData.PositiveDir;
		}
		else
		{
			data &= ~HoldData.PositiveDir;
		}
	}

	private void Update()
	{
		MovementUpdate(MoveHorizontal, ref _horizontalData, ref horizontalRepeatWait);
		MovementUpdate(MoveVertical, ref _verticalData, ref verticalRepeatWait);
	}

	private void MovementUpdate(UnityEvent<bool> moveEvent, ref HoldData data, ref float repeatWait)
	{
		if ((data & HoldData.Holding) == 0)
		{
			return;
		}

		bool holdingPositive = (data & HoldData.PositiveDir) != 0;

		if ((data & HoldData.Starting) != 0)
		{
			Debug.Log(holdingPositive);
			moveEvent.Invoke(holdingPositive);
			repeatWait = firstRepeatDelay;
			data &= ~HoldData.Starting;
			return;
		}

		repeatWait -= Time.deltaTime;

		if (repeatWait <= 0)
		{
			moveEvent.Invoke(holdingPositive);
			repeatWait = repeatRate;
		}
	}

	public void OnConfirm(InputAction.CallbackContext context)
	{
		if (!context.started)
		{
			return;
		}

		Confirm.Invoke();

		PlayerInput.DeactivateInput();
	}

	private enum HoldData : byte
	{
		None = 0,
		Holding = 1,
		PositiveDir = 2,
		Starting = 4
	}
}
