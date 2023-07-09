using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scenario Definition", menuName = "Scenario Definition")]
public class ScenarioDefinition : ScriptableObject
{
    public DeckDefinition heroDeckDefinition;
    public DeckDefinition enemyDeckDefinition;
}
