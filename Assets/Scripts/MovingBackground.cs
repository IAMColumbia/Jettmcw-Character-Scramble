using UnityEngine;

public class MovingBackground : MonoBehaviour
{
	[SerializeField] private Vector2 _speed;

	public void Update()
	{
		Vector2 newPos = transform.localPosition;
		newPos += _speed * Time.deltaTime;

		if (newPos.x > 1920)
			newPos.x -= 1920 * 2;

		if (newPos.y < -1080)
			newPos.y += 1080 * 2;

		transform.localPosition = newPos;
	}
}
