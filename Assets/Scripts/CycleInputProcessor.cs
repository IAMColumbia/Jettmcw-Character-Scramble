using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CycleInputProcessor : MonoBehaviour
{
	[SerializeField] private float threshold = 0.5f;
	[SerializeField] private float firstRepeatDelay = 0.5f;
	[SerializeField] private float repeatRate = 0.25f;

	private HoldData _horizontalData;
	private float _horizontalRepeatWait;

	public UnityEvent<bool> MoveHorizontal;

	public void OnDirectionalInput(InputAction.CallbackContext context)
	{
		// Only respond to "performed" phase, as to avoid duplicate signals at "started"
		switch (context.phase)
		{
			// If input is cancelled that means the player is no longer holding
			case InputActionPhase.Canceled:
				_horizontalData = HoldData.None;
				return;
			case InputActionPhase.Performed:
				break;
			default:
				return;
		}

		float input = context.ReadValue<float>();

		AdjustHoldData(input, ref _horizontalData);
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
		MovementUpdate(MoveHorizontal, ref _horizontalData, ref _horizontalRepeatWait);
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

	private enum HoldData : byte
	{
		None = 0,
		Holding = 1,
		PositiveDir = 2,
		Starting = 4
	}
}
