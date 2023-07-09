using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Champion
{
    public string name;
    public int hp;
    public int mp;
    public Deck deck;
    public List<CardDefinition> hand;
    private int fatigue;
    public Champion foe;
    public List<StatusEffectInfo> statusEffects;
    public int Strength => statusEffects.Select(i => i.definition).Sum(d => d.strength);
    public int Armor => statusEffects.Select(i => i.definition).Sum(d => d.armor);

    public Champion(string name, int hp, Deck deck)
    {
        this.name = name;
        this.hp = hp;
        this.deck = deck;
        hand = new List<CardDefinition>();
        statusEffects = new List<StatusEffectInfo>();
    }

    public IEnumerator TakeTurn(BattlefieldManager battlefield, int round)
    {
        mp = round;

        Debug.Log($"[Round {round}]{name}'s turn has begun.");
        yield return battlefield.StartCoroutine(battlefield.ShowBigText($"[R{round}] {name}'s turn!"));

        //Age and expire status effects.
        List<StatusEffectInfo> flaggedForRemoval = new List<StatusEffectInfo>();
        foreach (StatusEffectInfo i in statusEffects)
        {
            i.age++;
            if (i.definition.duration != -1 && i.age >= i.definition.duration)
            {
                flaggedForRemoval.Add(i);
            }
        }
        foreach (StatusEffectInfo i in flaggedForRemoval)
        {
            Debug.Log($"Hero's status effect: '{i.definition}' has worn off!");
            statusEffects.Remove(i);
        }

        if (round == 1)
            yield return battlefield.StartCoroutine(DrawStartingHand(battlefield));
        else
            yield return battlefield.StartCoroutine(DrawPhase(battlefield));

        if (hp > 0)
        {
            while (CanDoSomething())
            {
                yield return battlefield.StartCoroutine(DoSomething(battlefield, round));
            }
        }

        yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} ends their turn!"));
        Debug.Log($"{name} ends their turn!");
    }

    public IEnumerator DrawStartingHand(BattlefieldManager battlefield)
    {
        yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} is about to draw their starting hand..."));

        for (int i = 0; i < 3; i++)
        {
            CardDefinition drawn = deck.Draw();

            if (drawn != null)
            {
                hand.Add(drawn);
                battlefield.PutCardInHand(this, drawn);
                yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} draws {drawn.name}."));
            }
        }
    }

    private IEnumerator DrawPhase(BattlefieldManager battlefield)
    {
        yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} is about to draw a card from their deck..."));

        CardDefinition drawn = deck.Draw();

        if (drawn != null)
        {
            hand.Add(drawn);
            battlefield.PutCardInHand(this, drawn);
            yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} draws {drawn.name}."));
        }
        else
        {
            yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} tries to draw, but their deck is empty!"));

            hp -= ++fatigue;
            yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} takes {fatigue} damage from fatigue."));
        }
    }

    private bool CanDoSomething()
    {
        return foe.hp > 0 && hand.Count > 0 && hand.Any(card => card.manaCost <= mp);
    }

    private IEnumerator DoSomething(BattlefieldManager battlefield, int round)
    {
        string handString = "";
        foreach (CardDefinition cardDefinition in hand)
            handString += $"{cardDefinition.name}, ";
        Debug.Log($"{name}. HP: {hp}. MP: {mp}. Hand: {handString}");

        yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} is picking a card to play..."));

        List<CardDefinition> playableCards = hand.Where(card => card.manaCost <= mp).ToList();
        int random = Random.Range(0, playableCards.Count);
        CardDefinition cardToPlay = playableCards[random];
        hand.Remove(cardToPlay);

        yield return battlefield.StartCoroutine(PlayCard(battlefield, cardToPlay, round));
        
    }

    private IEnumerator PlayCard(BattlefieldManager battlefield, CardDefinition card, int round)
    {
        mp -= card.manaCost;
        Debug.Log($"{name} plays {card.name}!");
        battlefield.RemoveCardFromHand(this, card);
        battlefield.ShowCard(card, this);
        yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} plays {card.name}!"));
        battlefield.StopShowingCard();

        foreach(CardEffect cardEffect in card.cardEffects)
        {
            yield return battlefield.StartCoroutine(ResolveEffect(battlefield, cardEffect));
        }

        foreach(ConditionalEffect conditionalEffect in card.conditionalEffects)
        {
            if(conditionalEffect.condition.Evaluate(round))
                yield return battlefield.StartCoroutine(ResolveEffect(battlefield, conditionalEffect.effect));
        }

        if(card.cardEffects.Count == 0 && card.conditionalEffects.Count == 0)
        {
            yield return battlefield.StartCoroutine(battlefield.ShowReasonableText("..The card has no effect."));
        }
    }

    private IEnumerator ResolveEffect(BattlefieldManager battlefield, CardEffect cardEffect)
    {
        if (cardEffect.damage > 0)
        {
            int d = cardEffect.damage + Strength - foe.Armor;
            if (d < 0)
                d = 0;
            foe.hp -= d;

            Debug.Log($"{name} deals {d} damage to {foe.name}! Foe HP: {foe.hp}");
            yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} deals {d} damage to {foe.name}!"));
        }

        if (cardEffect.healing > 0)
        {
            hp += cardEffect.healing;
            Debug.Log($"{name} heals for {cardEffect.healing}! HP: {hp}");
            yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} heals for {cardEffect.healing}!"));
        }

        foreach (StatusEffectDefinition statusEffect in cardEffect.statusEffects)
        {
            statusEffects.Add(new StatusEffectInfo() { definition = statusEffect });
            Debug.Log($"{name} gains {statusEffect}.");
            yield return battlefield.StartCoroutine(battlefield.ShowReasonableText($"{name} gains {statusEffect}!"));
        }
    }
}