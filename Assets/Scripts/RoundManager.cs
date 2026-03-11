using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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

    private int _round = 1;
    private int _finishers = 0;

	void Awake()
    {
        SetRoundColors();
        Instance = this;
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
            playerProgression.RestartProgression();
        }

        ControlRandomizer.Instance.RandomizeBindings();

        _timer.TimeLeft = 15f;
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
