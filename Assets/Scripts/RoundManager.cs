using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
	public Color[] PrimeSpecimenColors { get; } = new Color[3];

	[SerializeField] private CycleOptionsManager _rowManager;
    [SerializeField] private SpriteRenderer[] _primeSpecimenSprites = new SpriteRenderer[3];

	void Awake()
    {
        StartNewRound();
        Instance = this;
	}

    public void StartNewRound()
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
        StartNewRound();

        List<PlayerInput> players = PlayerControllerManager.Instance.Players;
        foreach (PlayerInput player in players)
        {
            Progression playerProgression = player.GetComponent<Progression>();
            playerProgression.RestartProgression();
        }
	}
}
