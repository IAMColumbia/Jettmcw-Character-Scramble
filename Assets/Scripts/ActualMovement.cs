using UnityEngine;
using UnityEngine.InputSystem;

public class ActualMovement : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;

	public void Move(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();
		rb.linearVelocityX = input.x * 3;
		float stretch = 1f + (input.y * 0.5f);
		transform.localScale = new Vector3(1f / stretch, stretch);
	}

	public void Jump(InputAction.CallbackContext context)
	{
		rb.AddForceY(200f);
		float randomAngle = Random.Range(-5f, 5f);
		rb.angularVelocity = Mathf.Clamp(rb.angularVelocity + randomAngle, -10f, 10f);
	}
}
