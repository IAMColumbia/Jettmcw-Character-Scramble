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
    public bool DoingCharacters = true;

    public UnityEvent RoundPassed;

    private int _round = 1;
    private int _finishers = 0;

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
        Debug.Log("Do next round wait called!");

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
            Debug.Log("From no finishers");
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
		Debug.Log("From resume into next");
		NextRound();
		yield break;
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

        _speedBonus[_finishers].color = new Color(1f, 1f, 1f, 0.2745098f);
		_finishers++;
        if (_finishers < 4)
        {
            _speedBonus[_finishers].color = Color.white;
        }

		return speedBonus;
    }
}
