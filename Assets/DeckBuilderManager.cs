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
        Card.OnDeckChanged += OnDeckChanged;
        BattlefieldManager.OnMatchStart += OnMatchStart;
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
                heroDeckBuilder.Populate(BattlefieldManager.Instance.hero, heroDeck);
                enemyDeckBuilder.Populate(BattlefieldManager.Instance.enemy, enemyDeck);
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

    private void OnMatchStart()
    {
        deckBuilding = true;
        activeToggler.SetActive(deckBuilding);
        battlefieldTextActiveToggler.SetActive(!deckBuilding);
        heroDeckBuilder.Populate(BattlefieldManager.Instance.hero, heroDeck);
        enemyDeckBuilder.Populate(BattlefieldManager.Instance.enemy, enemyDeck);
    }

    public void SetHeroDeck(Champion champion,Deck deck)
    {
        Debug.Log("DBM Setting Hero deck");
        heroDeck = deck;
        heroDeckBuilder.Populate(champion, heroDeck);
    }

    public void SetEnemyDeck(Champion champion, Deck deck)
    {
        enemyDeck = deck;
        enemyDeckBuilder.Populate(champion, enemyDeck);
    }

    private void OnDeckChanged()
    {
        heroDeck.cards.Clear();
        foreach (Card card in heroDeckBuilder.GetComponentsInChildren<Card>())
            heroDeck.cards.Add(card.cardDefinition);

        enemyDeck.cards.Clear();
        foreach (Card card in enemyDeckBuilder.GetComponentsInChildren<Card>())
            enemyDeck.cards.Add(card.cardDefinition);
    }
}
