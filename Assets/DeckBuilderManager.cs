using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilderManager : MonoBehaviour
{
    public static DeckBuilderManager Instance { get; private set; }

    private Deck heroDeck;
    private Deck enemyDeck;

    [SerializeField] private DeckBuilder heroDeckBuilder;
    [SerializeField] private DeckBuilder enemyDeckBuilder;
    [SerializeField] private GameObject activeToggler;
    [SerializeField] private GameObject battlefieldTextActiveToggler;

    public bool deckBuilding;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(!deckBuilding)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                deckBuilding = true;
                activeToggler.SetActive(deckBuilding);
                battlefieldTextActiveToggler.SetActive(!deckBuilding);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                deckBuilding = false;
                activeToggler.SetActive(deckBuilding);
                battlefieldTextActiveToggler.SetActive(!deckBuilding);
            }
        }
    }

    public void SetHeroDeck(Deck deck)
    {
        heroDeck = deck;
        heroDeckBuilder.Populate(heroDeck);
    }

    public void SetEnemyDeck(Deck deck)
    {
        enemyDeck = deck;
        enemyDeckBuilder.Populate(enemyDeck);
    }
}
