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

    public string GetDescription(Champion champion)
    {
        string description = "";
        int count = 0;

        if (damage > 0)
        {
            description += AddDamageToDescription(champion, damage, count);
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

    private string AddDamageToDescription(Champion champion, int damage, int count)
    {
        string toRet = "";
        if (count > 0)
            toRet += "\n";

        bool isDamageAdjusted = false;

        int d = damage;
        isDamageAdjusted = (champion != null && champion.foe != null) && (champion.Strength > 0 || champion.foe.Armor > 0);
        if (isDamageAdjusted)
            d = damage + (champion.Strength * champion.StrengthMultiplier) - champion.foe.Armor;

        toRet += $"Deal {(isDamageAdjusted ? $"*{d}*" : d)} damage.";
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
