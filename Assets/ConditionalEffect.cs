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

    public override string ToString()
    {
        string toRet = $"{condition}{effect}";

        return toRet;
    }
}
