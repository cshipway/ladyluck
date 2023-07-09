using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowManager : MonoBehaviour
{
    public static int scenarioIndex = 0;

    [SerializeField] private List<ScenarioDefinition> scenarios;

    private void Start()
    {
        BattlefieldManager.Instance.StartScenario(scenarios[scenarioIndex]);
    }

    public static void NextScenario()
    {
        if (scenarioIndex < 2)
        {
            scenarioIndex++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Application.Quit();
        }
    }

    public static void RestartScenario()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
