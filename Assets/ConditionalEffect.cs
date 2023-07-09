using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[System.Serializable]
public class ConditionalEffect
{
    public string name;
    public Condition condition;
    public CardEffect effect;

    public string GetDescription(Champion champion)
    {
        string toRet = $"{condition}{effect.GetDescription(champion)}";

        return toRet;
    }
}
