using UnityEngine;
using UnityEngine.InputSystem;

public class IconSwap : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;

	[SerializeField] private Sprite[] _keyboardTextures;
	[SerializeField] private Sprite[] _gamepadTextures;

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
}
