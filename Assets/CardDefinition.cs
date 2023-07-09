using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Definition", menuName = "Card Definition")]
public class CardDefinition : ScriptableObject
{
    public Sprite portrait;
    public int manaCost = 1;
    public List<CardEffect> cardEffects;
    public List<ConditionalEffect> conditionalEffects;
    [TextArea] public string appendDescription;

    public Color color = new Color32(62, 59, 70, 255);

    private void OnValidate()
    {
        foreach(CardEffect cardEffect in cardEffects.Concat(conditionalEffects.Select(con => con.effect)))
        {
            cardEffect.name = cardEffect.GetDescription(null);

            foreach (StatusEffectDefinition statusEffect in cardEffect.statusEffects)
            {
                statusEffect.name = statusEffect.ToString();
            }

            if (cardEffect.searchCard != null)
                cardEffect.shuffle = true;
        }

        foreach(ConditionalEffect conditionalEffect in conditionalEffects)
        {
            conditionalEffect.condition.name = conditionalEffect.condition.ToString();
            conditionalEffect.name = conditionalEffect.GetDescription(null);
        }
    }

    public string GetDescription(Champion champion)
    {
        string description = "";
        int count = 0;

        foreach(CardEffect cardEffect in cardEffects)
        {
            if (count > 0)
                description += "\n";
            description += $"{cardEffect.GetDescription(champion)}";
            count++;
        }

        foreach(ConditionalEffect conditionalEffect in conditionalEffects)
        {
            if (count > 0)
                description += "\n";
            description += $"{conditionalEffect.GetDescription(champion)}";
            count++;
        }

        if (!string.IsNullOrEmpty(appendDescription))
        {
            if (count > 0)
                description += "\n";
            description += $"{appendDescription}";
            count++;
        }

        if(description.Contains("strength"))
        {
            if (count > 0)
                description += "\n";
            description += $"<i>Strength: Your strikes deal +1 damage.</i>";
            count++;
        }
        if(description.Contains("armor"))
        {
            if (count > 0)
                description += "\n";
            description += $"<i>Armor: You take -1 damage.</i>";
        }

        return description;

        
    }
}
