using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ControlRandomizer : MonoBehaviour
{
	private static readonly string[] s_actions = new string[] { "Move", "Confirm", "Cancel" };
	private static readonly string[] s_filters = new string[] { "Slot A", "Slot B", "Slot C", "Slot D" };
	private static readonly int[] s_bindingCounts = new int[] { 3, 4, 4 };

	public static ControlRandomizer Instance { get; private set; }

	public int[] FilterIdxs { get; } = new int[3];

	public UnityEvent ControlSchemeChanged;

	private void Awake()
	{
		Instance = this;
		RandomizeBindings();
	}

	public void RandomizeBindings()
	{
		for (int i = 0; i < 3; i++)
		{
			int filterIdx = Random.Range(0, s_bindingCounts[i]);

			FilterIdxs[i] = filterIdx;
		}

		ControlSchemeChanged.Invoke();
	}

	public void SetToCommonBindings(PlayerInput player)
	{
		for (int i = 0; i < 3; i++)
		{
			string actionName = s_actions[i];
			InputAction action = player.actions.FindAction(actionName);

			int filterIdx = FilterIdxs[i];
			string filter = s_filters[filterIdx];
			ApplyFilter(action, filter);
		}

		ControlSchemeChanged.Invoke();
	}

	private static void ApplyFilter(InputAction action, string group)
	{
		action.Disable();

		for (int i = 0; i < action.bindings.Count; i++)
		{
			InputBinding binding = action.bindings[i];
			if (binding.isComposite)
			{
				continue;
			}

			bool isRightSlot = binding.groups.Contains(group);

			string path = isRightSlot ? null : string.Empty;
			binding = new() { overridePath = path };
			action.ApplyBindingOverride(i, binding);
		}

		action.Enable();
	}
}
