using UnityEngine;

public class PlayerCycling : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _current;
	[SerializeField] private SpriteRenderer _left;
	[SerializeField] private SpriteRenderer _right;

	public Color Current { get => _current.color; set => _current.color = value; }
	public Color Left { get => _left.color; set => _left.color = value; }
	public Color Right { get => _right.color; set => _right.color = value; }

	public void Awake()
	{
		CyclicalOptions.Instance.Register(this);
	}

	public void Cycle(bool right)
	{
		CyclicalOptions co = CyclicalOptions.Instance;
		if (right)
		{
			co.CycleRight(this);
		}
		else
		{
			co.CycleLeft(this);
		}
	}
}
