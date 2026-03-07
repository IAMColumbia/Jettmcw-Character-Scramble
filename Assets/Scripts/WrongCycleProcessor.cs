using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WrongCycleProcessor : MonoBehaviour
{
	[SerializeField] private float _threshold = 0.5f;
	private readonly Dictionary<string, bool> _wronglyHeldActions = new();

	public UnityEvent WrongLeft, WrongRight;

	public void OnDirectionalInput(InputAction.CallbackContext context)
	{
		string action = context.action.name;

		// Only respond to "performed" phase, as to avoid duplicate signals at "started"
		switch (context.phase)
		{
			// If input is cancelled that means the player is no longer holding
			case InputActionPhase.Canceled:
				_wronglyHeldActions.Remove(action);
				return;
			case InputActionPhase.Performed:
				break;
			default:
				return;
		}

		float input = context.ReadValue<float>();

		bool holdingPositive = input > _threshold;
		bool holding = holdingPositive || input < -_threshold;

		if (!holding)
		{
			_wronglyHeldActions.Remove(action);
			return;
		}

		bool wasHeld = _wronglyHeldActions.TryGetValue(action, out bool wasPositive);
		if (wasHeld && holdingPositive == wasPositive)
		{
			return;
		}

		_wronglyHeldActions[action] = holdingPositive;
		UnityEvent eventToInvoke = holdingPositive ? WrongRight : WrongLeft;
		eventToInvoke.Invoke();
	}
}
