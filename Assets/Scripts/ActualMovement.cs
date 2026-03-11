using UnityEngine;
using UnityEngine.InputSystem;

public class ActualMovement : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
	Vector2 moveIn = Vector2.zero;

	public void Move(InputAction.CallbackContext context)
	{
		moveIn = context.ReadValue<Vector2>();
	}

	public void FixedUpdate()
	{
		rb.linearVelocityX = moveIn.x * 3;
		float stretch = 1f + (moveIn.y * 0.5f);
		Vector3 newScale = Vector3.Lerp(transform.localScale, new Vector3(1f / stretch, stretch), 0.2f);
		transform.localScale = newScale;
	}

	public void Jump(InputAction.CallbackContext _)
	{
		rb.AddForceY(200f);
		float randomAngle = Random.Range(-5f, 5f);
		rb.angularVelocity = Mathf.Clamp(rb.angularVelocity + randomAngle, -10f, 10f);
	}
}
