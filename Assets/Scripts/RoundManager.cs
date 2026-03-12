using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
	public Color[] PrimeSpecimenColors { get; } = new Color[3];

	[SerializeField] private CycleOptionsManager _rowManager;
    [SerializeField] private SpriteRenderer[] _primeSpecimenSprites = new SpriteRenderer[3];
	[SerializeField] private TextMeshProUGUI[] _speedBonus = new TextMeshProUGUI[4];

	[SerializeField] private TextMeshProUGUI _roundCounter;
	[SerializeField] private TextMeshProUGUI _timerInstruction;
    [SerializeField] private Timer _timer;
	[SerializeField] private Transform _stripes;
    public bool DoingCharacters = true;

    public UnityEvent RoundPassed;

    private int _round = 1;
    private int _finishers = 0;

	private Sequence NumberSizing;
	private Sequence StripesBonus;

	void Awake()
    {
        SetRoundColors();
        Instance = this;
        _timer.OnComplete.AddListener(DoNextRoundWait);
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (DoingCharacters && _finishers == (_round > 1 ? PlayerControllerManager.Instance.Players.Count : 4))
        {
            DoNextRoundWait();
		}
	}

	public void DoNextRoundWait() => StartCoroutine(NextRoundWait());

	private IEnumerator NextRoundWait()
    {
		DoingCharacters = false;
		_timer.IsPaused = true;
		PlayerInputManager.instance.DisableJoining();

		foreach (PlayerInput player in PlayerControllerManager.Instance.Players)
		{
			Progression playerProgression = player.GetComponent<Progression>();
			if (!playerProgression.Finished)
			{
				playerProgression.RestartProgression();
				player.DeactivateInput();
				PlayerAreas.GetArea(player).PressAnyButton.text = "Did Not Finish";
			}
		}

		if (_round == 1)
		{
			for (int i = PlayerControllerManager.Instance.Players.Count; i < 4; i++)
			{
				PlayerAreas.GetStats(i).Deactivate();
			}
		}

		yield return new WaitForSeconds(1f);

        // If there are finished players, give them time to play
        // Otherwise, start next round immediately

        if (_finishers > 0)
        {
            _timerInstruction.text = "[Next Round In]";
            _timer.TimeLeft = 3f;
			_timer.IsPaused = false;
            _timer.TurnRed = false;
			_timer.OnComplete.RemoveListener(DoNextRoundWait);
            _timer.OnComplete.AddListener(ResumeIntoNext);
        }
        else
		{
			ResumeGame();
			NextRound();
		}
	}

    public void ResumeIntoNext() => StartCoroutine(ResumeNextRound());

    private IEnumerator ResumeNextRound()
	{
		yield return new WaitForSeconds(1f);

		_timerInstruction.text = "[Choose Your Character]";
		_timer.OnComplete.RemoveListener(ResumeIntoNext);
		_timer.OnComplete.AddListener(DoNextRoundWait);
		ResumeGame();
		NextRound();
    }

	public void ResumeGame()
    {
        _timer.IsPaused = false;
		DoingCharacters = true;
		PlayerInputManager.instance.EnableJoining();
	}

    public void SetRoundColors()
    {
        _rowManager.SetColors();
        
        // Update Combo UI
        for (int i = 0; i < 3; i++)
        {
			PrimeSpecimenColors[i] = Utility.Choose(_rowManager.Rows[i].Options).First(c => !PrimeSpecimenColors.Take(i).Contains(c));
            _primeSpecimenSprites[i].color = PrimeSpecimenColors[i];
		}
	}

    public void NextRound()
    {
        SetRoundColors();
        _finishers = 0;
		_speedBonus[0].color = Color.white;
        for (int i = 1; i < 4; i++)
        {
            _speedBonus[i].color = new Color(1f, 1f, 1f, 0.2745098f);
		}

		List<PlayerInput> players = PlayerControllerManager.Instance.Players;
        foreach (PlayerInput player in players)
        {
            Progression playerProgression = player.GetComponent<Progression>();
            playerProgression.FinalCharacter.localScale = Vector3.one;
            playerProgression.RestartProgression();
			PlayerAreas.GetArea(player).PressAnyButton.text = "Press Any Button";
			PlayerAreas.GetArea(player).ControlsChanged(true);
		}

        ControlRandomizer.Instance.RandomizeBindings();

        _timer.TimeLeft = 15f;
		_timer.TurnRed = true;
		_round++;
        _roundCounter.text = _round.ToString();
	}

    public int GetSpeedBonus()
    {
		int speedBonus = _finishers switch {
            0 => 4,
            1 => 3,
            2 => 2,
            3 => 1,
            _ => throw new System.InvalidOperationException()
        };

		Color clear = new(1f, 1f, 1f, 0.2745098f);

		NumberSizing.Complete();
		NumberSizing = Sequence.Create()
			.Group(Tween.Scale(_speedBonus[_finishers].transform, 1f, 3f, 0.75f, Ease.OutCirc, 2, CycleMode.Rewind))
			.Group(Tween.Color(_speedBonus[_finishers], Color.white, clear, 0.75f, startDelay: 1f))
		;

		_finishers++;
        if (_finishers < 4)
        {
			NumberSizing.Group(Tween.Color(_speedBonus[_finishers], clear, Color.white, 0.75f, startDelay: 1f));
        }

		return speedBonus;
    }

	public void DoStripesBonus()
	{
		StripesBonus.Complete();
		StripesBonus = Sequence.Create()
			.Group(Tween.Scale(_stripes, 1f, 2f, 0.75f, Ease.OutCirc, 2, CycleMode.Rewind))
		;
	}
}
