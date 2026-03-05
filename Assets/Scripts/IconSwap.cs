using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class IconSwap : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;

	[SerializeField] private Sprite[] _keyboardTextures;
	[SerializeField] private Sprite[] _gamepadTextures;

	private Sequence _pressAnimation;

	[SerializeField] private bool RequirePositive;

	public void ChangeTexture(PlayerInput player, int textureIndex)
	{
		string controlScheme = player.currentControlScheme;

		Sprite[] textures = controlScheme switch
		{
			"Keyboard" => _keyboardTextures,
			"Gamepad" => _gamepadTextures,
			_ => throw new System.InvalidOperationException()
		};

		Sprite texture = textures[textureIndex];
		_renderer.sprite = texture;
	}

	public void DoPressAnimationIf(bool positive)
	{
		if (RequirePositive != positive)
		{
			return;
		}
		DoPressAnimation();
	}

	public void DoPressAnimation()
	{
		if (_pressAnimation.isAlive) _pressAnimation.Complete();

		_pressAnimation = Sequence.Create()
			.Group(Tween.Scale(_renderer.transform, 0.45f, 0.35f, 0.1f, Ease.OutSine, 2, CycleMode.Rewind));
		;
	}
}
