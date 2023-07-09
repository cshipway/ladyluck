using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField] private DeckDefinition startingDeck;
    [SerializeField] private Card cardPrefab;

    public void Populate(Champion champion, Deck deck)
    {
        foreach (Card card in GetComponentsInChildren<Card>())
            DestroyImmediate(card.gameObject);

        foreach (CardDefinition cardDefinition in deck.cards)
        {
            Card c = Instantiate(cardPrefab, transform).Bootup(cardDefinition, true, champion);
            foreach (MeshRenderer rend in c.GetComponentsInChildren<MeshRenderer>())
                rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

    }

    public Deck GetDeck()
    {
        List<CardDefinition> cards = new List<CardDefinition>();

        foreach (Card card in GetComponentsInChildren<Card>())
            cards.Add(card.cardDefinition);

        return new Deck() { cards = cards };
    }
}
