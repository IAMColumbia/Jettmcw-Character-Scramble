using PrimeTween;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _playerNumber;
	[SerializeField] private TextMeshProUGUI _score;
	[SerializeField] private TextMeshProUGUI _placement;
	public TextMeshProUGUI PressAnyButton;

	[SerializeField] private Color _color;
	private int _scoreNum = 0;

	private Sequence _scoreBounce;
	private Sequence _colorChange;

	public void Activate()
	{
		_colorChange.Stop();
		_colorChange = Sequence.Create()
			.Group(Tween.Color(_playerNumber, _color, 0.2f))
			.Group(Tween.Color(_score, _color, 0.2f))
			.Group(Tween.Color(_placement, _color, 0.2f))
			.Group(Tween.Color(PressAnyButton, Color.white, 0.2f))
		;
	}

	public void Deactivate()
	{
		_colorChange.Stop();
		_colorChange = Sequence.Create()
			.Group(Tween.Color(PressAnyButton, new Color(1f, 1f, 1f, 0.2745098f), 0.2f))
		;
	}

	public int Score
	{
		get => _scoreNum;
		set
		{
			_score.text = value.ToString();
			_scoreNum = value;
			ScoreManager.Instance.UpdatePlacements();
			_scoreBounce.Complete();
			_scoreBounce = Sequence.Create().
				Group(Tween.ShakeLocalPosition(_score.transform, new Vector3(0f, 25f, 0f), 0.3f))
			;
		}
	}

	public int Placement
	{
		set
		{
			string written = value switch
			{
				1 => "1st",
				2 => "2nd",
				3 => "3rd",
				4 => "4th",
				_ => throw new System.InvalidOperationException()
			};
			_placement.text = $"{written} Place";
		}
	}
}
