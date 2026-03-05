using UnityEngine;
using PrimeTween;

public class PlayerCycling : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _left;
	[SerializeField] private SpriteRenderer _current;
	[SerializeField] private SpriteRenderer _right;
	[SerializeField] private SpriteRenderer _hidden;

	[SerializeField] private Transform _leftAnchor;
	[SerializeField] private Transform _middleAnchor;
	[SerializeField] private Transform _rightAnchor;

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

		if (CycleAnimation.isAlive) CycleAnimation.Complete();

		if (right)
		{
			_hidden.color = Left;
			_hidden.transform.position = _leftAnchor.position;

			currentCircle.CycleRight(this);

			CycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _rightAnchor.position, _middleAnchor.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, 0.7f, 1f, time, Ease.InSine))
				.Group(Tween.Position(_left.transform, _middleAnchor.position, _leftAnchor.position, time, Ease.OutBack))
				.Group(Tween.Scale(_left.transform, 1f, 0.7f, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, 0.7f, 0f, time, Ease.InSine))
				.Group(Tween.Scale(_right.transform, 0f, 0.7f, time, Ease.OutSine))
			;
		}
		else
		{
			_hidden.color = Right;
			_hidden.transform.position = _rightAnchor.position;

			currentCircle.CycleLeft(this);


			CycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _leftAnchor.position, _middleAnchor.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, 0.7f, 1f, time, Ease.InSine))
				.Group(Tween.Position(_right.transform, _middleAnchor.position, _rightAnchor.position, time, Ease.OutBack))
				.Group(Tween.Scale(_right.transform, 1f, 0.7f, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, 0.7f, 0f, time, Ease.InSine))
				.Group(Tween.Scale(_left.transform, 0f, 0.7f, time, Ease.OutSine))
			;
		}
	}
}
