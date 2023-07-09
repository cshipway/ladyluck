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
    [SerializeField] private GameObject ladyLuckPortrait;
    [SerializeField] private GameObject heroPortrait;

    public bool deckBuilding;

    private void Awake()
    {
        Instance = this;
        Card.OnDeckChanged += OnDeckChanged;
    }

    private void OnDestroy()
    {
        Card.OnDeckChanged -= OnDeckChanged;
    }

    private void Update()
    {
        if(!deckBuilding)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                MusicManager.Instance.PlayBuff();
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
                //MusicManager.Instance.PlayShuffle();
                MusicManager.Instance.PlayBuff();
                deckBuilding = false;
                activeToggler.SetActive(deckBuilding);
                battlefieldTextActiveToggler.SetActive(!deckBuilding);
            }
        }

        ladyLuckPortrait.SetActive(deckBuilding);
        heroPortrait.SetActive(!deckBuilding);
    }

    public void OnMatchStart()
    {
        MusicManager.Instance.PlayBuff();
        deckBuilding = true;
        activeToggler.SetActive(deckBuilding);
        battlefieldTextActiveToggler.SetActive(!deckBuilding);
        heroDeckBuilder.Populate(BattlefieldManager.Instance.hero, heroDeck);
        enemyDeckBuilder.Populate(BattlefieldManager.Instance.enemy, enemyDeck);
    }

    public void SetHeroDeck(Champion champion,Deck deck)
    {
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
