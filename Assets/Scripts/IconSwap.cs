using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class IconSwap : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private int _actionIndex;

	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private Sprite[] _keyboardTextures, _gamepadTextures;

	private Sequence _pressAnimation;

	[SerializeField] private bool RequirePositive;

	private void OnEnable()
	{
		ControlRandomizer.Instance.ControlSchemeChanged.AddListener(ChangeTexture);
		ChangeTexture();
	}
	private void OnDisable()
	{
		ControlRandomizer.Instance.ControlSchemeChanged.RemoveListener(ChangeTexture);
	}

	public void ChangeTexture()
	{
		string controlScheme = _playerInput.currentControlScheme;
		Sprite[] textures = controlScheme switch
		{
			"Keyboard" => _keyboardTextures,
			"Gamepad" => _gamepadTextures,
			_ => throw new System.InvalidOperationException()
		};

		int textureIndex = ControlRandomizer.Instance.FilterIdxs[_actionIndex];

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
