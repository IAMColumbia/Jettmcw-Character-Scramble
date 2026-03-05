using UnityEngine;
using UnityEngine.InputSystem;

public class ControlSchemeOverrider : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInputs;
	[SerializeField] private InputActionAsset _actions;

	private InputAction _moveAction;
	private InputAction _confirmAction;
	private InputAction _cancelAction;

	private string _currentMoveSlot = "Slot A";
	private string _currentConfirmSlot = "Slot A";
	private string _currentCancelSlot = "Slot A";

	private void Awake()
	{
		_playerInputs = GetComponent<PlayerInput>();
		_actions = _playerInputs.actions;

		_moveAction = _actions.FindAction("Move");
		_confirmAction = _actions.FindAction("Confirm");
		_cancelAction = _actions.FindAction("Cancel");
	}

	public static void ApplySlotFilter(InputAction action, string activeSlot)
	{
		action.Disable();

		for (int i = 0; i < action.bindings.Count; i++)
		{
			var binding = action.bindings[i];

			if (binding.isComposite) continue;

			bool isRightSlot = binding.groups.Contains(activeSlot);

			if (isRightSlot) Debug.Log(binding.name);

			string path = isRightSlot ? null : string.Empty;
			InputBinding bindingOverride = new() { overridePath = path };
			action.ApplyBindingOverride(i, bindingOverride);
		}

		action.Enable();
	}
}
