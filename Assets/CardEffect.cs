using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffect
{
    public string name;
    public int damage;
    public int healing;
    public CardDefinition searchCard;
    public bool shuffle;
    public List<StatusEffectDefinition> statusEffects;

    public override string ToString()
    {
        string description = "";
        int count = 0;

        if (damage > 0)
        {
            description += AddDamageToDescription(damage, count);
            count++;
        }

        if (healing > 0)
        {
            description += AddHealingToDescription(healing, count);
            count++;
        }
        if (searchCard != null)
        {
            if (count > 0)
                description += "\n";
            description += $"Search your deck for {searchCard.name} and place it in your hand.";
            count++;
        }
        if (shuffle)
        {
            if (count > 0)
                description += "\n";
            description += "Shuffle your deck.";
            count++;
        }
        foreach (StatusEffectDefinition statusEffect in statusEffects)
        {
            description += AddGainStatusEffectToDescription(statusEffect, count);
            count++;
        }

        return description;
    }

    private string AddDamageToDescription(int damage, int count)
    {
        string toRet = "";
        if (count > 0)
            toRet += "\n";
        toRet += $"Deal {damage} damage.";
        return toRet;
    }

    private string AddHealingToDescription(int amount, int count)
    {
        string toRet = "";
        if (count > 0)
            toRet += "\n";
        toRet += $"Heal for {amount}.";
        return toRet;
    }

    private string AddGainStatusEffectToDescription(StatusEffectDefinition statusEffect, int count)
    {
        string toRet = "";
        if (count > 0)
            toRet += "\n";
        toRet += $"Gain {statusEffect}.";
        return toRet;
    }
}
