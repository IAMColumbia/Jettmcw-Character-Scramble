using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public void OnDirectionalInput(InputAction.CallbackContext context)
	{
		Vector2 value = context.ReadValue<Vector2>();
		transform.position += new Vector3(value.x, value.y);
	}
}
