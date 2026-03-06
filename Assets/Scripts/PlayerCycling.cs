using UnityEngine;
using PrimeTween;

public class PlayerCycling : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _left, _current, _right, _hidden;
	[SerializeField] private Transform _leftAnchor, _middleAnchor, _rightAnchor, _endAnchor;
	[SerializeField] private int _rowIndex;

	private OptionRow _optionRow;

	private Sequence CycleAnimation;
	private Color _trueCurrentColor, _trueLeftColor, _trueRightColor;

	public Color Current
	{
		get => _trueCurrentColor;
		set
		{
			if (_trueCurrentColor == value) return;
			_trueCurrentColor = value;
			_current.color = value;
		}
	}

	public Color Left
	{
		get => _trueLeftColor;
		set
		{
			if (_trueLeftColor == value) return;
			_trueLeftColor = value;
			_left.color = value;
		}
	}

	public Color Right
	{
		get => _trueRightColor;
		set
		{
			if (_trueRightColor == value) return;
			_trueRightColor = value;
			_right.color = value;
		}
	}

	public void Register()
	{
		_left.enabled = true;
		_current.enabled = true;
		_right.enabled = true;
		_hidden.enabled = true;
		_optionRow = CycleOptionsManager.Instance.Rows[_rowIndex];
		_optionRow.Register(this);
	}

	public void Cycle(bool right)
	{
		_optionRow = CycleOptionsManager.Instance.Rows[_rowIndex];
		if (_optionRow.IsFull)
		{
			return;
		}

		float time = 0.25f;
		float small = 0.6f;

		CycleAnimation.Complete();

		_hidden.color = right ? Left : Right;
		_optionRow.Cycle(this, right);

		if (right)
		{
			_hidden.transform.position = _leftAnchor.position;

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
			_hidden.transform.position = _rightAnchor.position;

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

	public void BeMovedAway()
	{
		CycleAnimation.Complete();
		_left.enabled = false;
		_right.enabled = false;
		_hidden.enabled = false;
		_current.transform.position = _endAnchor.position;
	}

	public void BeMovedBack()
	{
		_left.enabled = true;
		_right.enabled = true;
		_hidden.enabled = true;
		_current.transform.position = _middleAnchor.position;
	}

	public void Show(bool show)
	{
		_left.enabled = show;
		_current.enabled = show;
		_right.enabled = show;
		_hidden.enabled = show;
	}

	public void Release()
	{
		_optionRow = CycleOptionsManager.Instance.Rows[_rowIndex];
		_optionRow.Remove(this);
	}
}
