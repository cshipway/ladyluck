using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance { get; private set; }
    [SerializeField] private Image tooltipBg;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;

    private Coroutine c;

    private void Awake()
    {
        Instance = this;
    }

    public static void SetTooltip(Champion champion, CardDefinition cardDef)
    {
        if(Instance.c != null)
            Instance.StopCoroutine(Instance.c);
        Instance.c = Instance.StartCoroutine(Instance.TooltipLingerCoroutine(champion, cardDef));
    }

    private IEnumerator TooltipLingerCoroutine(Champion champion, CardDefinition cardDef)
    {
        float t = 0;

        tooltipBg.enabled = true;
        cardNameText.enabled = true;
        cardDescriptionText.enabled = true;

        while (t < 0.1f)
        {
            cardNameText.text = $"{cardDef.name} ({cardDef.manaCost})";
            cardDescriptionText.text = cardDef.GetDescription(champion);

            t += Time.deltaTime;
            yield return null;
        }

        tooltipBg.enabled = false;
        cardNameText.enabled = false;
        cardDescriptionText.enabled = false;

        c = null;
    }
}
