using UnityEngine;
using UnityEngine.InputSystem;

public class IconTexture : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private int _actionIndex;

	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private Sprite[] _keyboardTextures, _gamepadTextures;

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
}
