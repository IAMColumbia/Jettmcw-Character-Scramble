using UnityEngine;
using PrimeTween;
using UnityEngine.InputSystem;

public class PlayerCycling : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _left, _current, _right, _hidden;
	[SerializeField] private Transform _leftAnchor, _middleAnchor, _rightAnchor, _endAnchor, _leftDisappear, _rightDisappear, _leftNext, _middleNext, _rightNext;
	[SerializeField] private int _rowIndex;

	public Transform Selection => _current.transform;

	public OptionRow OptionRow { get; private set; }

	[SerializeField] private PlayerInput _playerInput;
	public int PlayerNumber { get; private set; }

	public Sequence CycleAnimation, ProgressAnimation;

	private Color _trueCurrentColor, _trueLeftColor, _trueRightColor;

	[SerializeField] private Vector3 _fullScale;
	[SerializeField] private float _sideFactor, _endFactor, _previewFactor;

	private void Awake()
	{
		PlayerNumber = PlayerControllerManager.Instance.GetPlayerNumber(_playerInput) + 1;
	}

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
			_trueLeftColor = value;
			_left.color = value;
		}
	}

	public Color Right
	{
		get => _trueRightColor;
		set
		{
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
		OptionRow = CycleOptionsManager.Instance.Rows[_rowIndex];
		OptionRow.Register(this);
	}

	public void Preview()
	{
		OptionRow = CycleOptionsManager.Instance.Rows[_rowIndex];
		OptionRow.AddPreviewer(this);
	}

	public void StopPreview()
	{
		OptionRow = CycleOptionsManager.Instance.Rows[_rowIndex];
		OptionRow.RemovePreviewer(this);
	}

	public void Cycle(bool right)
	{
		OptionRow = CycleOptionsManager.Instance.Rows[_rowIndex];
		if (OptionRow.IsFull)
		{
			return;
		}

		float time = 0.25f;

		CycleAnimation.Complete();
		ProgressAnimation.Complete();

		Vector3 smallScale = _fullScale * _sideFactor;

		_hidden.color = right ? Left : Right;
		OptionRow.Cycle(this, right);

		if (right)
		{
			_hidden.transform.position = _leftAnchor.position;

			CycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _rightAnchor.position, _middleAnchor.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, smallScale, _fullScale, time, Ease.InSine))
				.Group(Tween.Position(_left.transform, _middleAnchor.position, _leftAnchor.position, time, Ease.OutBack))
				.Group(Tween.Scale(_left.transform, _fullScale, smallScale, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, smallScale, Vector3.zero, time, Ease.InSine))
				.Group(Tween.Scale(_right.transform, Vector3.zero, smallScale, time, Ease.OutSine))
			;
		}
		else
		{
			_hidden.transform.position = _rightAnchor.position;

			CycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _leftAnchor.position, _middleAnchor.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, smallScale, _fullScale, time, Ease.InSine))
				.Group(Tween.Position(_right.transform, _middleAnchor.position, _rightAnchor.position, time, Ease.OutBack))
				.Group(Tween.Scale(_right.transform, _fullScale, smallScale, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, smallScale, Vector3.zero, time, Ease.InSine))
				.Group(Tween.Scale(_left.transform, Vector3.zero, smallScale, time, Ease.OutSine))
			;
		}
	}

	public void SendToEnd()
	{
		CycleAnimation.Complete();
		ProgressAnimation.Complete();

		_hidden.enabled = false;

		float time = 0.15f;

		Vector3 finalScale = _fullScale * _endFactor;
		Vector3 sideScale = _fullScale * _sideFactor;

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Position(_current.transform, _middleAnchor.position, _endAnchor.position, time, Ease.InOutCubic))
			.Group(Tween.Scale(_current.transform, _fullScale, finalScale, time, Ease.OutCirc))
			.Group(Tween.Position(_left.transform, _leftAnchor.position, _leftDisappear.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_left.transform, sideScale, Vector3.zero, 0.25f, Ease.InOutSine))
			.Group(Tween.Position(_right.transform, _rightAnchor.position, _rightDisappear.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_right.transform, sideScale, Vector3.zero, 0.25f, Ease.InOutSine))
		;
	}

	public void ReturnToPosition()
	{
		ProgressAnimation.Complete();

		_hidden.enabled = true;

		float time = 0.15f;

		Vector3 finalScale = _fullScale * _endFactor;
		Vector3 sideScale = _fullScale * _sideFactor;

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Position(_current.transform, _endAnchor.position, _middleAnchor.position, time, Ease.InOutCubic))
			.Group(Tween.Scale(_current.transform, finalScale, _fullScale, time, Ease.InCirc))
			.Group(Tween.Position(_left.transform, _leftDisappear.position, _leftAnchor.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_left.transform, Vector3.zero, sideScale, time, Ease.OutSine))
			.Group(Tween.Position(_right.transform, _rightDisappear.position, _rightAnchor.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_right.transform, Vector3.zero, sideScale, time, Ease.OutSine))
		;
	}

	public void SendToPosition()
	{
		Show(true);
		ProgressAnimation.Complete();

		float time = 0.2f;

		Vector3 sideScale = _fullScale * _sideFactor;
		Vector3 previewScale = _fullScale * _previewFactor;

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Position(_current.transform, _middleNext.position, _middleAnchor.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_current.transform, previewScale, _fullScale, time, Ease.OutSine))
			.Group(Tween.Position(_left.transform, _leftNext.position, _leftAnchor.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_left.transform, Vector3.zero, sideScale, time, Ease.OutSine))
			.Group(Tween.Position(_right.transform, _rightNext.position, _rightAnchor.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_right.transform, Vector3.zero, sideScale, time, Ease.OutSine))
		;
	}

	public void ReturnToPreview()
	{
		CycleAnimation.Complete();
		ProgressAnimation.Complete();

		float time = 0.2f;

		Vector3 sideScale = _fullScale * _sideFactor;
		Vector3 previewScale = _fullScale * _previewFactor;

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Position(_current.transform, _middleAnchor.position, _middleNext.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_current.transform, _fullScale, previewScale, time, Ease.OutSine))
			.Group(Tween.Position(_left.transform, _leftAnchor.position, _leftNext.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_left.transform, sideScale, Vector3.zero, 0.25f, Ease.InOutSine))
			.Group(Tween.Position(_right.transform, _rightAnchor.position, _rightNext.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_right.transform, sideScale, Vector3.zero, 0.25f, Ease.InOutSine))
		;
	}

	public void ShowPreview()
	{
		ProgressAnimation.Complete();
		ShowCurrent(true);

		float time = 0.2f;

		Vector3 previewScale = _fullScale * _previewFactor;

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Scale(_current.transform, Vector3.zero, previewScale, time, Ease.OutSine))
		;
	}

	public void HidePreview()
	{
		ProgressAnimation.Complete();

		float time = 0.2f;

		Vector3 previewScale = _fullScale * _previewFactor;

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Scale(_current.transform, previewScale, Vector3.zero, time, Ease.InSine))
		;
	}

	public void Show(bool show)
	{
		_left.enabled = show;
		_current.enabled = show;
		_right.enabled = show;
		_hidden.enabled = show;
	}

	public void ShowCurrent(bool show)
	{
		_current.enabled = show;
	}

	public void Release()
	{
		OptionRow = CycleOptionsManager.Instance.Rows[_rowIndex];
		OptionRow.Remove(this);
	}
}
