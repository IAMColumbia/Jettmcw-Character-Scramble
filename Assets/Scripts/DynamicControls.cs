using UnityEngine.InputSystem;

public static class DynamicControls
{
	public static void ApplyFilter(InputAction action, string slot)
	{
		action.Disable();

		for (int i = 0; i < action.bindings.Count; i++)
		{
			InputBinding binding = action.bindings[i];

			if (binding.isComposite) continue;
			bool isRightSlot = binding.groups.Contains(slot);

			string path = isRightSlot ? null : string.Empty;
			binding = new() { overridePath = path };
			action.ApplyBindingOverride(i, binding);
		}

		action.Enable();
	}
}
