using PrimeTween;
using UnityEngine;

public class AnimatedBackground : MonoBehaviour
{
	[SerializeField] private Color[] _colors;
	[SerializeField] private SpriteRenderer _renderer;
	private int _index;

	private void Start()
	{
		_index = (_index + 1) % _colors.Length;
		Tween.Color(_renderer, _colors[_index], 15f, Ease.InOutSine).OnComplete(Start);
	}
}
