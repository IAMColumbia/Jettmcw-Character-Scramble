using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerControllerManager : MonoBehaviour
{
	private const string SlotA = "Slot A";
	private const string SlotB = "Slot B";
	private const string SlotC = "Slot C";
	private const string SlotD = "Slot D";
	private static readonly string[] MoveSlots = new string[] { SlotA, SlotB, SlotC };
	private static readonly string[] ConfirmSlots = new string[] { SlotA, SlotB, SlotC, SlotD };
	private static readonly string[] CancelSlots = new string[] { SlotA, SlotB, SlotC, SlotD };

	public static PlayerControllerManager Instance;
    public Transform Canvas;
    public List<PlayerInput> Players;
	public UnityEvent ControlSchemeChanged;

	public string MoveBindingSlot, ConfirmBindingSlot, CancelBindingSlot;
	public int MoveBindingIdx, ConfirmBindingIdx, CancelBindingIdx;

	private void Awake()
	{
		Instance = this;
		RandomizeBindings();
	}

	public void RandomizeBindings()
	{
		RandomizeBinding(MoveSlots, ref MoveBindingSlot, ref MoveBindingIdx);
		RandomizeBinding(ConfirmSlots, ref ConfirmBindingSlot, ref ConfirmBindingIdx);
		RandomizeBinding(CancelSlots, ref CancelBindingSlot, ref CancelBindingIdx);

		foreach (PlayerInput player in Players)
		{
			SetBindings(player.actions);
		}
	}

	private void RandomizeBinding(string[] slots, ref string name, ref int idx)
	{
		idx = Random.Range(0, slots.Length);
		name = slots[idx];
		Debug.Log(name);
	}

	public void OnPlayerAdded(PlayerInput player)
    {
        Players.Add(player);

		SetBindings(player.actions);
		
		player.DeactivateInput();
		StartCoroutine(EnableNextFrame());

		IEnumerator EnableNextFrame()
		{
			yield return null;
			player.ActivateInput();
		}
	}
	private void SetBindings(InputActionAsset actions)
	{
		DynamicControls.ApplyFilter(actions.FindAction("Move"), MoveBindingSlot);
		DynamicControls.ApplyFilter(actions.FindAction("Confirm"), ConfirmBindingSlot);
		DynamicControls.ApplyFilter(actions.FindAction("Cancel"), CancelBindingSlot);
		ControlSchemeChanged.Invoke();
	}
}
