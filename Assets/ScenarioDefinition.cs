using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scenario Definition", menuName = "Scenario Definition")]
public class ScenarioDefinition : ScriptableObject
{
    [TextArea] public List<string> scenarioIntroStrings;

    public AudioClip scenarioMusicTrack;

    public DeckDefinition heroDeckDefinition;
    public DeckDefinition enemyDeckDefinition;

    [TextArea] public List<string> scenarioOutroStrings;

    
}
