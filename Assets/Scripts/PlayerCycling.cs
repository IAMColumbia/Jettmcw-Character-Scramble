using UnityEngine;
using PrimeTween;

public class PlayerCycling : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _left;
	[SerializeField] private SpriteRenderer _current;
	[SerializeField] private SpriteRenderer _right;
	[SerializeField] private SpriteRenderer _hidden;

	private Sequence CycleAnimation;

	private CyclicalOptions currentCircle;

	private Color _trueCurrentColor;
	private Color _trueLeftColor;
	private Color _trueRightColor;

	public Color Current
	{
		get
		{
			return _trueCurrentColor;
		}

		set
		{
			if (_trueCurrentColor == value) return;

			_trueCurrentColor = value;
			_current.color = value;
		}
	}

	public Color Left
	{
		get
		{
			return _trueLeftColor;
		}

		set
		{
			if (_trueLeftColor == value) return;

			_trueLeftColor = value;
			_left.color = value;
		}
	}

	public Color Right
	{
		get
		{
			return _trueRightColor;
		}

		set
		{
			if (_trueRightColor == value) return;
			_trueRightColor = value;


			_right.color = value;
		}
	}

	public void Start()
	{
		currentCircle = CyclicalOptions.Instance;
		currentCircle.Register(this);
	}

	public void Cycle(bool right)
	{
		float time = 0.25f;
		float small = 0.6f;

		if (CycleAnimation.isAlive) CycleAnimation.Complete();

		if (right)
		{
			_hidden.color = Left;
			_hidden.transform.position = _left.transform.parent.position;

			currentCircle.CycleRight(this);

			CycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _right.transform.parent.position, _current.transform.parent.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, small, 1f, time, Ease.InSine))
				.Group(Tween.Position(_left.transform, _current.transform.parent.position, _left.transform.parent.position, time, Ease.OutBack))
				.Group(Tween.Scale(_left.transform, 1f, small, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, small, 0f, time, Ease.InSine))
				.Group(Tween.Scale(_right.transform, 0f, small, time, Ease.OutSine))
			;
		}
		else
		{
			_hidden.color = Right;
			_hidden.transform.position = _right.transform.parent.position;

			currentCircle.CycleLeft(this);

			CycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _left.transform.parent.position, _current.transform.parent.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, small, 1f, time, Ease.InSine))
				.Group(Tween.Position(_right.transform, _current.transform.parent.position, _right.transform.parent.position, time, Ease.OutBack))
				.Group(Tween.Scale(_right.transform, 1f, small, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, small, 0f, time, Ease.InSine))
				.Group(Tween.Scale(_left.transform, 0f, small, time, Ease.OutSine))
			;
		}
	}
}
