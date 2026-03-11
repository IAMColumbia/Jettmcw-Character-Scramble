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

    [SerializeField] private TextMeshProUGUI _roundCounter;
	[SerializeField] private TextMeshProUGUI _timerInstruction;
    [SerializeField] private Timer _timer;

    private int _round = 1;

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
}
