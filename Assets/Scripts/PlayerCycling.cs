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
	[SerializeField] private Transform _leftAnchor, _middleAnchor, _rightAnchor;

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

	public void Awake()
	{
		PlayerControllerManager pcm = PlayerControllerManager.Instance;
		Transform parent = transform.parent;

		parent.SetParent(pcm.Canvas.GetChild(pcm.Players.Count - 1), true);
		//parent.localScale = Vector3.one * 250;
		parent.localPosition = Vector3.zero;

		currentCircle = CycleOptionsManager.Instance.Rows[0];
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
			_hidden.transform.position = _leftAnchor.position;

			currentCircle.CycleRight(this);

			CycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _rightAnchor.position, _middleAnchor.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, small, 1f, time, Ease.InSine))
				.Group(Tween.Position(_left.transform, _middleAnchor.position, _leftAnchor.position, time, Ease.OutBack))
				.Group(Tween.Scale(_left.transform, 1f, small, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, small, 0f, time, Ease.InSine))
				.Group(Tween.Scale(_right.transform, 0f, small, time, Ease.OutSine))
			;
		}
		else
		{
			_hidden.color = Right;
			_hidden.transform.position = _rightAnchor.position;

			currentCircle.CycleLeft(this);

			CycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _leftAnchor.position, _middleAnchor.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, small, 1f, time, Ease.InSine))
				.Group(Tween.Position(_right.transform, _middleAnchor.position, _rightAnchor.position, time, Ease.OutBack))
				.Group(Tween.Scale(_right.transform, 1f, small, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, small, 0f, time, Ease.InSine))
				.Group(Tween.Scale(_left.transform, 0f, small, time, Ease.OutSine))
			;
		}
	}
}
