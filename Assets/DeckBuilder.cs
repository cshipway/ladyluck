using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField] private DeckDefinition startingDeck;
    [SerializeField] private Card cardPrefab;

    [ContextMenu("Reset to Deck Definition")]
    private void ResetToDeckDefinition()
    {
        foreach (Card card in GetComponentsInChildren<Card>())
            DestroyImmediate(card.gameObject);

        foreach (CardDefinition cardDefinition in startingDeck.deck.cards)
            Instantiate(cardPrefab, transform).Bootup(cardDefinition, true);
    }

    public void Populate(Deck deck)
    {
        foreach (Card card in GetComponentsInChildren<Card>())
            DestroyImmediate(card.gameObject);

        foreach (CardDefinition cardDefinition in deck.cards)
            Instantiate(cardPrefab, transform).Bootup(cardDefinition, true);
    }

    public Deck GetDeck()
    {
        List<CardDefinition> cards = new List<CardDefinition>();

        foreach (Card card in GetComponentsInChildren<Card>())
            cards.Add(card.cardDefinition);

        return new Deck() { cards = cards };
    }
}
