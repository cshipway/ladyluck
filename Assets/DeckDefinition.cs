using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck Definition", menuName = "Deck Definition")]
public class DeckDefinition : ScriptableObject
{
    public Sprite portrait;
    public int startingHp = 10;
    public Deck deck;
}
