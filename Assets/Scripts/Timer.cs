using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
	public UnityEvent OnComplete;
	public float TimeLeft;
	public bool IsPaused;

	[SerializeField] private TextMeshProUGUI _tens;
	[SerializeField] private TextMeshProUGUI _ones;
	[SerializeField] private TextMeshProUGUI _dot;
	[SerializeField] private TextMeshProUGUI _tenths;

	private Sequence _bounce; 

	public void Update()
	{
		if (IsPaused || TimeLeft == 0)
		{
			return;
		}

		TimeLeft -= Time.deltaTime;

		if (TimeLeft <= 0)
		{
			TimeLeft = 0;
			OnComplete.Invoke();
		}

		bool hasTensPlace = TimeLeft >= 10;
		_tens.gameObject.SetActive(hasTensPlace);
		if (hasTensPlace)
		{
			_tens.text = Mathf.FloorToInt(TimeLeft / 10).ToString();
		}
		string onesPlace = Mathf.FloorToInt(TimeLeft % 10).ToString();
		if (onesPlace != _ones.text || TimeLeft == 0)
		{
			_bounce.Complete();

			float intensity = Mathf.Max(1.1f, Mathf.Pow(1.03f, 15f - TimeLeft));
			_bounce = Sequence.Create()
				.Group(Tween.Scale(transform, Vector3.one, Vector3.one * intensity, 0.15f * intensity, Ease.OutCirc, 2, CycleMode.Rewind))
				.Group(Tween.ShakeLocalPosition(transform, new Vector3(5f, 10f), 0.3f * intensity))
			;
		}
		_ones.text = onesPlace;
		_tenths.text = Mathf.FloorToInt(TimeLeft * 10 % 10).ToString();

		Color color = Color.Lerp(new Color(1f, 0.3f, 0.3f), Color.white, TimeLeft / 5f);
		_tens.color = color;
		_ones.color = color;
		_dot.color = color;
		_tenths.color = color;

	}
}
