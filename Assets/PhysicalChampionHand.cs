using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalChampionHand : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;

    public void PutCardInHand(Champion champion, CardDefinition card)
    {
        LayoutElement le = Instantiate(cardPrefab, transform).Bootup(card, false, champion).AddComponent<LayoutElement>();
        le.flexibleWidth = 1;
        foreach (Image image in le.GetComponentsInChildren<Image>())
            image.raycastTarget = false;
    }

    public void RemoveCardFromHand(CardDefinition cardDefinition)
    {
        Card toRemove = null;
        foreach(Card card in GetComponentsInChildren<Card>())
        {
            if(card.cardDefinition == cardDefinition)
            {
                toRemove = card;
                break;
            }
        }

        if(toRemove != null)
        {
            Destroy(toRemove.gameObject);
        }
    }
}
