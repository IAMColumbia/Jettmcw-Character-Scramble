using PrimeTween;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _playerNumber;
	[SerializeField] private TextMeshProUGUI _score;
	[SerializeField] private TextMeshProUGUI _placement;
	public GameObject PressAnyButton;

	[SerializeField] private Color _color;
	private int _scoreNum = 0;

	private Sequence _scoreBounce;

	public void Activate()
	{
		Tween.Color(_playerNumber, _color, 0.2f);
		Tween.Color(_score, _color, 0.2f);
		Tween.Color(_placement, _color, 0.2f);
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
