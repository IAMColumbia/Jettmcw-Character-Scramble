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

	public void ActivateColors()
	{
		Tween.Color(_playerNumber, _color, 0.2f);
		Tween.Color(_score, _color, 0.2f);
		Tween.Color(_placement, _color, 0.2f);
	}
}
