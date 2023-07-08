using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
    public List<CardDefinition> cards;

    public CardDefinition Draw()
    {
        CardDefinition toRet;

        if (cards.Count > 0)
        {
            toRet = cards[0];
            cards.RemoveAt(0);
        }
        else
            toRet = null;

        return toRet;

    }
}
