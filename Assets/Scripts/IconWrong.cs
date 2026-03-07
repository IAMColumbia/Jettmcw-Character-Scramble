using PrimeTween;
using UnityEngine;

public class IconWrong : MonoBehaviour
{
	[SerializeField] private bool _requirePositive;
	[SerializeField] private SpriteRenderer _renderer;

	private Sequence _pressAnimation;

	public void DoWrongAnimationIf(bool positive)
	{
		if (_requirePositive == positive)
		{
			DoWrongAnimation();
		}
	}

	public void DoWrongAnimation()
	{
		if (_pressAnimation.isAlive) _pressAnimation.Complete();

		Color start = _renderer.color;
		_pressAnimation = Sequence.Create()
			.Group(Tween.Color(_renderer, start, Color.red, 0.1f, Ease.OutSine, 2, CycleMode.Rewind));
		;
	}
}
