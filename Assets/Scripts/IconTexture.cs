using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;

public class IconTexture : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private int _actionIndex;

	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private Sprite[] _keyboardTextures, _xboxTextures, _psTextures, _switchTextures;

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
		int textureIndex = ControlRandomizer.Instance.FilterIdxs[_actionIndex];
		_renderer.sprite = DetermineSpriteSheet()[textureIndex];
	}

	private Sprite[] DetermineSpriteSheet()
	{
		string controlScheme = _playerInput.currentControlScheme;

		if (controlScheme == "Keyboard")
		{
			return _keyboardTextures;
		}

		string layout = _playerInput.GetDevice<Gamepad>().layout;

		if (layout.Contains("Switch") || layout.Contains("JoyCon"))
		{
			return _switchTextures;
		}

		if (layout.Contains("DualShock") || layout.Contains("DualSense"))
		{
			return _psTextures;
		}

		return _xboxTextures;
	}
}
