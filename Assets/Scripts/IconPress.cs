using PrimeTween;
using UnityEngine;

public class IconPress : MonoBehaviour
{
	[SerializeField] private Vector3 _move;
	private Sequence _pressAnimation;

	public void DoPressAnimation()
	{
		_pressAnimation.Complete();

		_pressAnimation = Sequence.Create()
			.Group(Tween.Scale(transform, 0.45f, 0.35f, 0.1f, Ease.OutSine, 2, CycleMode.Rewind))
			.Group(Tween.LocalPosition(transform.parent, Vector3.zero, _move, 0.1f, Ease.OutCirc, 2, CycleMode.Rewind))
		;
	}
}
