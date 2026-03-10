using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private CycleOptionsManager _rowManager;
    public Color[] PrimeSpecimenColors = new Color[3];
    [SerializeField] private SpriteRenderer[] _primeSpecimenSprites = new SpriteRenderer[3];

	void Awake()
    {
        StartNewRound();
	}

    public void StartNewRound()
    {
        _rowManager.SetColors();
        
        for (int i = 0; i < 3; i++)
        {
			PrimeSpecimenColors[i] = Utility.Choose(_rowManager.Rows[i].Options).First(c => !PrimeSpecimenColors.Take(i).Contains(c));
            _primeSpecimenSprites[i].color = PrimeSpecimenColors[i];

		}


	}
}
