using PrimeTween;
using UnityEngine;

public class IconWrong : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;

	private Sequence _pressAnimation;

	public void DoWrongAnimation()
	{
		_pressAnimation.Complete();

		Color start = _renderer.color;
		_pressAnimation = Sequence.Create()
			.Group(Tween.Color(_renderer, start, Color.red, 0.1f, Ease.OutSine, 2, CycleMode.Rewind))
			.Group(Tween.ShakeLocalPosition(transform, Vector3.one * 0.4f, 0.2f))
		;

		SFXManager.PlayIncorrectSound();
	}
}
