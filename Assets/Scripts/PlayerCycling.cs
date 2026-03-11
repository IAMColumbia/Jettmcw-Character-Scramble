using UnityEngine;
using PrimeTween;
using UnityEngine.InputSystem;

public class PlayerCycling : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private SpriteRenderer _left, _current, _right, _hidden;
	[SerializeField] private PlayerAnchors _anchors;
	[SerializeField] private int _rowIndex;
	[SerializeField] private Vector3 _fullScale;
	[SerializeField] private float _sideFactor, _endFactor, _previewFactor;

	public OptionRow OptionRow { get; private set; }
	public int PlayerNumber { get; private set; }
	public Sequence ProgressAnimation { get; private set; }

	private Sequence _cycleAnimation;
	private Tween _leftColorChange, _rightColorChange, _currentColorChange;

	private void Awake()
	{
		PlayerNumber = PlayerControllerManager.Instance.GetPlayerNumber(_playerInput) + 1;
	}

	public Color Left { get; private set; }
	public Color Current { get; private set; }
	public Color Right { get; private set; }

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

		SFXManager.PlayMove(Random.Range(0.15f, 0.25f));

		_cycleAnimation.Complete();
		ProgressAnimation.Complete();

		Vector3 smallScale = _fullScale * _sideFactor;

		_hidden.color = right ? Left : Right;
		OptionRow.Cycle(this, right);

		if (right)
		{
			_hidden.transform.position = _anchors.LeftChoice.position;

			_cycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _anchors.RightChoice.position, _anchors.Selection.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, smallScale, _fullScale, time, Ease.InSine))
				.Group(Tween.Position(_left.transform, _anchors.Selection.position, _anchors.LeftChoice.position, time, Ease.OutBack))
				.Group(Tween.Scale(_left.transform, _fullScale, smallScale, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, smallScale, Vector3.zero, time, Ease.InSine))
				.Group(Tween.Scale(_right.transform, Vector3.zero, smallScale, time, Ease.OutSine))
			;
		}
		else
		{
			_hidden.transform.position = _anchors.RightChoice.position;

			_cycleAnimation = Sequence.Create()
				.Group(Tween.Position(_current.transform, _anchors.LeftChoice.position, _anchors.Selection.position, time, Ease.InBack))
				.Group(Tween.Scale(_current.transform, smallScale, _fullScale, time, Ease.InSine))
				.Group(Tween.Position(_right.transform, _anchors.Selection.position, _anchors.RightChoice.position, time, Ease.OutBack))
				.Group(Tween.Scale(_right.transform, _fullScale, smallScale, time, Ease.OutSine))
				.Group(Tween.Scale(_hidden.transform, smallScale, Vector3.zero, time, Ease.InSine))
				.Group(Tween.Scale(_left.transform, Vector3.zero, smallScale, time, Ease.OutSine))
			;
		}
	}

	public void SendToEnd()
	{
		_cycleAnimation.Complete();
		ProgressAnimation.Complete();

		_hidden.enabled = false;

		float time = 0.15f;

		Vector3 finalScale = _fullScale * _endFactor;
		Vector3 sideScale = _fullScale * _sideFactor;

		SFXManager.PlayUp(0.5f);

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Position(_current.transform, _anchors.Selection.position, _anchors.Finished[_rowIndex].position, time, Ease.InOutCubic))
			.Group(Tween.Scale(_current.transform, _fullScale, finalScale, time, Ease.OutCirc))
			.Group(Tween.Position(_left.transform, _anchors.LeftChoice.position, _anchors.LeftUpwards.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_left.transform, sideScale, Vector3.zero, 0.25f, Ease.InOutSine))
			.Group(Tween.Position(_right.transform, _anchors.RightChoice.position, _anchors.RightUpwards.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_right.transform, sideScale, Vector3.zero, 0.25f, Ease.InOutSine))
			.InsertCallback(time, SnapIn)
		;
	}

	private void SnapIn()
	{
		Progression progression = _playerInput.GetComponent<Progression>();
		Transform end = progression.FinalCharacter;
		progression.SnapShake.Complete();
		_current.transform.SetParent(end, true);
		progression.SnapShake = Sequence.Create()
			.Group(Tween.ShakeLocalPosition(end, new Vector3(0.1f, 0.3f, 0f), 0.1f))
		;
	}

	public void ReturnToPosition()
	{
		ProgressAnimation.Complete();

		_hidden.enabled = true;

		float time = 0.15f;

		Vector3 finalScale = _fullScale * _endFactor;
		Vector3 sideScale = _fullScale * _sideFactor;

		DisconnectCurrentFromComplete();

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Position(_current.transform, _anchors.Finished[_rowIndex].position, _anchors.Selection.position, time, Ease.InOutCubic))
			.Group(Tween.Scale(_current.transform, finalScale, _fullScale, time, Ease.InCirc))
			.Group(Tween.Position(_left.transform, _anchors.LeftUpwards.position, _anchors.LeftChoice.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_left.transform, Vector3.zero, sideScale, time, Ease.OutSine))
			.Group(Tween.Position(_right.transform, _anchors.RightUpwards.position, _anchors.RightChoice.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_right.transform, Vector3.zero, sideScale, time, Ease.OutSine))
		;
	}

	public void DisconnectCurrentFromComplete()
	{
		_current.transform.SetParent(_left.transform.parent, true);
	}

	public void SendToPosition()
	{
		Show(true);
		ProgressAnimation.Complete();

		float time = 0.2f;

		Vector3 sideScale = _fullScale * _sideFactor;
		Vector3 previewScale = _fullScale * _previewFactor;

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Position(_current.transform, _anchors.Preview.position, _anchors.Selection.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_current.transform, previewScale, _fullScale, time, Ease.OutSine))
			.Group(Tween.Position(_left.transform, _anchors.LeftDownwards.position, _anchors.LeftChoice.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_left.transform, Vector3.zero, sideScale, time, Ease.OutSine))
			.Group(Tween.Position(_right.transform, _anchors.RightDownwards.position, _anchors.RightChoice.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_right.transform, Vector3.zero, sideScale, time, Ease.OutSine))
		;
	}

	public void ReturnToPreview()
	{
		_cycleAnimation.Complete();
		ProgressAnimation.Complete();

		float time = 0.2f;

		Vector3 sideScale = _fullScale * _sideFactor;
		Vector3 previewScale = _fullScale * _previewFactor;

		ProgressAnimation = Sequence.Create()
			.Group(Tween.Position(_current.transform, _anchors.Selection.position, _anchors.Preview.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_current.transform, _fullScale, previewScale, time, Ease.OutSine))
			.Group(Tween.Position(_left.transform, _anchors.LeftChoice.position, _anchors.LeftDownwards.position, time, Ease.OutCirc))
			.Group(Tween.Scale(_left.transform, sideScale, Vector3.zero, 0.25f, Ease.InOutSine))
			.Group(Tween.Position(_right.transform, _anchors.RightChoice.position, _anchors.RightDownwards.position, time, Ease.OutCirc))
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
		_left.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = show;
		_current.enabled = show;
		_current.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = show;
		_right.enabled = show;
		_right.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = show;
		_hidden.enabled = show;
		_hidden.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = show;
	}

	public void ShowCurrent(bool show)
	{
		_current.enabled = show;
		_current.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = show;
	}

	public void Release()
	{
		OptionRow = CycleOptionsManager.Instance.Rows[_rowIndex];
		OptionRow.Remove(this);
	}

	public void ChangeLeft(Color color, bool animate)
	{
		Left = color;
		_leftColorChange.Stop();
		if (animate)
		{
			_leftColorChange = Tween.Color(_left, color, 0.1f);
		}
		else
		{
			_left.color = color;
		}
	}

	public void ChangeRight(Color color, bool animate)
	{
		Right = color;
		_rightColorChange.Stop();
		if (animate)
		{
			_rightColorChange = Tween.Color(_right, color, 0.1f);
		}
		else
		{
			_right.color = color;
		}
	}
	public void ChangeCurrent(Color color, bool animate)
	{
		Current = color;
		_currentColorChange.Stop();
		if (animate)
		{
			_currentColorChange = Tween.Color(_current, color, 0.1f);
		}
		else
		{
			_current.color = color;
		}
	}

	public void SetPositionsToInitialForTop()
	{
		_current.transform.position = _anchors.Selection.position;
		_current.transform.localScale = _fullScale;

		_left.transform.position = _anchors.LeftChoice.position;
		_left.transform.localScale = _fullScale * _sideFactor;

		_right.transform.position = _anchors.RightChoice.position;
		_right.transform.localScale = _fullScale * _sideFactor;
	}

	public void SetPreviewAtPosition()
	{
		_current.transform.position = _anchors.Preview.position;
		_current.transform.localScale = _fullScale * _previewFactor;
	}

	public void StopAnimations()
	{
		ProgressAnimation.Complete();
		_cycleAnimation.Complete();
	}
}
