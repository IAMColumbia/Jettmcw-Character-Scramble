using System.Collections.Generic;
using UnityEngine;
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

	public string MoveBindingSlot;
	public string ConfirmBindingSlot;
	public string CancelBindingSlot;

	private void Awake()
	{
		Instance = this;
		RandomizeBindings();
	}

	public void RandomizeBindings()
	{
		RandomizeBinding(MoveSlots, ref MoveBindingSlot);
		RandomizeBinding(ConfirmSlots, ref ConfirmBindingSlot);
		RandomizeBinding(CancelSlots, ref CancelBindingSlot);

		foreach (PlayerInput player in Players)
		{
			SetBindings(player);
		}
	}

	private void RandomizeBinding(string[] slots, ref string output)
	{
		int idx = Random.Range(0, slots.Length);
		output = slots[idx];
		Debug.Log(output);
	}

	public void OnPlayerAdded(PlayerInput player)
    {
        Players.Add(player);
		SetBindings(player);
	}
	private void SetBindings(PlayerInput player)
	{
		InputActionAsset actions = player.actions;
		ControlSchemeOverrider.ApplySlotFilter(actions.FindAction("Move"), MoveBindingSlot);
		ControlSchemeOverrider.ApplySlotFilter(actions.FindAction("Confirm"), ConfirmBindingSlot);
		ControlSchemeOverrider.ApplySlotFilter(actions.FindAction("Cancel"), CancelBindingSlot);
	}
}
