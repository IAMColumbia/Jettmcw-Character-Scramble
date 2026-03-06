using UnityEngine;

public class UIToggle : MonoBehaviour
{
	private bool _enabled = true;

	public void DisableUI()
	{
		if (!_enabled)
		{
			return;
		}
		_enabled = false;

		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(false);
		}
	}

	public void EnableUI()
	{
		if (_enabled)
		{
			return;
		}
		_enabled = true;

		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(true);
		}
	}
}
