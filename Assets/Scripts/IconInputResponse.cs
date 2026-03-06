using PrimeTween;
using UnityEngine;

public class IconInputResponse : MonoBehaviour
{
	[SerializeField] private bool _requirePositive;

	private Sequence _pressAnimation;

	public void DoPressAnimationIf(bool positive)
	{
		if (_requirePositive == positive)
		{
			DoPressAnimation();
		}
	}

	public void DoPressAnimation()
	{
		if (_pressAnimation.isAlive) _pressAnimation.Complete();

		_pressAnimation = Sequence.Create()
			.Group(Tween.Scale(transform, 0.45f, 0.35f, 0.1f, Ease.OutSine, 2, CycleMode.Rewind));
		;
	}
}
