using UnityEngine;

public class UIToggle : MonoBehaviour
{
	[SerializeField] private SpriteRenderer[] _elements;
	private bool _enabled = true;

	public void DisableUI()
	{
		if (!_enabled)
		{
			return;
		}
		_enabled = false;

		foreach (SpriteRenderer element in _elements)
		{
			element.enabled = false;
		}
	}

	public void EnableUI()
	{
		if (_enabled)
		{
			return;
		}
		_enabled = true;

		foreach (SpriteRenderer element in _elements)
		{
			element.enabled = true;
		}
	}
}
