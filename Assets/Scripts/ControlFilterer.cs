using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ControlFilterer : MonoBehaviour
{
	[Header("Cycle Action")]
	[SerializeField] private string[] _cycleActionNames;
	public int CycleActionIndex;
	public UnityEvent<InputAction.CallbackContext> CorrectCycleInput, IncorrectCycleInput;

	[Header("Select Action")]
	[SerializeField] private string[] _selectActionNames;
	public int SelectActionIndex;
	public UnityEvent<InputAction.CallbackContext> CorrectSelectInput, IncorrectSelectInput;

	[Header("Back Action")]
	[SerializeField] private string[] _backActionNames;
	public int BackActionIndex;
	public UnityEvent<InputAction.CallbackContext> CorrectBackInput, IncorrectBackInput;

	private void OnEnable()
	{
		ControlRandomizer.Instance.ControlSchemeChanged.AddListener(SetFilters);
		SetFilters();
	}

	private void OnDisable()
	{
		ControlRandomizer.Instance.ControlSchemeChanged.RemoveListener(SetFilters);
	}

	private void SetFilters()
	{
		ControlRandomizer cr = ControlRandomizer.Instance;
		CycleActionIndex = cr.FilterIdxs[0];
		SelectActionIndex = cr.FilterIdxs[1];
		BackActionIndex = cr.FilterIdxs[2];
	}

	public void FilterCycleAction(InputAction.CallbackContext context)
	{
		FilterAction(context, _cycleActionNames, CycleActionIndex, CorrectCycleInput, IncorrectCycleInput);
	}

	public void FilterSelectAction(InputAction.CallbackContext context)
	{
		FilterAction(context, _selectActionNames, SelectActionIndex, CorrectSelectInput, IncorrectSelectInput);
	}

	public void FilterBackAction(InputAction.CallbackContext context)
	{
		FilterAction(context, _backActionNames, BackActionIndex, CorrectBackInput, IncorrectBackInput);
	}

	private void FilterAction(
		InputAction.CallbackContext context,
		string[] actions,
		int index,
		UnityEvent<InputAction.CallbackContext> correctEvent,
		UnityEvent<InputAction.CallbackContext> incorrectEvent)
	{
		string action = context.action.name;
		string correct = actions[index];
		bool wasCorrect = action == correct;
		Debug.Log(wasCorrect);
		var eventToInvoke = wasCorrect ? correctEvent : incorrectEvent;
		eventToInvoke.Invoke(context);
	}
}
